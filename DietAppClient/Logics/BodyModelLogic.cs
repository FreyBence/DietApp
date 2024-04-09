using DietAppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Logics
{
    public class BodyModelLogic : IBodyModelLogic
    {
        static readonly int[] RK4wt = [1, 2, 2, 1];
        IBaselineLogic _baselineLogic;
        IDailyParamsLogic _dailyParamsLogic;

        public BodyModelLogic(IBaselineLogic baselineLogic, IDailyParamsLogic dailyParamsLogic)
        {
            _baselineLogic = baselineLogic;
            _dailyParamsLogic = dailyParamsLogic;
        }

        public BodyModel GenerateBodyModel(Baseline baseline)
        {
            return new BodyModel(_baselineLogic.GetFatWeight(baseline), _baselineLogic.GetLeanWeight(baseline),
                baseline.Glycogen, baseline.DECW, _baselineLogic.GetTherm(baseline));
        }

        public BodyModel GenerateBodyModel(Baseline baseline, DailyParams dailyParams, int simlength)
        {
            BodyModel traj = GenerateBodyModel(baseline);

            for (int i = 0; i < simlength; i++)
            {
                traj = GetBodytraj(traj, baseline, dailyParams);
            }

            return traj;
        }

        public BodyModel GenerateBodyModel(Baseline baseline, Intervention intervention, int simlength)
        {
            return GenerateBodyModel(baseline, _dailyParamsLogic.GenerateDailyParams(intervention, baseline, baseline.Date), simlength);
        }

        public double GetWeight(BodyModel bodyModel, Baseline baseline)
        {
            return bodyModel.Fat + bodyModel.Lean + _baselineLogic.GetGlycogenH2O(bodyModel.Glycogen, baseline) + bodyModel.Decw;
        }

        public double GetFatPercent(BodyModel bodyModel, Baseline baseline)
        {
            return bodyModel.Fat / GetWeight(bodyModel, baseline) * 100.0;
        }

        public BodyChange Dt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return new BodyChange()
            {
                Df = Dfdt(bodyModel, baseline, dailyParams),
                Dl = Dldt(bodyModel, baseline, dailyParams),
                Dg = Dgdt(bodyModel, baseline, dailyParams),
                DDecw = DDecwdt(bodyModel, baseline, dailyParams),
                Dtherm = Dthermdt(bodyModel, baseline, dailyParams)
            };
        }

        public BodyModel GetBodytraj(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            BodyChange dt1 = Dt(bodyModel, baseline, dailyParams);
            BodyModel b2 = Addchange(bodyModel, dt1, 0.5);
            BodyChange dt2 = Dt(b2, baseline, dailyParams);
            BodyModel b3 = Addchange(bodyModel, dt2, 0.5);
            BodyChange dt3 = Dt(b3, baseline, dailyParams);
            BodyModel b4 = Addchange(bodyModel, dt3, 1.0);
            BodyChange dt4 = Dt(b4, baseline, dailyParams);
            BodyChange finaldt = Avgdt_weighted(RK4wt, [dt1, dt2, dt3, dt4]);
            BodyModel finalstate = Addchange(bodyModel, finaldt, 1.0);
            return finalstate;
        }

        public double GetTEE(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            double p = Getp(bodyModel);
            double calin = dailyParams.Calories;
            double carbflux = Carbflux(bodyModel, baseline, dailyParams);
            double expend = GetExpend(bodyModel, baseline, dailyParams);
            return (expend + (calin - carbflux) * ((1.0 - p) * 180.0 / 9440.0 + p * 230.0 / 1807.0)) / (1.0 + p * 230.0 / 1807.0 + (1.0 - p) * 180.0 / 9440.0);
        }

        public double GetExpend(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            double TEF = 0.1 * dailyParams.Calories;
            double weight = _baselineLogic.GetNewWeight(bodyModel.Fat, bodyModel.Lean, bodyModel.Glycogen, bodyModel.Decw, baseline);
            return _baselineLogic.GetK(baseline) + 22.0 * bodyModel.Lean + 3.2 * bodyModel.Fat + dailyParams.Actparam * weight + bodyModel.Therm + TEF;
        }

        public double Getp(BodyModel bodyModel)
        {
            return 1.990762711864407 / (1.990762711864407 + bodyModel.Fat);
        }

        public double Carbflux(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            double k_carb = _baselineLogic.GetCarbsIn(baseline) / Math.Pow(baseline.Glycogen, 2.0);
            return _dailyParamsLogic.GetCarbIntake(dailyParams) - k_carb * Math.Pow(bodyModel.Glycogen, 2.0);
        }

        public double Na_imbal(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return dailyParams.Sodium - baseline.Sodium - 3000.0 * bodyModel.Decw - 4000.0 * (1.0 - _dailyParamsLogic.GetCarbIntake(dailyParams) / _baselineLogic.GetCarbsIn(baseline));
        }

        public double Dfdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return (1.0 - Getp(bodyModel)) * (dailyParams.Calories - GetTEE(bodyModel, baseline, dailyParams) - Carbflux(bodyModel, baseline, dailyParams)) / 9440.0; ;
        }

        public double Dldt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return Getp(bodyModel) * (dailyParams.Calories - GetTEE(bodyModel, baseline, dailyParams) - Carbflux(bodyModel, baseline, dailyParams)) / 1807.0; ;
        }

        public double Dgdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return Carbflux(bodyModel, baseline, dailyParams) / 4180.0;
        }

        public double DDecwdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return Na_imbal(bodyModel, baseline, dailyParams) / 3220.0;
        }

        public double Dthermdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {
            return (0.14 * dailyParams.Calories - bodyModel.Therm) / 14.0;
        }

        public BodyModel Addchange(BodyModel bodyModel, BodyChange bchange, double tstep)
        {
            return new BodyModel(
                bodyModel.Fat + tstep * bchange.Df,
                bodyModel.Lean + tstep * bchange.Dl,
                bodyModel.Glycogen + tstep * bchange.Dg,
                bodyModel.Decw + tstep * bchange.DDecw,
                bodyModel.Therm + tstep * bchange.Dtherm
            );
        }

        public double Cals4balance(BodyModel bodyModel, Baseline baseline, double act)
        {
            double weight = GetWeight(bodyModel, baseline),
                Expend_no_food = _baselineLogic.GetK(baseline) + 22.0 * bodyModel.Lean + 3.2 * bodyModel.Fat + act * weight,
                p = Getp(bodyModel),
                p_d = 1.0 + p * 230.0 / 1807.0 + (1.0 - p) * 180.0 / 9440.0,
                p_n = (1.0 - p) * 180.0 / 9440.0 + p * 230.0 / 1807.0,
                maint_nocflux = Expend_no_food / (p_d - p_n - 0.24);

            return maint_nocflux;
        }

        public BodyChange Avgdt_weighted(int[] wt, BodyChange[] bchanges)
        {
            double sumf = 0.0;
            double suml = 0.0;
            double sumg = 0.0;
            double sumdecw = 0.0;
            double sumtherm = 0.0;
            double wti = 0;
            double wtsum = 0;

            for (int i = 0; i < bchanges.Length; i++)
            {
                wti = wt[i];
                wti = (wti < 0) ? 1 : wti;
                wtsum += wti;
                sumf += wti * bchanges[i].Df;
                suml += wti * bchanges[i].Dl;
                sumg += wti * bchanges[i].Dg;
                sumdecw += wti * bchanges[i].DDecw;
                sumtherm += wti * bchanges[i].Dtherm;
            }

            double nf = sumf / wtsum;
            double nl = suml / wtsum;
            double ng = sumg / wtsum;
            double ndecw = sumdecw / wtsum;
            double ntherm = sumtherm / wtsum;

            return new BodyChange()
            {
                Df = nf,
                Dl = nl,
                Dg = ng,
                DDecw = ndecw,
                Dtherm = ntherm
            };
        }
    }
}
