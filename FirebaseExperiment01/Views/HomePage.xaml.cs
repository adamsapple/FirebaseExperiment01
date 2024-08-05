namespace FirebaseExperiment01.Views;

public partial class HomePage : ContentPage
{
	public HomePage(ViewModels.HomeViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}