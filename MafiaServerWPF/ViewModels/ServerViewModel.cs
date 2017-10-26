using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiaServerWPF.Models;

namespace MafiaServerWPF.ViewModels
{
  internal class DelegateCommand : ICommand
  {
    Action<object> _function;

    public DelegateCommand(Action<object> action)
    {
      _function = action;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      if (_function != null)
        _function(parameter);

    }
  }

  class ServerViewModel : INotifyPropertyChanged
  {
    private ServerModel _serverModel;
    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand StartCommand{ get; private set; }
    private string _status;
    public ServerViewModel()
    {
      StartCommand = new DelegateCommand(OnStart);
      _serverModel = new ServerModel();
      if (!_serverModel.Initialize())
        StatusMessage = _serverModel.Status;
    }
    
    public string StatusMessage
    {
      get { return _status; }
      set
      {
        if (_status != value)
        {
          _status = value;
          RaisePropertyChanged(nameof(StatusMessage));
        }
      }
    }
    void OnStart(object param)
    {
      Debug.WriteLine(param);

    }

    private void RaisePropertyChanged(string property)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

  }
}
