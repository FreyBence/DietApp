using CommunityToolkit.Mvvm.Input;
using DietAppClient.Logics;
using DietAppClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageChartsLib;

namespace DietAppClient.ViewModels
{
    public class StattViewModel : INotifyPropertyChanged
    {
        IStattLogic _logic;

        private bool isGenerated;
        public bool IsGenerated
        {
            get { return isGenerated; }
            set
            {
                isGenerated = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsGenerated"));
                (OnDataClicked as RelayCommand).NotifyCanExecuteChanged();
                (OnWeightClicked as RelayCommand).NotifyCanExecuteChanged();
                (OnFatClicked as RelayCommand).NotifyCanExecuteChanged();
                (OnExpClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        private int goalWeight;
        public int GoalWeight
        {
            get { return goalWeight; }
            set
            {
                goalWeight = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GoalWeight"));
            }
        }

        private int reachDays;
        public int ReachDays
        {
            get { return reachDays; }
            set
            {
                reachDays = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReachDays"));
            }
        }

        public ICommand OnGenerateClicked { get; set; }
        public ICommand OnDataClicked { get; set; }
        public ICommand OnWeightClicked { get; set; }
        public ICommand OnFatClicked { get; set; }
        public ICommand OnExpClicked { get; set; }

        private double avgDailyCols;
        private double preMaintCols;
        private double processCols;
        private double postMaintCols;
        private double preFatP;
        private double postFatP;
        private double preBmi;
        private double postBmi;
        private double preWeight;
        private double postWeight;

        private ChartDataSet[] chartDatas;

        public StattViewModel(IStattLogic logic)
        {
            _logic = logic;
            isGenerated = false;
            OnGenerateClicked = new Command(Generate);
            OnDataClicked = new RelayCommand(() => { }, () => IsGenerated);
            OnWeightClicked = new RelayCommand(() => { }, () => IsGenerated);
            OnFatClicked = new RelayCommand(() => { }, () => IsGenerated);
            OnExpClicked = new RelayCommand(() => { }, () => IsGenerated);
            
        }

        private void Generate()
        {
            try
            {
                double[] bmis = _logic.GetBmis(goalWeight);
                preBmi = Math.Round(bmis[0], 1);
                postBmi = Math.Round(bmis[1], 1);

                avgDailyCols = Math.Round(_logic.GetAverageDailyCalorie(), 1);
                preMaintCols = Math.Round(_logic.GetPreMaintCols(), 1);

                double[] cols = _logic.GenerateGoalInterventions(goalWeight, reachDays);
                processCols = Math.Round(cols[0], 1);
                postMaintCols = Math.Round(cols[1], 1);

                chartDatas = _logic.GetChartDatas(365);
                preWeight = Math.Round(chartDatas[0].Weight, 1);
                postWeight = Math.Round(chartDatas[364].Weight, 1);
                preFatP = Math.Round(chartDatas[0].FatPercent, 1);
                postFatP = Math.Round(chartDatas[364].FatPercent, 1);

                IsGenerated = true;
            }
            catch (Exception e) 
            { 

            }
        }

        public List<Label> SwitchToData()
        {
            List<Label> result = new List<Label>();

            Label label = new Label();
            label.HeightRequest = 20;
            label.VerticalTextAlignment = TextAlignment.Start;
            label.VerticalOptions = LayoutOptions.Fill;
            label.Margin = 10;

            label.Text = $"Average daily calorie: {avgDailyCols} (Cal)";
            result.Add(GetLabelCopy(label));
            label.Text = $"Maintain calorie: {preMaintCols} (Cal/day)";
            result.Add(GetLabelCopy(label));
            label.Text = $"Calorie to reach goal: {processCols} (Cal/day)";
            result.Add(GetLabelCopy(label));
            label.Text = $"Calorie to maintain goal: {postMaintCols} (Cal/day)";
            result.Add(GetLabelCopy(label));
            label.Text = $"Current wieght: {preWeight} kg";
            result.Add(GetLabelCopy(label));
            label.Text = $"Final wieght: {postWeight} kg";
            result.Add(GetLabelCopy(label));
            label.Text = $"Current fat: {preFatP}%";
            result.Add(GetLabelCopy(label));
            label.Text = $"Final fat: {postFatP}%";
            result.Add(GetLabelCopy(label));
            label.Text = $"Current BMI: {preBmi}";
            result.Add(GetLabelCopy(label));
            label.Text = $"Final BMI: {postBmi}";
            result.Add(label);

            return result;
        }

        public ImageSource SwitchToWeight()
        { 
            string chd = "a:|" +Math.Round(chartDatas[0].Weight, 1).ToString();
            string chxl = "1:|1";
            for (int i = 1; i < chartDatas.Length; i++)
            {
                chd += "," + Math.Round(chartDatas[i].Weight, 1).ToString();

                if ((i + 1) % 50 == 0)
                    chxl += "|" + (i + 1);
            }

            ImageCharts chart = new ImageCharts().cht("lc").chxt("y,x")
                .chco("512BD4").chs("999x550").chd(chd).chl("Goal day".PadLeft(reachDays, '|'))
                .chds("a").chxl(chxl).chxr($"0,{preWeight - 5},{goalWeight + 5}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }

        public ImageSource SwitchToFat()
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
                .chco("512BD4").chs("999x550").chd(chd).chl("Goal day".PadLeft(reachDays, '|'))
                .chds("a").chxl(chxl).chxr($"0,{Math.Round(preFatP) - 5},{Math.Round(postFatP) + 5}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }

        public ImageSource SwitchToExp()
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
                .chco("3072F3,ff0000,ff0000").chs("999x550").chd(chd2 + "|" + chd1).chl("Goal day".PadLeft(reachDays, '|'))
                .chdl("Intake|Expenditure").chdlp("b")
                .chds("a").chxl(chxl).chxr($"0,{Math.Round(chartDatas[0].Intake)},{processCols}");
            MemoryStream memory = new MemoryStream(chart.toBuffer());
            return ImageSource.FromStream(() => memory);
        }

        private Label GetLabelCopy(Label label)
        { 
            Label copy = new Label();
            copy.HeightRequest = label.HeightRequest;
            copy.VerticalTextAlignment = label.VerticalTextAlignment;
            copy.VerticalOptions = label.VerticalOptions;
            copy.Margin = label.Margin;
            copy.Text = label.Text ;

            return copy;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
