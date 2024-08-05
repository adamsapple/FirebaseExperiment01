namespace FirebaseExperiment01
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            Services = services;
            InitializeComponent();

            var page = Helpers.ServiceHelper.GetService<Views.BootstrapPage>();
            //var shell = Helpers.ServiceHelper.GetService<AppShell>();
            MainPage = page;

            //Helpers.NavigationHelper.Shell.Navigation.PushAsync(page, false);

            //var navi = Helpers.ServiceHelper.GetService<NavigationPage>();
            //navi.PushAsync(page);
            //MainPage = navi;
        }
    }
}
