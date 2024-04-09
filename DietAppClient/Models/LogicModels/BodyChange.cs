namespace DietAppClient.Models
{
    public class BodyChange
    {
        public double Df { get; set; }
        public double Dl { get; set; }
        public double Dg { get; set; }
        public double DDecw { get; set; }
        public double Dtherm { get; set; }

        public BodyChange(double df, double dl, double dg, double dDecw, double dtherm)
        {
            Df = df;
            Dl = dl;
            Dg = dg;
            DDecw = dDecw;
            Dtherm = dtherm;
        }

        public BodyChange() 
        { }
    }
}
