using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DietAppClient.Data;
using DietAppClient.Models;
using DietAppClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    [QueryProperty(nameof(DoRefresh), "DoRefresh")]
    public class EatingsViewModel : INotifyPropertyChanged
    {
        IEatingRepository _repo;
        public bool DoRefresh { set { Refresh(); SelectedEating = Eatings.FirstOrDefault(t => t.Id == SelectedEating.Id); } }
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
        public async Task Refresh()
        {
            Eatings = new ObservableCollection<Eating>(_repo.ReadAll());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Eatings"));
        }


        public async void Add()
        {
            await Shell.Current.GoToAsync(nameof(MenageEatingPage) + "?DoChange=Add");
        }

        private async void Update()
        {
            await Shell.Current.GoToAsync(nameof(MenageEatingPage), new Dictionary<string, object> {
                { "Eating", SelectedEating },
                { "DoChange", "Edit" }
            });
        }

        private async void Delete()
        {
            _repo.Delete(SelectedEating.Id);
            Refresh();
            SelectedEating = null;
        }
    }
}
