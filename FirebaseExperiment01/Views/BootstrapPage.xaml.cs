namespace FirebaseExperiment01.Views;

public partial class BootstrapPage : ContentPage
{
	#region DI.
	//private ViewModels.BootstrapViewModel vm;
    #endregion DI.

    public BootstrapPage(ViewModels.BootstrapViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
    }
}