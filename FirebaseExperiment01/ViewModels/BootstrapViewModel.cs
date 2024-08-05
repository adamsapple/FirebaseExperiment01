using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.ViewModels;

/// <summary>
/// 
/// </summary>
public partial class BootstrapViewModel : ViewModelBase
{
    #region Commands.
    [RelayCommand]
    private async Task Bootstrap()
    {
        await TryLoginAsync();
    }
    #endregion Commands.

    /// <summary>
    /// ctor
    /// </summary>
    public BootstrapViewModel()
    {
        Title = "Loading";
    }

    /// <summary>
    /// ログインを試行する
    /// </summary>
    /// <returns></returns>
    public async Task TryLoginAsync()
    {
        var page = Helpers.ServiceHelper.GetService<Views.LoginPage>();
        var shell = Helpers.ServiceHelper.GetService<AppShell>();
        App.Current.MainPage = shell;
        await Helpers.NavigationHelper.Shell.Navigation!.PushAsync(page, false);
    }
}
