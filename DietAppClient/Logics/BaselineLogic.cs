using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public class BaselineLogic : IBaselineLogic
    {
        public double[] GetHealthyWeightRange(Baseline baseline)
        {
            double low = Math.Round(18.5 * Math.Pow(baseline.Height / 100, 2));
            double high = Math.Round(25 * Math.Pow(baseline.Height / 100, 2));
            return [low, high];
        }

        public double GetBFP(Baseline baseline)
        {
            double result;
            if (baseline.IsMale)
                result = (0.14 * baseline.Age + 37.310000000000002 * Math.Log(GetBMI(baseline)) - 103.94);
            else
                result = (0.14 * baseline.Age + 39.960000000000001 * Math.Log(GetBMI(baseline)) - 102.01000000000001);

            if (result >= 60 || result < 0)
                return baseline.Bfp;

            return result;
        }

        public double GetBMI(Baseline baseline)
        {
            return baseline.Weight / Math.Pow(baseline.Height / 100.0, 2.0);
        }

        public double GetFatWeight(Baseline baseline)
        {
            return baseline.Weight * GetBFP(baseline) / 100.0;
        }

        public double GetLeanWeight(Baseline baseline)
        {
            return baseline.Weight - GetFatWeight(baseline);
        }

        public double GetTherm(Baseline baseline)
        {
            return 0.14 * GetTEE(baseline);
        }

        public double GetTEE(Baseline baseline)
        {
            return baseline.Pal * GetRMR(baseline);
        }

        public double GetRMR(Baseline baseline)
        {
            if (baseline.IsMale)
                return (9.99 * baseline.Weight + 625.0 * baseline.Height / 100.0 - 4.92 * baseline.Age + 5.0);
            else
                return (9.99 * baseline.Weight + 625.0 * baseline.Height / 100.0 - 4.92 * baseline.Age - 161.0);
        }

        public double GetMaintCals(Baseline baseline)
        {
            return baseline.Pal * GetRMR(baseline);
        }

        public double GetActivityParam(Baseline baseline)
        {
            return 0.9 * GetRMR(baseline) * (baseline.Pal - 1) / baseline.Weight;
        }

        public double GetCarbsIn(Baseline baseline)
        {
            return baseline.CarbIntakePct / 100.0 * GetMaintCals(baseline);
        }

        public double GetK(Baseline baseline)
        {
            return 0.76 * GetMaintCals(baseline) - 22.0 * GetLeanWeight(baseline) - 3.2 * GetFatWeight(baseline) - GetActivityParam(baseline) * baseline.Weight;
        }

        public double GetGlycogenH2O(double newGlycogen, Baseline baseline)
        {
            return 3.7 * (newGlycogen - baseline.Glycogen);
        }

        public double GetNewWeight(double fat, double lean, double glycogen, double decw, Baseline baseline)
        {
            return fat + lean + GetGlycogenH2O(glycogen, baseline) + decw;
        }
    }
}
