using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.ViewModels;

public partial class LogoutedViewModel : ViewModelBase
{
    #region Commands.
    [RelayCommand]
    private async Task GoToLoginPage()
    {
        var page = Helpers.ServiceHelper.GetService<Views.LoginPage>();
        var navi = Helpers.NavigationHelper.Shell.Navigation;
        
        await navi.PopAsync(false);
        await navi.PushModalAsync(page, false);
    }
    #endregion Commands.

    public LogoutedViewModel()
    {
        Title = "Logouted bye";
    }
}
