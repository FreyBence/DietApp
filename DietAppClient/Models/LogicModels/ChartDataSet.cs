namespace DietAppClient.Models
{
    public class ChartDataSet
    {
        public double Weight { get; set; }
        public double FatPercent { get; set; }
        public double Intake { get; set; }
        public double Expenditure { get; set; }

        public ChartDataSet(double weight, double fatPercent, double intake, double expenditure)
        {
            Weight = weight;
            FatPercent = fatPercent;
            Intake = intake;
            Expenditure = expenditure;
        }

        public ChartDataSet()
        { }
    }
}
