using DietAppClient.Models;
using ImageChartsLib;

namespace DietAppClient.Helpers
{
    public class ChartImageGenerator
    {
        public static ImageSource GenerateWeightChart(ChartDataSet[] chartDatas, double startRange, double endRange, int labelOffset)
        {
            string chd = "a:|" + Math.Round(chartDatas[0].Weight, 1).ToString();
            string chxl = "1:|1";
            for (int i = 1; i < chartDatas.Length; i++)
            {
                chd += "," + Math.Round(chartDatas[i].Weight, 1).ToString();

                if ((i + 1) % 50 == 0)
                    chxl += "|" + (i + 1);
            }

            ImageCharts chart = new ImageCharts().cht("lc").chxt("y,x")
                .chco("512BD4").chs("999x550").chd(chd).chl("Goal day".PadLeft(labelOffset, '|'))
                .chds("a").chxl(chxl).chxr($"0,{Math.Round(startRange) - 5},{Math.Round(endRange) + 5}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }

        public static ImageSource GenerateFatChart(ChartDataSet[] chartDatas, double startRange, double endRange, int labelOffset)
        {
            string chd = "a:|" + Math.Round(chartDatas[0].FatPercent, 1).ToString();
            string chxl = "1:|1";
            for (int i = 1; i < chartDatas.Length; i++)
            {
                chd += "," + Math.Round(chartDatas[i].FatPercent, 1).ToString();

                if ((i + 1) % 50 == 0)
                    chxl += "|" + (i + 1);
            }

            ImageCharts chart = new ImageCharts().cht("lc").chxt("y,x")
                .chco("512BD4").chs("999x550").chd(chd).chl("Goal day".PadLeft(labelOffset, '|'))
                .chds("a").chxl(chxl).chxr($"0,{Math.Round(startRange) - 5},{Math.Round(endRange) + 5}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }

        public static ImageSource GenerateExpChart(ChartDataSet[] chartDatas, double endRange, int labelOffset)
        {
            string chd2 = "a:|" + Math.Round(chartDatas[0].Expenditure, 1).ToString();
            string chd1 = "|" + Math.Round(chartDatas[0].Intake, 1).ToString();
            string chxl = "1:|1";
            for (int i = 1; i < chartDatas.Length; i++)
            {
                chd1 += "," + Math.Round(chartDatas[i].Intake, 1).ToString();
                chd2 += "," + Math.Round(chartDatas[i].Expenditure, 1).ToString();

                if ((i + 1) % 50 == 0)
                    chxl += "|" + (i + 1);
            }

            ImageCharts chart = new ImageCharts().cht("lc").chxt("y,x")
                .chco("3072F3,ff0000,ff0000").chs("999x550").chd(chd2 + "|" + chd1).chl("Goal day".PadLeft(labelOffset, '|'))
                .chdl("Intake|Expenditure").chdlp("b")
                .chds("a").chxl(chxl).chxr($"0,{Math.Round(chartDatas[0].Intake)},{Math.Round(endRange)}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }
    }
}
