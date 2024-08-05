using CommunityToolkit.Mvvm.Input;
using FirebaseExperiment01.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [RelayCommand]
    private async Task Logout()
    {
        var page = Helpers.ServiceHelper.GetService<LogoutedPage>();
        var navi = Helpers.NavigationHelper.Shell.Navigation;
        await Helpers.NavigationHelper.Shell.GoToAsync("///Blank", false);
        await navi.PushModalAsync(page);
    }

    /// <summary>
    /// ctor.
    /// </summary>
    public HomeViewModel() 
    {
        Title= "Home";
    }
}
