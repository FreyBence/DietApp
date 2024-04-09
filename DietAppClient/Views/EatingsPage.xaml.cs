using DietAppClient.ViewModels;

namespace DietAppClient.Views
{
    public partial class EatingsPage : ContentPage
    {
        EatingsViewModel _vm;
        public EatingsPage(EatingsViewModel vm)
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