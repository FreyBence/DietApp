using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Models
{
    public class BodyModel
    {
        static readonly int[] RK4wt = new int[] { 1, 2, 2, 1 };
        double fat;
        double lean;
        double glycogen;
        double decw;
        double therm;

        public double Fat { get => fat; set => fat = value; }
        public double Lean { get => lean; set => lean = value; }
        public double Glycogen { get => glycogen; set => glycogen = value; }
        public double Decw { get => decw; set => decw = value; }
        public double Therm { get => therm; set => therm = value; }

        public BodyModel(double fat, double lean, double glycogen, double decw, double therm) {
            Fat = fat;
            Lean = lean;
            Glycogen = glycogen;
            Decw = decw;
            Therm = therm;
        }

        public BodyModel(Baseline baseline)
            : this(baseline.GetFatWeight(), baseline.GetLeanWeight(), baseline.Glycogen, baseline.DECW, baseline.GetTherm())
        {}

        public BodyModel(Baseline baseline, DailyParams dailyParams, int simlength)
        {
            BodyModel traj = new BodyModel(baseline);

            for (int i = 0; i < simlength; i++)
            {
                traj = GetBodytraj(traj, baseline, dailyParams);
            }

            Fat = traj.Fat;
            Lean = traj.Lean;
            Glycogen = traj.Glycogen;
            Decw = traj.Decw;
            Therm = traj.Therm;
        }

        public BodyModel(Baseline baseline, Intervention intervention, int simlength)
            : this(baseline, new DailyParams(intervention, baseline), simlength)
        {}

        public double GetWeight(Baseline baseline)
        {
            return fat + lean + baseline.GetGlycogenH2O(glycogen) + decw;
        }

        public double GetFatPercent(Baseline baseline)
        {
            return fat / GetWeight(baseline) * 100.0;
        }

        public BodyChange Dt(Baseline baseline, DailyParams dailyParams)
        {
            return new BodyChange()
            {
                Df = Dfdt(baseline, dailyParams),
                Dl = Dldt(baseline, dailyParams),
                Dg = Dgdt(baseline, dailyParams),
                DDecw = DDecwdt(baseline, dailyParams),
                Dtherm = Dthermdt(baseline, dailyParams)
            };
        }

        public static BodyModel GetBodytraj(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams)
        {

            BodyChange dt1 = bodyModel.Dt(baseline, dailyParams);
            BodyModel b2 = bodyModel.Addchange(dt1, 0.5);
            BodyChange dt2 = b2.Dt(baseline, dailyParams);
            BodyModel b3 = bodyModel.Addchange(dt2, 0.5);
            BodyChange dt3 = b3.Dt(baseline, dailyParams);
            BodyModel b4 = bodyModel.Addchange(dt3, 1.0);
            BodyChange dt4 = b4.Dt(baseline, dailyParams);
            BodyChange finaldt = bodyModel.Avgdt_weighted(RK4wt, new BodyChange[]{ dt1, dt2, dt3, dt4 });
            BodyModel finalstate = bodyModel.Addchange(finaldt, 1.0);
            return finalstate;
        }

        public double GetTEE(Baseline baseline, DailyParams dailyParams)
        {
            double p = Getp();
            double calin = dailyParams.Calories;
            double carbflux = Carbflux(baseline, dailyParams);
            double expend = GetExpend(baseline, dailyParams);
            return (expend + (calin - carbflux) * ((1.0 - p) * 180.0 / 9440.0 + p * 230.0 / 1807.0)) / (1.0 + p * 230.0 / 1807.0 + (1.0 - p) * 180.0 / 9440.0);

        }

        public double GetExpend(Baseline baseline, DailyParams dailyParams)
        {
            double TEF = 0.1 * dailyParams.Calories;
            double weight = baseline.GetNewWeight(fat , lean, glycogen, decw);
            return baseline.GetK() + 22.0 * lean + 3.2 * fat + dailyParams.Actparam * weight + therm + TEF;
        }

        public double Getp()
        {
            return 1.990762711864407 / (1.990762711864407 + fat);
        }

        public double Carbflux(Baseline baseline, DailyParams dailyParams)
        {
            double k_carb = baseline.GetCarbsIn() / Math.Pow(baseline.Glycogen, 2.0);
            return dailyParams.GetCarbIntake() - k_carb * Math.Pow(glycogen, 2.0);
        }

        public double Na_imbal(Baseline baseline, DailyParams dailyParams)
        {
            return dailyParams.Sodium - baseline.Sodium - 3000.0 * decw - 4000.0 * (1.0 - dailyParams.GetCarbIntake() / baseline.GetCarbsIn());
        }

        public double Dfdt(Baseline baseline, DailyParams dailyParams)
        {
            return (1.0 - Getp()) * (dailyParams.Calories - GetTEE(baseline, dailyParams) - Carbflux(baseline, dailyParams)) / 9440.0; ;
        }

        public double Dldt(Baseline baseline, DailyParams dailyParams)
        {
            return Getp() * (dailyParams.Calories - GetTEE(baseline, dailyParams) - Carbflux(baseline, dailyParams)) / 1807.0; ;
        }

        public double Dgdt(Baseline baseline, DailyParams dailyParams)
        {
            return Carbflux(baseline, dailyParams) / 4180.0;
        }

        public double DDecwdt(Baseline baseline, DailyParams dailyParams)
        {
            return Na_imbal(baseline, dailyParams) / 3220.0;
        }

        public double Dthermdt(Baseline baseline, DailyParams dailyParams)
        {
            return (0.14 * dailyParams.Calories - therm) / 14.0;
        }

        public BodyModel Addchange(BodyChange bchange, double tstep)
        {
            return new BodyModel(
                fat + tstep * bchange.Df,
                lean + tstep * bchange.Dl,
                glycogen + tstep * bchange.Dg,
                decw + tstep * bchange.DDecw,
                therm + tstep * bchange.Dtherm
            );
        }
        public double Cals4balance(Baseline baseline, double act)
        {
            double weight = GetWeight(baseline),
                Expend_no_food = baseline.GetK() + 22.0 * lean + 3.2 * fat + act * weight,
                p = Getp(),
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
