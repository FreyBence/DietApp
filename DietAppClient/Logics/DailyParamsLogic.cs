using DietAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Logics
{
    public class DailyParamsLogic : IDailyParamsLogic
    {
        private IBaselineLogic _baselineLogic;

        public DailyParamsLogic(IBaselineLogic baselineLogic)
        {
            _baselineLogic = baselineLogic;
        }
        public DailyParams GenerateDailyParams(Baseline baseline, DateTime date)
        {
            return new DailyParams(_baselineLogic.GetMaintCals(baseline), baseline.CarbIntakePct, baseline.Sodium, _baselineLogic.GetActivityParam(baseline), date);
        }

        public DailyParams GenerateDailyParams(Intervention intervention, Baseline baseline, DateTime date)
        {
            return new DailyParams(intervention.Calories, intervention.Carbinpercent, intervention.Sodium, _baselineLogic.GetActivityParam(baseline), date);
        }

        public double GetCarbIntake(DailyParams dailyParams)
        {
            return dailyParams.Carbpercent / 100.0 * dailyParams.Calories;
        }

        public DailyParams[] GetParamtray(Baseline baseline, Intervention intervention1, Intervention intervention2, int simlength)
        {
            DailyParams[] paramtraj = new DailyParams[simlength];
            bool noeffect1 = intervention1.Day > simlength;
            bool noeffect2 = intervention2.Day > simlength;
            bool noeffect = noeffect1 && noeffect2;

            paramtraj[0] = GenerateDailyParams(baseline, baseline.Date);

            if (noeffect)
            {
                for (int i = 1; i < simlength; i++)
                {
                    paramtraj[i] = GenerateDailyParams(baseline, new DateTime(baseline.Date.Ticks).AddDays(i));
                }
            }
            else
            {
                Intervention firstIntervention = intervention1.Day < intervention2.Day ? intervention1 : intervention2;
                Intervention secondIntervention = intervention1.Day < intervention2.Day ? intervention2 : intervention1;

                for (int i = 1; i < firstIntervention.Day; i++)
                {
                    paramtraj[i] = GenerateDailyParams(baseline, new DateTime(baseline.Date.Ticks).AddDays(i));
                }

                int endfirst = Math.Min(secondIntervention.Day, simlength);

                for (int i = firstIntervention.Day; i < endfirst; i++)
                {
                    paramtraj[i] = GenerateDailyParams(firstIntervention, baseline, new DateTime(baseline.Date.Ticks).AddDays(i));
                }

                if (simlength > secondIntervention.Day)
                {
                    for (int i = secondIntervention.Day; i < simlength; i++)
                    {
                        paramtraj[i] = GenerateDailyParams(secondIntervention, baseline, new DateTime(baseline.Date.Ticks).AddDays(i));
                    }
                }
            }
            return paramtraj;
        }
    }
}
