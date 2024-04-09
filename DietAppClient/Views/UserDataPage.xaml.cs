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

