namespace FirebaseExperiment01.Views;

public partial class LogoutedPage : ContentPage
{
	public LogoutedPage(ViewModels.LogoutedViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}