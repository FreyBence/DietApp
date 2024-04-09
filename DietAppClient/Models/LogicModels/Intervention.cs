namespace DietAppClient.Models
{
    public class Intervention
    {
        public int Day { get; set; }
        public double Calories { get; set; }
        public double Carbinpercent { get; set; }
        public double Sodium { get; set; }

        public Intervention(int day, double calories, double carbinpercent)
        {
            Day = day;
            Calories = calories;
            Carbinpercent = carbinpercent;
        }

        public Intervention()
            : this(100, 2200, 50)
        { }
    }
}
