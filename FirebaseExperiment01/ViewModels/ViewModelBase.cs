using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseExperiment01.ViewModels;

/// <summary>
/// 
/// </summary>
//[INotifyPropertyChanged]
public partial class ViewModelBase : ObservableObject, IQueryAttributable
{
    #region Properties.
    [ObservableProperty]
    private string _title = string.Empty;


    private bool _isBusy = false;

    public virtual bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);        
    }

   
    #endregion Properties.

    #region IQueryAttributable.
    /// <summary>
    /// 画面遷移時のQuery(引数)を受け取る為のハンドラ
    /// </summary>
    /// <param name="query"></param>
    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    #endregion IQueryAttributable.
}
