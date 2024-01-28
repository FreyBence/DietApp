using DietAppClient.Logics;
using DietAppClient.Models;
using DietAppClient.ViewModels;

namespace DietAppClient.Views;

public partial class UserDataPage : ContentPage
{
    public UserDataPage(UserDataViewModel vm)
	{
        InitializeComponent();
        this.BindingContext = vm;
	}
}

