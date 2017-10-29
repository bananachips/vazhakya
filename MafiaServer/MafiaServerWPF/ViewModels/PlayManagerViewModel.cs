using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;
using MafiaServerWPF.Models;

namespace MafiaServerWPF.ViewModels
{
	

	public class PlayManagerViewModel : BaseViewModel
	{
		private string _test;
		private bool _isVisible = true;
		private PlayManagerModel _playManagerModel = new PlayManagerModel();

		public PlayManagerViewModel()
		{
			PlayerList = new List<PlayerInfo>();
			PlayerList.Add(new PlayerInfo() { Name = "Anjali" });
			PlayerList.Add(new PlayerInfo() { Name = "Sachin" });
			PlayerList.Add(new PlayerInfo() { Name = "Meera" });
		}

		public List<PlayerInfo> PlayerList
		{
			get { return _playManagerModel._playerList; }
			set { SetField(ref _playManagerModel._playerList, value, "PlayerList"); }
		}

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
