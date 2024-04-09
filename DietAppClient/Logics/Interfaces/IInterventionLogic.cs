using DietAppClient.Models;

namespace DietAppClient.Logics
{
    public interface IInterventionLogic
    {
        Intervention CheckGoal(Baseline baseline, double goalWeight, int goalTime);
        void SetProportionalSodium(Intervention intervention, Baseline baseline);
    }
}