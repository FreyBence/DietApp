using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IBaselineLogic
    {
        double GetActivityParam(Baseline baseline);
        double GetBFP(Baseline baseline);
        double GetBMI(Baseline baseline);
        double GetCarbsIn(Baseline baseline);
        double GetFatWeight(Baseline baseline);
        double GetGlycogenH2O(double newGlycogen, Baseline baseline);
        double[] GetHealthyWeightRange(Baseline baseline);
        double GetK(Baseline baseline);
        double GetLeanWeight(Baseline baseline);
        double GetMaintCals(Baseline baseline);
        double GetNewWeight(double fat, double lean, double glycogen, double decw, Baseline baseline);
        double GetRMR(Baseline baseline);
        double GetTEE(Baseline baseline);
        double GetTherm(Baseline baseline);
    }
}