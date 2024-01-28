using DietAppClient.Models;
using DietAppClient.ViewModels;

namespace DietAppClient.Views
{
    public partial class MenageEatingPage : ContentPage
    {
        public MenageEatingPage(MenageEatingViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}