using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using DietAppClient.Exceptions;
using DietAppClient.Helpers;
using DietAppClient.Logics;
using DietAppClient.Models;
using DietAppClient.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    public class StattViewModel : INotifyPropertyChanged
    {
        IStattLogic _logic;

        private double? goalWeight;
        public double? GoalWeight
        {
            get { return goalWeight; }
            set
            {
                goalWeight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GoalWeight"));
                (OnGenerateClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        private int? reachDays;
        public int? ReachDays
        {
            get { return reachDays; }
            set
            {
                reachDays = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReachDays"));
                (OnGenerateClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        private double? avgDailyCols;
        public double? AvgDailyCols
        {
            get { return avgDailyCols; }
            set
            {
                avgDailyCols = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvgDailyCols"));
                (OnDataClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        private double preMaintCols;
        public double PreMaintCols
        {
            get { return preMaintCols; }
            set
            {
                preMaintCols = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreMaintCols"));
            }
        }

        private double processCols;
        public double ProcessCols
        {
            get { return processCols; }
            set
            {
                processCols = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProcessCols"));
            }
        }

        private double postMaintCols;
        public double PostMaintCols
        {
            get { return postMaintCols; }
            set
            {
                postMaintCols = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostMaintCols"));
            }
        }

        private double preWeight;
        public double PreWeight
        {
            get { return preWeight; }
            set
            {
                preWeight = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreWeight"));
            }
        }

        private double postWeight;
        public double PostWeight
        {
            get { return postWeight; }
            set
            {
                postWeight = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostWeight"));
            }
        }

        private double preFatP;
        public double PreFatP
        {
            get { return preFatP; }
            set
            {
                preFatP = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreFatP"));
            }
        }

        private double postFatP;
        public double PostFatP
        {
            get { return postFatP; }
            set
            {
                postFatP = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostFatP"));
            }
        }

        private double preBmi;
        public double PreBmi
        {
            get { return preBmi; }
            set
            {
                preBmi = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreBmi"));
            }
        }

        private double postBmi;
        public double PostBmi
        {
            get { return postBmi; }
            set
            {
                postBmi = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostBmi"));
            }
        }

        private ImageSource weightChartImageSource;
        public ImageSource WeightChartImageSource
        {
            get { return weightChartImageSource; }
            set
            {
                weightChartImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WeightChartImageSource"));
                (OnWeightClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        private ImageSource fatChartImageSource;
        public ImageSource FatChartImageSource
        {
            get { return fatChartImageSource; }
            set
            {
                fatChartImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FatChartImageSource"));
                (OnFatClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        private ImageSource expChartImageSource;
        public ImageSource ExpChartImageSource
        {
            get { return expChartImageSource; }
            set
            {
                expChartImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExpChartImageSource"));
                (OnExpClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public ICommand OnGenerateClicked { get; set; }
        public ICommand OnDataClicked { get; set; }
        public ICommand OnWeightClicked { get; set; }
        public ICommand OnFatClicked { get; set; }
        public ICommand OnExpClicked { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public StattViewModel(IStattLogic logic)
        {
            _logic = logic;

            OnGenerateClicked = new RelayCommand(Generate, () =>
            {
                return goalWeight > 0 && reachDays > 0;
            });
            OnDataClicked = new RelayCommand(() => { }, () => AvgDailyCols != null);
            OnWeightClicked = new RelayCommand(() => { }, () => WeightChartImageSource != null);
            OnFatClicked = new RelayCommand(() => { }, () => FatChartImageSource != null);
            OnExpClicked = new RelayCommand(() => { }, () => ExpChartImageSource != null);
        }

        private void Generate()
        {
            try
            {
                (Dictionary<string, double> dietDatas, ChartDataSet[] chartDatas) = _logic.GetDietDatas((double)goalWeight, (int)reachDays);

                AvgDailyCols = dietDatas[nameof(AvgDailyCols)];
                PreMaintCols = dietDatas[nameof(PreMaintCols)];
                ProcessCols = dietDatas[nameof(ProcessCols)];
                PostMaintCols = dietDatas[nameof(PostMaintCols)];
                PreWeight = dietDatas[nameof(PreWeight)];
                PostWeight = dietDatas[nameof(PostWeight)];
                PreFatP = dietDatas[nameof(PreFatP)];
                PostFatP = dietDatas[nameof(PostFatP)];
                PreBmi = dietDatas[nameof(PreBmi)];
                PostBmi = dietDatas[nameof(PostBmi)];

                WeightChartImageSource = ChartImageGenerator.GenerateWeightChart(chartDatas, PreWeight, PostWeight, (int)ReachDays);
                FatChartImageSource = ChartImageGenerator.GenerateFatChart(chartDatas, preFatP, PostFatP, (int)ReachDays);
                ExpChartImageSource = ChartImageGenerator.GenerateExpChart(chartDatas, processCols, (int)ReachDays);
            }
            catch (HealthCheckException e)
            {
                var popup = new PopupView(e.Message.Replace(", ", ",\n"));
                App.Current.MainPage.ShowPopup(popup);
            }
        }
    }
}
