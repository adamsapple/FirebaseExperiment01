namespace FirebaseExperiment01.Views;

public partial class OptionPage : ContentPage
{
	public OptionPage(ViewModels.OptionViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}