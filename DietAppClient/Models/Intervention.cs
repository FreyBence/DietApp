using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Models
{
    public class Intervention
    {
        int day;
        double calories;
        double carbinpercent;
        double sodium;

        public Intervention(int day, double calories, double carbinpercent) {
            Day = day;
            Calories = calories;
            Carbinpercent =  carbinpercent;
        }

        public Intervention() 
            : this(100, 2200, 50)
        {}

        public int Day { get => day; set => day = value; }
        public double Calories { get => calories; set => calories = value; }
        public double Carbinpercent { get => carbinpercent; set => carbinpercent = value; }
        public double Sodium { get => sodium; set => sodium = value; }

        public Intervention CheckGoal(Baseline baseline, double goalWeight, int goalTime)
        {
            double holdcals;
            Intervention goalInter = new Intervention(1, 0, baseline.CarbIntakePct);

            goalInter.SetProportionalSodium(baseline);

            if (baseline.Weight == goalWeight)
            {
                goalInter.calories = baseline.GetMaintCals();
                goalInter.SetProportionalSodium(baseline);
            }
            else
            {
                BodyModel starvTest = new BodyModel(baseline, goalInter, goalTime);

                double starvWeight = starvTest.GetWeight(baseline);
                starvWeight = (starvWeight < 0) ? 0 : starvWeight;

                double error = Math.Abs(starvWeight - goalWeight);

                if ((error < 0.001) || (goalWeight <= starvWeight))
                {
                    goalInter.Calories = 0.0;
                    throw new Exception("Unachievable Goal");
                }

                double checkcals = 0;
                double calstep = 200;

                double PCXerror = 0;
                do
                {
                    holdcals = checkcals;
                    checkcals += calstep;

                    goalInter.Calories = checkcals;
                    goalInter.SetProportionalSodium(baseline);

                    BodyModel testBody = new BodyModel(baseline, goalInter, goalTime);
                    double testWeight = testBody.GetWeight(baseline);

                    if (testWeight < 0)
                    {
                        PCXerror++;
                        if (PCXerror > 10)
                        {
                            throw new Exception("Unachievable Goal");
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

        public double GetAct(Baseline baseline)
        {
            return baseline.GetActivityParam();
        }

        public void SetProportionalSodium(Baseline baseline)
        {
            sodium = baseline.Sodium * calories / baseline.GetMaintCals();
        }
    }
}
