namespace DietAppClient.Models
{
    public class DailyParams
    {
        public double Calories { get; set; }
        public double Carbpercent { get; set; }
        public double Sodium { get; set; }
        public double Actparam { get; set; }
        public DateTime Date { get; set; }

        public DailyParams(double calories, double carbpercent, double sodium, double actparam, DateTime date)
        {
            Calories = calories;
            Carbpercent = carbpercent;
            Sodium = sodium;
            Actparam = actparam;
            Date = date;
        }
    }
}
