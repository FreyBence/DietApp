namespace DietAppClient.Models
{
    public class Baseline
    {
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double Bfp { get; set; }
        public double Rmr { get; set; }
        public double Pal { get; set; }
        public double CarbIntakePct { get; set; }
        public double Sodium { get; set; }
        public double DECW { get; set; }
        public double Glycogen { get; set; }
        public DateTime Date { get; set; }

        public Baseline(bool isMale, int age, double height, double weight, double pal, DateTime date)
        {
            IsMale = isMale;
            Age = age;
            Height = height;
            Weight = weight;
            Pal = pal;
            Bfp = 18;
            Rmr = 1708;
            CarbIntakePct = 50;
            Sodium = 4000;
            DECW = 0;
            Glycogen = 0.5;
            Date = date;
        }
    }
}
