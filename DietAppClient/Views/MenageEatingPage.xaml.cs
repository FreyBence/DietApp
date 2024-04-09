using DietAppClient.ViewModels;

namespace DietAppClient.Views
{
    public partial class MenageEatingPage : ContentPage
    {
        MenageEatingViewModel _vm;
        public MenageEatingPage(MenageEatingViewModel vm)
        {
            _vm = vm;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.Refresh();
        }
    }
}