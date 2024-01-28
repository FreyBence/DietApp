using CommunityToolkit.Mvvm.ComponentModel;
using DietAppClient.Models;
using DietAppClient.ViewModels;
using DietAppClient.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace DietAppClient.Views
{
    public partial class EatingsPage : ContentPage
    {
        public EatingsPage(EatingsViewModel vm)
        { 
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}