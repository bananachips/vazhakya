using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;

namespace MafiaServerWPF.ViewModels
{
    public struct PlayerInfo
    {
        public string Name { get; set; }
    }
    public class PlayManagerViewModel: BaseViewModel
    {
        private string _test;
        private bool _isVisible = true;

        public bool isPlayManagerVisible
        {
            get { return _isVisible; }
            set { SetField(ref _isVisible, value, "isPlayManagerVisible"); }
        }

        public string Test
        {
            get { return _test; }
            set { SetField(ref _test, value, "Test"); }
        }

    }
}
