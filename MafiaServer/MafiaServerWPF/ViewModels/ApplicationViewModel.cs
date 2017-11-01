using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils;
using System.Windows.Input;
using MafiaServerWPF.Models;

namespace MafiaServerWPF.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
	{
		public ICommand StartCommand { get; private set; }
		//public ICommand SwitchCommand { get; private set; }
		private BaseViewModel _currentVM;
		private ServerViewModel _serverVM;
		private ServerModel _serverModel;
		private PlayManagerViewModel _playManagerVM;
		private PlayManagerModel _playManagerModel;
		
		public string AppName { get { return "hello"; } }

		public BaseViewModel CurrentViewModel
		{
			get { return _currentVM; }
			set { SetField(ref _currentVM, value, "CurrentViewModel"); }
		}

		
		public ApplicationViewModel()
		{
			_serverModel = new ServerModel();
			_serverVM = new ServerViewModel(_serverModel);
			//_serverVM.StartCommand = new DelegateCommand(OnStart);
			_playManagerModel = new PlayManagerModel(_serverModel);
			_playManagerVM = new PlayManagerViewModel(_playManagerModel);
			_serverModel.ServerStarted += _server_started;
			CurrentViewModel = _serverVM;
			
		}

		void OnSwitch(object obj)
		{
			
		}

		private void _server_started(object sender, EventArgs args)
		{
			CurrentViewModel = _playManagerVM;

		}

	}
}
