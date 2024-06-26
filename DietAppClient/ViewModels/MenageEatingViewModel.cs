﻿using DietAppClient.Data;
using DietAppClient.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    [QueryProperty(nameof(Eating), "Eating")]
    public class MenageEatingViewModel : INotifyPropertyChanged
    {
        IEatingRepository _repo;

        private Eating eating;

        public Eating Eating
        {
            get { return eating; }
            set
            {
                eating = value?.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Eating"));
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

        public event PropertyChangedEventHandler PropertyChanged;

        public MenageEatingViewModel(IEatingRepository repo)
        {
            _repo = repo;
        }

        public void Refresh()
        {
            if (Eating == null)
            {
                Eating = new Eating();
                Title = "Add Eating";
                ButtonText = "Add";
                OnButtonClicked = new Command(OnAddClicked);
            }
            else
            {
                Title = "Edit Eating";
                ButtonText = "Edit";
                OnButtonClicked = new Command(OnEditClicked);
            }
        }

        private async void OnAddClicked()
        {
            _repo.Create(Eating);
            await Shell.Current.GoToAsync($"../");
        }

        private async void OnEditClicked()
        {
            _repo.Update(Eating);
            await Shell.Current.GoToAsync($"../");
        }

    }
}
