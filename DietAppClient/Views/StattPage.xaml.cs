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
        i_chart.Source = null;
        sl_Datas.Children.Clear();
		foreach (var item in _vm.SwitchToData())
		{
            sl_Datas.Add(item);
		}
    }

    private void btn_Weight_Clicked(object sender, EventArgs e)
    {
        sl_Datas.Children.Clear();
        i_chart.Source = _vm.SwitchToWeight();
    }

    private void btn_Fat_Clicked(object sender, EventArgs e)
    {
        sl_Datas.Children.Clear();
        i_chart.Source = _vm.SwitchToFat();
    }

    private void btn_Exp_Clicked(object sender, EventArgs e)
    {
        sl_Datas.Children.Clear();
        i_chart.Source = _vm.SwitchToExp();
    }
}