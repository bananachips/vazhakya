using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiaServerWPF.Models;
using System.Windows.Controls;
using System.Globalization;
using CommonUtils;

namespace MafiaServerWPF.ViewModels
{

	internal class NumericValidationRule : ValidationRule
	{
		public Type ValidationType { get; set; }
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string strValue = Convert.ToString(value);

			if (string.IsNullOrEmpty(strValue))
				return new ValidationResult(false, "Value cannot be coverted to string.");
			bool canConvert = false;
			switch (ValidationType.Name)
			{

				case "Boolean":
					bool boolVal = false;
					canConvert = bool.TryParse(strValue, out boolVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "Input should be type of boolean");
				case "Int32":
					int intVal = 0;
					canConvert = int.TryParse(strValue, out intVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "Input should be type of Int32");
				case "Double":
					double doubleVal = 0;
					canConvert = double.TryParse(strValue, out doubleVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "Input should be type of Double");
				case "Int64":
					long longVal = 0;
					canConvert = long.TryParse(strValue, out longVal);
					return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, "Input should be type of Int64");
				default:
					throw new InvalidCastException("{ValidationType.Name} is not supported");
			}
		}
	}


	class ServerViewModel : BaseViewModel
	{
		private ServerModel _serverModel;
		PlayManagerViewModel _playManagerVM;
		public ICommand StartCommand { get; set; }
		public ICommand SwitchCommand { get; set; }
		private string _status;
		private int _port = 8090;
		private string _buttonName = "Start";
		private bool _isVisible = true;

		public ServerViewModel(ServerModel sm)
		{
			StartCommand = new DelegateCommand(OnStart);
			_serverModel = sm;
		}

		public bool isServerManagerVisible
		{
			get { return _isVisible; }
			set { SetField(ref _isVisible, value, "isServerManagerVisible"); }
		}

		public int Port
		{
			get { return _port; }
			set { SetField(ref _port, value, "Port"); }
		}

		public string StartOrStop
		{
			get { return _buttonName; }
			set { SetField(ref _buttonName, value, "StartOrStop"); }
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

		void OnStart(object param)
		{
			if (_serverModel.Initialize(_port))
			{
				isServerManagerVisible = false;
			}
			StatusMessage = _serverModel.Status;
			
		}



	}
}
