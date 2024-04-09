using CommunityToolkit.Mvvm.Input;
using DietAppClient.Data;
using DietAppClient.Models;
using DietAppClient.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    public class RecordsViewModel : INotifyPropertyChanged
    {
        IRecordRepository _repo;
        public ObservableCollection<Record> Records { get; set; }

        private Record selectedRecord;
        public Record SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                selectedRecord = value?.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedRecord"));
                (OnUpdateClicked as RelayCommand).NotifyCanExecuteChanged();
                (OnDeleteClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OnAddClicked { get; set; }
        public ICommand OnUpdateClicked { get; set; }
        public ICommand OnDeleteClicked { get; set; }

        public RecordsViewModel()
        { }

        public RecordsViewModel(IRecordRepository repo)
        {
            _repo = repo;
            Records = new ObservableCollection<Record>(repo.ReadAll());
            OnAddClicked = new Command(Add);
            OnUpdateClicked = new RelayCommand(Update, () => SelectedRecord != null);
            OnDeleteClicked = new RelayCommand(Delete, () => SelectedRecord != null);
        }
        public void Refresh()
        {
            Records = new ObservableCollection<Record>(_repo.ReadAll());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Records"));
            SelectedRecord = Records.FirstOrDefault(t => t.Id == SelectedRecord?.Id, null);
        }


        public async void Add()
        {
            var navigationParams = new Dictionary<string, object> {
                { "Record", null }
            };
            await Shell.Current.GoToAsync(nameof(MenageRecordPage), navigationParams);
        }

        private async void Update()
        {
            var navigationParams = new Dictionary<string, object> {
                { "Record", SelectedRecord }
            };
            await Shell.Current.GoToAsync(nameof(MenageRecordPage), navigationParams);
        }

        private async void Delete()
        {
            _repo.Delete(SelectedRecord.Id);
            Refresh();
        }
    }
}
