namespace DietAppClient.Models
{
    public class BodyModel
    {
        public double Fat { get; set; }
        public double Lean { get; set; }
        public double Glycogen { get; set; }
        public double Decw { get; set; }
        public double Therm { get; set; }

        public BodyModel(double fat, double lean, double glycogen, double decw, double therm)
        {
            Fat = fat;
            Lean = lean;
            Glycogen = glycogen;
            Decw = decw;
            Therm = therm;
        }
    }
}
