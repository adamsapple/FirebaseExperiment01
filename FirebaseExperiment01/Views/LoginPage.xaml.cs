namespace FirebaseExperiment01.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(ViewModels.LoginViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}