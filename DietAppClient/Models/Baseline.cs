using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Models
{
    public class Baseline
    {
        bool isMale;
        int age;
        double height;
        double weight;
        double bfp;
        double rmr;
        double pal;
        double carbIntakePct;
        double sodium;
        double dECW;
        double glycogen;

        public bool IsMale { get => isMale; set => isMale = value; }
        public int Age { get => age; set => age = value; }
        public double Height { get => height; set => height = value; }
        public double Weight { get => weight; set => weight = value; }
        public double Bfp { get => bfp; set => bfp = value; }
        public double Rmr { get => rmr; set => rmr = value; }
        public double Pal { get => pal; set => pal = value; }
        public double CarbIntakePct { get => carbIntakePct; set => carbIntakePct = value; }
        public double Sodium { get => sodium; set => sodium = value; }
        public double DECW { get => dECW; set => dECW = value; }
        public double Glycogen { get => glycogen; set => glycogen = value; }

        public Baseline(bool isMale, int age, double height, double weight, double pal)
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
        }

        public double[] GetHealthyWeightRange()
        {
            double low = Math.Round(18.5 * Math.Pow((height / 100), 2));
            double high = Math.Round(25 * Math.Pow((height / 100), 2));
            return new double[2] { low, high };
        }

        public double GetBFP()
        {
            double result;
            if (isMale)
                result = (0.14 * age + 37.310000000000002 * Math.Log(GetBMI()) - 103.94);
            else
                result = (0.14 * age + 39.960000000000001 * Math.Log(GetBMI()) - 102.01000000000001);

            if (result >= 60 || result < 0)
                return bfp;

            return result;
        }

        public double GetBMI()
        {
            return weight / Math.Pow(height / 100.0, 2.0);
        }

        public double GetFatWeight()
        {
            return weight * GetBFP() / 100.0;
        }

        public double GetLeanWeight()
        {
            return weight - GetFatWeight();
        }

        public double GetTherm()
        {
            return 0.14 * GetTEE();
        }

        public double GetTEE()
        {
            return pal * GetRMR();
        }

        public double GetRMR()
        {
            if (isMale)
                return (9.99 * weight + 625.0 * height / 100.0 - 4.92 * age + 5.0);
            else
                return (9.99 * weight + 625.0 * height / 100.0 - 4.92 * age - 161.0);
        }

        public double GetMaintCals()
        {
            return pal * GetRMR();
        }

        public double GetActivityParam()
        {
            return (0.9 * GetRMR() * pal - GetRMR()) / weight;
        }

        public double GetCarbsIn()
        {
            return carbIntakePct / 100.0 * GetMaintCals();
        }

        public double GetK()
        {
            return 0.76 * GetMaintCals() - 22.0 * GetLeanWeight() - 3.2 * GetFatWeight() - GetActivityParam() * weight;
        }

        public double GetGlycogenH2O(double newGlycogen)
        {
            return 3.7 * (newGlycogen - glycogen);
        }

        public double GetNewWeight(double fat, double lean, double glycogen, double decw)
        {
            return fat + lean + GetGlycogenH2O(glycogen) + decw;
        }
    }
}
