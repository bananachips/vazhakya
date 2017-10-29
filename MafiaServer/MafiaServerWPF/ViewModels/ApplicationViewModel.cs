using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils;
using System.Windows.Input;
namespace MafiaServerWPF.ViewModels
{
    public class ApplicationViewModel: BaseViewModel
    {
        public ICommand StartCommand { get; private set; }
        //public ICommand SwitchCommand { get; private set; }
        private BaseViewModel _currentVM;
        public string AppName { get { return "hello"; } }
        public BaseViewModel CurrentViewModel
        {
            get { return _currentVM; }
            set { SetField(ref _currentVM, value, "CurrentViewModel"); }
        }

        private List<BaseViewModel> _VMList = new List<BaseViewModel>(){new ServerViewModel(), new PlayManagerViewModel()};

        public ApplicationViewModel()
        {
            (_VMList[0] as ServerViewModel).SwitchCommand = new DelegateCommand(OnSwitch);
            CurrentViewModel = _VMList[0];
            //SwitchCommand = new DelegateCommand(OnSwitch);
            StartCommand = new DelegateCommand(OnStart);
        }

        void OnSwitch(object obj)
        {
            CurrentViewModel = _VMList[1];
        }

        void OnStart(object param)
        {
            CurrentViewModel = _VMList[1];
        }
        
    }
}
