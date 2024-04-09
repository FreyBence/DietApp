using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IBodyModelLogic
    {
        BodyModel Addchange(BodyModel bodyModel, BodyChange bchange, double tstep);
        BodyChange Avgdt_weighted(int[] wt, BodyChange[] bchanges);
        double Cals4balance(BodyModel bodyModel, Baseline baseline, double act);
        double Carbflux(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double DDecwdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double Dfdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double Dgdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double Dldt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        BodyChange Dt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double Dthermdt(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        BodyModel GenerateBodyModel(Baseline baseline);
        BodyModel GenerateBodyModel(Baseline baseline, DailyParams dailyParams, int simlength);
        BodyModel GenerateBodyModel(Baseline baseline, Intervention intervention, int simlength);
        BodyModel GetBodytraj(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double GetExpend(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double GetFatPercent(BodyModel bodyModel, Baseline baseline);
        double Getp(BodyModel bodyModel);
        double GetTEE(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
        double GetWeight(BodyModel bodyModel, Baseline baseline);
        double Na_imbal(BodyModel bodyModel, Baseline baseline, DailyParams dailyParams);
    }
}