using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IDailyParamsLogic
    {
        DailyParams GenerateDailyParams(Baseline baseline, DateTime date);
        DailyParams GenerateDailyParams(Intervention intervention, Baseline baseline, DateTime date);
        double GetCarbIntake(DailyParams dailyParams);
        DailyParams[] GetParamtray(Baseline baseline, Intervention intervention1, Intervention intervention2, int simlength);
    }
}