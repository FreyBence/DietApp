using DietAppClient.Data;
using DietAppClient.Exceptions;
using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public class StattLogic : IStattLogic
    {
        IEatingRepository _eatingRepo;
        IUserRepository _userRepo;
        IRecordRepository _recordRepo;
        IBaselineLogic _baselineLogic;
        IBodyModelLogic _bodyModelLogic;
        IInterventionLogic _interventionLogic;
        IDailyParamsLogic _dailyParamsLogic;

        public StattLogic(IUserRepository userRepo, IEatingRepository eatingRepo, IRecordRepository recordRepo,
            IBaselineLogic baselineLogic, IBodyModelLogic bodyModelLogic, IInterventionLogic interventionLogic, IDailyParamsLogic dailyParamsLogic)
        {
            _userRepo = userRepo;
            _eatingRepo = eatingRepo;
            _recordRepo = recordRepo;
            _baselineLogic = baselineLogic;
            _bodyModelLogic = bodyModelLogic;
            _interventionLogic = interventionLogic;
            _dailyParamsLogic = dailyParamsLogic;
        }

        public (Dictionary<string, double>, ChartDataSet[]) GetDietDatas(double goalWeight, int goalTime, int simlength = 365)
        {
            Baseline baseline = GetBaslineFromUser();
            (Intervention goalIntervention, Intervention goalMaintIntervention) = GenerateGoalInterventions(goalWeight, goalTime, baseline);
            List<Record> records = _recordRepo.ReadAll().ToList();
            DailyParams[] paramtraj = _dailyParamsLogic.GetParamtray(baseline, goalIntervention, goalMaintIntervention, simlength);
            BodyModel[] bodytraj = new BodyModel[simlength];
            ChartDataSet[] chartDataSets = new ChartDataSet[simlength];

            bodytraj[0] = _bodyModelLogic.GenerateBodyModel(baseline);
            chartDataSets[0] = new ChartDataSet()
            {
                Weight = _bodyModelLogic.GetWeight(bodytraj[0], baseline),
                FatPercent = _bodyModelLogic.GetFatPercent(bodytraj[0], baseline),
                Intake = paramtraj[0].Calories,
                Expenditure = _bodyModelLogic.GetExpend(bodytraj[0], baseline, paramtraj[0]),
            };

            foreach (var param in paramtraj)
            {
                var recordCals = records.Where(t => t.Date.ToString("yyyy-MM-dd") == param.Date.ToString("yyyy-MM-dd"));
                if (recordCals.Count() != 0)
                    param.Calories = recordCals.Sum(t => (double)t.Calories);
            }

            for (int i = 1; i < simlength; i++)
            {
                DailyParams dparams = paramtraj[i];
                bodytraj[i] = _bodyModelLogic.GetBodytraj(bodytraj[i - 1], baseline, dparams);
                chartDataSets[i] = new ChartDataSet()
                {
                    Weight = _bodyModelLogic.GetWeight(bodytraj[i], baseline),
                    FatPercent = _bodyModelLogic.GetFatPercent(bodytraj[i], baseline),
                    Intake = paramtraj[i].Calories,
                    Expenditure = _bodyModelLogic.GetTEE(bodytraj[i], baseline, paramtraj[i])
                };
            }

            (double preBmi, double postBmi) = GetBmis(goalWeight);
            Dictionary<string, double> dietDatas = new Dictionary<string, double>()
            {
                ["AvgDailyCols"] = GetAverageDailyCalorie(),
                ["PreMaintCols"] = _baselineLogic.GetMaintCals(baseline),
                ["ProcessCols"] = goalIntervention.Calories,
                ["PostMaintCols"] = goalMaintIntervention.Calories,
                ["PreWeight"] = chartDataSets[0].Weight,
                ["PostWeight"] = chartDataSets[364].Weight,
                ["PreFatP"] = chartDataSets[0].FatPercent,
                ["PostFatP"] = chartDataSets[364].FatPercent,
                ["PreBmi"] = preBmi,
                ["PostBmi"] = postBmi
            };

            return (dietDatas, chartDataSets);
        }

        private (Intervention, Intervention) GenerateGoalInterventions(double goalWeight, int goalTime, Baseline baseline)
        {
            double[] range = _baselineLogic.GetHealthyWeightRange(baseline);
            if (goalWeight < range[0] || goalWeight > range[1])
                throw new HealthCheckException($"Goal is not recommended, healthy weight range: {range[0]} - {range[1]}");

            Intervention goalIntervention = _interventionLogic.CheckGoal(baseline, goalWeight, goalTime);
            Intervention goalMaintIntervention = new Intervention();

            BodyModel goalBody = _bodyModelLogic.GenerateBodyModel(baseline, goalIntervention, goalTime + 1);
            double goalMaintCals;

            if (goalWeight == baseline.Weight)
                goalMaintCals = _baselineLogic.GetMaintCals(baseline);
            else
                goalMaintCals = _bodyModelLogic.Cals4balance(goalBody, baseline, _baselineLogic.GetActivityParam(baseline));

            goalMaintIntervention.Day = goalTime + 1;
            goalMaintIntervention.Calories = goalMaintCals;
            goalMaintIntervention.Carbinpercent = baseline.CarbIntakePct;
            _interventionLogic.SetProportionalSodium(goalMaintIntervention, baseline);

            return (goalIntervention, goalMaintIntervention);
        }

        private (double, double) GetBmis(double goalWeight)
        {
            User user = _userRepo.Read();
            return ((double)user.Weight / Math.Pow((double)user.Height / 100.0, 2.0), goalWeight / Math.Pow((double)user.Height / 100.0, 2.0));
        }

        private double GetAverageDailyCalorie()
        {
            List<int> dailyCalories = new List<int>();

            if (_eatingRepo.ReadAll().Count() == 0)
                return 0;

            foreach (var item in _eatingRepo.ReadAll().GroupBy(t => t.Date.ToString("yyyy-MM-dd")))
            {
                dailyCalories.Add(item.Sum(t => (int)t.Fat + (int)t.Protein + (int)t.Carbohydrate));
            }
            return dailyCalories.Average();
        }

        private Baseline GetBaslineFromUser()
        {
            User user = _userRepo.Read();
            return new Baseline(
                user.Sex == "Male",
                (int)user.Age,
                (double)user.Height,
                (double)user.Weight,
                GetPal(user.WorkActivity, user.FreeTimeActivity),
                user.Date);
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

            switch (freeTimeActivity)
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
    }
}
