using DietAppClient.Data;
using DietAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Logics
{
    public class StattLogic : IStattLogic
    {
        IEatingRepository _eatingRepo;
        IUserRepository _userRepo;
        Baseline baseline;
        Intervention goalIntervention;
        Intervention goalMaintIntervention;

        public StattLogic(IUserRepository userRepo, IEatingRepository eatingRepo)
        {
            _userRepo = userRepo;
            _eatingRepo = eatingRepo;
            User user = userRepo.Read();
            baseline = new Baseline(
                user.Sex == "Male",
                user.Age,
                user.Height,
                user.Weight,
                GetPal(user.WorkActivity, user.FreeTimeActivity));
        }

        public double[] GetBmis(double goalWeight)
        {
            User user = _userRepo.Read();
            return new double[] { user.Weight / Math.Pow(user.Height / 100.0, 2.0), goalWeight / Math.Pow(user.Height / 100.0, 2.0) };
        }

        public double GetPreMaintCols()
        {
            return baseline.GetMaintCals();
        }

        private double GetPal(string workActivity, string freeTimeActivity)
        {
            double result = 1;

            switch (workActivity)
            {
                case "Very light":
                    result += 0.2;
                    break;
                case "Light":
                    result += 0.3;
                    break;
                case "Moderate":
                    result += 0.4;
                    break;
                case "Heavy":
                    result += 0.5;
                    break;
            }

            switch (workActivity)
            {
                case "Very light":
                    result += 0.2;
                    break;
                case "Light":
                    result += 0.3;
                    break;
                case "Moderate":
                    result += 0.4;
                    break;
                case "Active":
                    result += 0.5;
                    break;
                case "Very active":
                    result += 0.7;
                    break;
            }
            return result;
        }

        public double[] GenerateGoalInterventions(double goalWeight, int goalTime)
        {
            double[] range = baseline.GetHealthyWeightRange();
            if (goalWeight < range[0] || goalWeight > range[1])
                throw new Exception($"Goal is not healthy, healthy range: {range[0]} - {range[1]}");

            goalIntervention = new Intervention();
            goalMaintIntervention = goalIntervention;
            goalIntervention = goalIntervention.CheckGoal(baseline, goalWeight, goalTime);

            BodyModel goalBody = new BodyModel(baseline, goalIntervention, goalTime + 1);
            double goalMaintCals;

            if (goalWeight == baseline.Weight)
                goalMaintCals = baseline.GetMaintCals();
            else
                goalMaintCals = goalBody.Cals4balance(baseline, goalMaintIntervention.GetAct(baseline));

            goalMaintIntervention.Day = goalTime + 1;
            goalMaintIntervention.Calories = goalMaintCals;
            goalMaintIntervention.Carbinpercent = baseline.CarbIntakePct;
            goalMaintIntervention.SetProportionalSodium(baseline);

            return new double[2] { goalIntervention.Calories, goalMaintIntervention.Calories };
        }

        public ChartDataSet[] GetChartDatas(int simlength)
        {
            DailyParams[] paramtraj = DailyParams.GetParamtray(baseline, goalIntervention, goalMaintIntervention, simlength);
            BodyModel[] bodytraj = new BodyModel[simlength];
            ChartDataSet[] chartDataSets = new ChartDataSet[simlength];

            bodytraj[0] = new BodyModel(baseline);
            chartDataSets[0] = new ChartDataSet()
            {
                Weight = bodytraj[0].GetWeight(baseline),
                FatPercent = bodytraj[0].GetFatPercent(baseline),
                Intake = paramtraj[0].Calories,
                Expenditure = bodytraj[0].GetExpend(baseline, paramtraj[0]),
            };

            for (int i = 1; i < simlength; i++)
            {
                DailyParams dparams = paramtraj[i];
                bodytraj[i] = BodyModel.GetBodytraj(bodytraj[i - 1], baseline, dparams);
                chartDataSets[i] = new ChartDataSet()
                {
                    Weight = bodytraj[i].GetWeight(baseline),
                    FatPercent = bodytraj[i].GetFatPercent(baseline),
                    Intake = paramtraj[i].Calories,
                    Expenditure = bodytraj[i].GetTEE(baseline, paramtraj[i]),
                };
            }

            return chartDataSets;
        }

        public double GetAverageDailyCalorie()
        {
            List<int> dailyCalories = new List<int>();

            if (_eatingRepo.ReadAll().Count() == 0)
                return 0;

            foreach (var item in _eatingRepo.ReadAll().GroupBy(t => t.Date.ToString("yyyy-MM-dd")))
            {
                dailyCalories.Add(item.Sum(t => t.Fat + t.Protein + t.Carbohydrate));
            }
            return dailyCalories.Average();
        }
    }
}
