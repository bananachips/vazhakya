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
using System.Windows.Controls;
using System.Globalization;

namespace MafiaClient.ViewModel
{
  


  internal class NumericValidationRule : ValidationRule
  {
    public Type ValidationType { get; set; }
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      string strValue = Convert.ToString(value);

      if (string.IsNullOrEmpty(strValue))
        return new ValidationResult(false, $"Value cannot be coverted to string.");
      bool canConvert = false;
      switch (ValidationType.Name)
      {

        case "Boolean":
          bool boolVal = false;
          canConvert = bool.TryParse(strValue, out boolVal);
          return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean");
        case "Int32":
          int intVal = 0;
          canConvert = int.TryParse(strValue, out intVal);
          return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int32");
        case "Double":
          double doubleVal = 0;
          canConvert = double.TryParse(strValue, out doubleVal);
          return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
        case "Int64":
          long longVal = 0;
          canConvert = long.TryParse(strValue, out longVal);
          return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
        default:
          throw new InvalidCastException($"{ValidationType.Name} is not supported");
      }
    }

    
  }
  public class DelegateCommand : ICommand
  {
    Action<object> _function;
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
    public ICommand CancelCommand { get; set; }

    private string _name;
    private string _host;
    private string _status;
    private int _port;
    public string Name
    {
      get { return _name; }

      set
      {
        if (_name != value)
        {
          _name = value;
          RaisePropertyChanged("Name");
          _mafiaClientModel._name = _name;
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
    public int ServerPort
    {
      get { return _port; }

      set
      {
        if (_port != value)
        {
          _port = value;
					_mafiaClientModel._port = _port;

					RaisePropertyChanged("ServerPort");
        }
      }

    }

    public string StatusMessage
    {
      get { return _status; }

      set
      {
        if (_status != value)
        {
          _status = value;
          RaisePropertyChanged("StatusMessage");
        }
      }

    }

    public MafiaClientViewModel()
    {
      JoinCommand = new DelegateCommand(OnJoin);
      CancelCommand = new DelegateCommand(OnCancel);
    }

    private void OnCancel(object obj)
    {
      _mafiaClientModel.Disconnect();
    }

    public void OnJoin(object parameter)
    {
      bool success = _mafiaClientModel.Connect(_host, StatusMessageCallback);
      Debug.WriteLine("On join called");
    }

    private void RaisePropertyChanged(string property)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    private void StatusMessageCallback(string msg, int type)
    {
      StatusMessage = msg;
      
    }
  }
}
