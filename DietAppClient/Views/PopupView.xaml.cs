using CommunityToolkit.Maui.Views;

namespace DietAppClient.Views;

public partial class PopupView : Popup
{
    public PopupView(string message)
    {
        InitializeComponent();
        lb_text.Text = message;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }
}