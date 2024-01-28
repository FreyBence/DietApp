using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Models
{
    public class DailyParams
    {
        double calories;
        double carbpercent;
        double sodium;
        double actparam;

        public double Calories { get => calories; set => calories = value; }
        public double Carbpercent { get => carbpercent; set => carbpercent = value; }
        public double Sodium { get => sodium; set => sodium = value; }
        public double Actparam { get => actparam; set => actparam = value; }

        public DailyParams(double calories, double carbpercent, double sodium, double actparam) {
            Calories = calories;
            Carbpercent = carbpercent;
            Sodium = sodium;
            Actparam = actparam;
        }

        public DailyParams(Baseline baseline) 
            : this(baseline.GetMaintCals(), baseline.CarbIntakePct, baseline.Sodium, baseline.GetActivityParam())
        {}

        public DailyParams(Intervention intervention, Baseline baseline) 
            : this(intervention.Calories, intervention.Carbinpercent, intervention.Sodium, intervention.GetAct(baseline))
        {}

        public double GetCarbIntake()
        {
            return carbpercent / 100.0 * calories;
        }

        public static DailyParams[] GetParamtray(Baseline baseline, Intervention intervention1, Intervention intervention2, int simlength)
        {
            DailyParams[] paramtraj = new DailyParams[simlength];
            bool noeffect1 = intervention1.Day > simlength;
            bool noeffect2 = intervention2.Day > simlength;
            bool noeffect = noeffect1 && noeffect2;

            paramtraj[0] = new DailyParams(baseline);

            if (noeffect)
            {
                for (int i = 1; i < simlength; i++)
                {
                    paramtraj[i] = new DailyParams(baseline);
                }
            }
            else
            {
                Intervention firstIntervention = intervention1.Day < intervention2.Day ? intervention1 : intervention2;
                Intervention secondIntervention = intervention1.Day < intervention2.Day ? intervention2 : intervention1;

                for (int i = 1; i < firstIntervention.Day; i++)
                {
                    paramtraj[i] = new DailyParams(baseline);
                }

                int endfirst = Math.Min(secondIntervention.Day, simlength);

                for (int i = firstIntervention.Day; i < endfirst; i++)
                {
                    paramtraj[i] = new DailyParams(firstIntervention, baseline);
                }

                if (simlength > secondIntervention.Day)
                {
                    for (int i = secondIntervention.Day; i < simlength; i++)
                    {
                        paramtraj[i] = new DailyParams(secondIntervention, baseline);
                    }
                }
            }
            return paramtraj;
        }
    }
}
