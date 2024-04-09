using DietAppClient.Exceptions;
using DietAppClient.Models;
using System.Diagnostics;

namespace DietAppClient.Logics
{
    public class InterventionLogic : IInterventionLogic
    {
        IBaselineLogic _baselineLogic;
        IBodyModelLogic _bodyModelLogic;

        public InterventionLogic(IBaselineLogic baselineLogic, IBodyModelLogic bodyModelLogic)
        {
            _baselineLogic = baselineLogic;
            _bodyModelLogic = bodyModelLogic;
        }

        public Intervention CheckGoal(Baseline baseline, double goalWeight, int goalTime)
        {
            double holdcals;
            Intervention goalInter = new Intervention(1, 0, baseline.CarbIntakePct);

            SetProportionalSodium(goalInter, baseline);

            if (baseline.Weight == goalWeight)
            {
                goalInter.Calories = _baselineLogic.GetMaintCals(baseline);
                SetProportionalSodium(goalInter, baseline);
            }
            else
            {
                BodyModel starvTest = _bodyModelLogic.GenerateBodyModel(baseline, goalInter, goalTime);

                double starvWeight = _bodyModelLogic.GetWeight(starvTest, baseline);
                starvWeight = (starvWeight < 0) ? 0 : starvWeight;

                double error = Math.Abs(starvWeight - goalWeight);

                if ((error < 0.001) || (goalWeight <= starvWeight))
                {
                    throw new HealthCheckException("Unreachable Goal");
                }

                double checkcals = 0;
                double calstep = 200;

                double PCXerror = 0;
                do
                {
                    holdcals = checkcals;
                    checkcals += calstep;

                    goalInter.Calories = checkcals;
                    SetProportionalSodium(goalInter, baseline);

                    BodyModel testBody = _bodyModelLogic.GenerateBodyModel(baseline, goalInter, goalTime);
                    double testWeight = _bodyModelLogic.GetWeight(testBody, baseline);

                    if (testWeight < 0)
                    {
                        PCXerror++;
                        if (PCXerror > 10)
                        {
                            throw new HealthCheckException("Unreachable Goal");
                        }
                    }
                    error = Math.Abs(goalWeight - testWeight);

                    if ((error > 0.001) && (testWeight > goalWeight))
                    {
                        calstep /= 2;
                        checkcals = holdcals;
                    }
                } while (error > 0.001);
            }
            return goalInter;
        }

        public void SetProportionalSodium(Intervention intervention, Baseline baseline)
        {
            intervention.Sodium = baseline.Sodium * intervention.Calories / _baselineLogic.GetMaintCals(baseline);
        }
    }
}
