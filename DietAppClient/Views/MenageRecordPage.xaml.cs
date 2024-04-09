using DietAppClient.ViewModels;

namespace DietAppClient.Views;

public partial class MenageRecordPage : ContentPage
{
    MenageRecordViewModel _vm;
    public MenageRecordPage(MenageRecordViewModel vm)
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