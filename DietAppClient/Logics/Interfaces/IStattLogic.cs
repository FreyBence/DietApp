using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IStattLogic
    {
        (Dictionary<string, double>, ChartDataSet[]) GetDietDatas(double goalWeight, int goalTime, int simlength = 365);
    }
}