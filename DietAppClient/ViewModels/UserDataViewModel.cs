using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DietAppClient.Data;
using DietAppClient.Models;
using DietAppClient.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DietAppClient.ViewModels
{
    public class UserDataViewModel : INotifyPropertyChanged
    {
        IUserRepository _repo;
		private User user;

        public User User 
		{
			get { return user; }
			set 
			{
                user = value?.GetCopy(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User"));
            }
		}

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OnSaveClicked { get; set; }

        public UserDataViewModel( IUserRepository repo)
		{
			_repo = repo;
			User = _repo.Read();

			OnSaveClicked = new Command(Update);
        }

        public async void Update() 
		{
            _repo.Update(User);
        }
    }
}
