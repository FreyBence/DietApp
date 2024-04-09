using DietAppClient.Data;
using DietAppClient.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    [QueryProperty(nameof(Record), "Record")]
    public class MenageRecordViewModel : INotifyPropertyChanged
    {
        Random rn = new Random();
        IRecordRepository _repo;

        private Record record;

        public Record Record
        {
            get { return record; }
            set
            {
                record = value?.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Record"));
            }
        }

        string buttonText;

        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ButtonText"));
            }
        }

        public string title { get; set; }

        public string Title
        {
            get { return title; }
            set
            {
                title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
            }
        }
        public ICommand onButtonClicked;

        public ICommand OnButtonClicked
        {
            get { return onButtonClicked; }
            set
            {
                onButtonClicked = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnButtonClicked"));
            }
        }
        public ICommand OnAddTestClicked { get; set; }
        public ICommand OnClearTestClicked { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MenageRecordViewModel(IRecordRepository repo)
        {
            _repo = repo;
            OnClearTestClicked = new Command(ClearTest);
            OnAddTestClicked = new Command(AddTest);

        }

        public void Refresh()
        {
            if (Record == null)
            {
                Record = new Record();
                Title = "Add Record";
                ButtonText = "Add";
                OnButtonClicked = new Command(OnAddClicked);
            }
            else
            {
                Title = "Edit Record";
                ButtonText = "Edit";
                OnButtonClicked = new Command(OnEditClicked);
            }
        }

        private async void OnAddClicked()
        {
            _repo.Create(Record);
            await Shell.Current.GoToAsync($"../");
        }

        private async void OnEditClicked()
        {
            _repo.Update(Record);
            await Shell.Current.GoToAsync($"../");
        }

        private async void ClearTest()
        {
            var items = _repo.ReadAll().ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                _repo.Delete(items[i].Id);
                await Shell.Current.GoToAsync($"../");
            }
        }

        private async void AddTest()
        {
            for(var i = 0; i < 100; i++) 
            {
                _repo.Create(new Record() { 
                    Calories = rn.Next(2800, 3500),
                    Date = DateTime.Now.AddDays(i + 1)
                });
            }
            await Shell.Current.GoToAsync($"../");
        }
    }
}
