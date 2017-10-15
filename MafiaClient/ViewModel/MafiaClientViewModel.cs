using MafiaClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;
using System.DirectoryServices;

namespace MafiaClient.ViewModel
{
  public class DelegateCommand : ICommand
  {
    Action<object> _function;

    private MafiaClientViewModel mafiaClientViewModel;

    public DelegateCommand(Action<object> _action)
    {
      _function = _action;
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
  public class MafiaClientViewModel : INotifyPropertyChanged
  {
    private MafiaClientModel _mafiaClientModel = new MafiaClientModel();

    public event PropertyChangedEventHandler PropertyChanged;
    public ICommand JoinCommand { get; private set; }

    private string _name;
    private string _host;
    public string Name
    {
      get { return _name; }

      set
      {
        if (_name != value)
        {
          _name = value;
          RaisePropertyChanged("Name");
        }
      }

    }

    public string ServerHost
    {
      get { return _host; }

      set
      {
        if (_host != value)
        {
          _host = value;
          RaisePropertyChanged("ServerHost");
        }
      }

    }

    public MafiaClientViewModel()
    {
      JoinCommand = new DelegateCommand(OnJoin);
    }

    public void OnJoin(object parameter)
    {
      bool success = _mafiaClientModel.Connect(_host);
      Debug.WriteLine("On join called");
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
