namespace FirebaseExperiment01.Views;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage(ViewModels.MainTabbedViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        Children.Add(Helpers.ServiceHelper.GetService<HomePage>());
        Children.Add(Helpers.ServiceHelper.GetService<OptionPage>());
    }
}