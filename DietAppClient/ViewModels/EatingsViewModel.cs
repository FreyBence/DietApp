using CommunityToolkit.Mvvm.Input;
using DietAppClient.Data;
using DietAppClient.Models;
using DietAppClient.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    public class EatingsViewModel : INotifyPropertyChanged
    {
        IEatingRepository _repo;
        public ObservableCollection<Eating> Eatings { get; set; }

        private Eating selectedEating;
        public Eating SelectedEating
        {
            get { return selectedEating; }
            set
            {
                selectedEating = value?.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedEating"));
                (OnUpdateClicked as RelayCommand).NotifyCanExecuteChanged();
                (OnDeleteClicked as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OnAddClicked { get; set; }
        public ICommand OnUpdateClicked { get; set; }
        public ICommand OnDeleteClicked { get; set; }

        public EatingsViewModel()
        { }

        public EatingsViewModel(IEatingRepository repo)
        {
            _repo = repo;
            Eatings = new ObservableCollection<Eating>(repo.ReadAll());
            OnAddClicked = new Command(Add);
            OnUpdateClicked = new RelayCommand(Update, () => SelectedEating != null);
            OnDeleteClicked = new RelayCommand(Delete, () => SelectedEating != null);
        }
        public void Refresh()
        {
            Eatings = new ObservableCollection<Eating>(_repo.ReadAll());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Eatings"));
            SelectedEating = Eatings.FirstOrDefault(t => t.Id == SelectedEating?.Id, null);
        }

        public async void Add()
        {
            var navigationParams = new Dictionary<string, object> {
                { "Eating", null }
            };
            await Shell.Current.GoToAsync(nameof(MenageEatingPage), navigationParams);
        }

        private async void Update()
        {
            var navigationParams = new Dictionary<string, object> {
                { "Eating", SelectedEating }
            };
            await Shell.Current.GoToAsync(nameof(MenageEatingPage), navigationParams);
        }

        private async void Delete()
        {
            _repo.Delete(SelectedEating.Id);
            Refresh();
        }
    }
}
