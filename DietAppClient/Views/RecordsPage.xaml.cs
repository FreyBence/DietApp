using DietAppClient.ViewModels;

namespace DietAppClient.Views;

public partial class RecordsPage : ContentPage
{
    RecordsViewModel _vm;
    public RecordsPage(RecordsViewModel vm)
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