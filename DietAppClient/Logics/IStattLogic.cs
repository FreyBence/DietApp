using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IStattLogic
    {
        double[] GenerateGoalInterventions(double goalWeight, int goalTime);
        double GetAverageDailyCalorie();
        double[] GetBmis(double goalWeight);
        ChartDataSet[] GetChartDatas(int simlength);
        double GetPreMaintCols();
    }
}