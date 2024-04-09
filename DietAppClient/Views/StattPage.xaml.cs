using DietAppClient.ViewModels;

namespace DietAppClient.Views;

public partial class StattPage : ContentPage
{
    StattViewModel _vm;
    public StattPage(StattViewModel vm)
    {
        _vm = vm;
        InitializeComponent();
        this.BindingContext = vm;
    }

    private void btn_Data_Clicked(object sender, EventArgs e)
    {
        i_chart_weight.IsVisible = false;
        i_chart_fat.IsVisible = false;
        i_chart_exp.IsVisible = false;
        sl_Datas.IsVisible = true;
    }

    private void btn_Weight_Clicked(object sender, EventArgs e)
    {
        i_chart_weight.IsVisible = true;
        i_chart_fat.IsVisible = false;
        i_chart_exp.IsVisible = false;
        sl_Datas.IsVisible = false;
    }

    private void btn_Fat_Clicked(object sender, EventArgs e)
    {
        i_chart_weight.IsVisible = false;
        i_chart_fat.IsVisible = true;
        i_chart_exp.IsVisible = false;
        sl_Datas.IsVisible = false;
    }

    private void btn_Exp_Clicked(object sender, EventArgs e)
    {
        i_chart_weight.IsVisible = false;
        i_chart_fat.IsVisible = false;
        i_chart_exp.IsVisible = true;
        sl_Datas.IsVisible = false;
    }
}