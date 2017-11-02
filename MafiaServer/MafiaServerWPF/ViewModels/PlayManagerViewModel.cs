using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtils;
using MafiaServerWPF.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace MafiaServerWPF.ViewModels
{


	public class PlayManagerViewModel : BaseViewModel, IDataErrorInfo
	{
	
		private PlayManagerModel _playManagerModel;
		private ServerModel _serverModel;
		public ObservableCollection<PlayerInfo> PlayerList { get; set; }
		public int NumMafias { get; set; }
		public bool HasDoctor { get; set; }

		public bool HasJoker { get; set; }

		public bool HasDetective { get; set; }

		public DelegateCommand PlayCommand { get; private set; }

		public string Error
		{
			get { return string.Empty; }
		}

		public List<PlayerInfo> Mafias;
		public List<PlayerInfo> Villagers;
		public List<PlayerInfo> Specialists;

		public string this[string propertyName]
		{
			get
			{
				if (propertyName == nameof(NumMafias))
				{

					if (NumMafias < 1 /*|| NumMafias > PlayerList.Count/2*/)
						return "Number of Mafias cannot be less than " + NumMafias ;
				}
				return string.Empty;
			}
		}

		public PlayManagerViewModel(PlayManagerModel pm, ServerModel sm)
		{
			_serverModel = sm;
			_playManagerModel = pm;
			_serverModel.PlayerJoined += _serverModel_PlayerJoined;
			_serverModel.PlayerLeft += _serverModel_PlayerLeft; ;
			PlayerList = new ObservableCollection<PlayerInfo>();
			
			NumMafias = 2;
			HasDetective = true;
			HasDoctor = true;
			HasJoker = true;

			// for testing 
			PlayerList.Add(new PlayerInfo() { Name = "Anjali" });
			PlayerList.Add(new PlayerInfo() { Name = "Abhilash" });
			PlayerList.Add(new PlayerInfo() { Name = "Augustus" });
			PlayerList.Add(new PlayerInfo() { Name = "Chayanika" });
			PlayerList.Add(new PlayerInfo() { Name = "Karishma" });
			PlayerList.Add(new PlayerInfo() { Name = "Rhea" });
			PlayerList.Add(new PlayerInfo() { Name = "Reshmi" });
			PlayCommand = new DelegateCommand(OnPlay);
		}

		private void OnPlay(object obj)
		{
			IEnumerable<PlayerInfo> temp =(IEnumerable<PlayerInfo>)PlayerList.GetEnumerator();
			List<PlayerInfo> p =  temp.ToList();
			CommonUtils.Utilities.Shuffle(ref p);
			
			//Create Mafia List
			//Create Villager List
			//Create SpecialChar List

			for (int i = 0; i < PlayerList.Count; ++i)
			{


			}
		}

		private void _serverModel_PlayerLeft(object sender, EventArgs args)
		{
			ConnectionInfo info = (ConnectionInfo)sender;
			PlayerInfo pi = new PlayerInfo() { Name = info.Name, IP = info.Connection.RemoteEndPoint.Address.ToString(), Port = 8000 };
			Application.Current.Dispatcher.Invoke(() => { PlayerList.Remove(pi); });
		}

		private void _serverModel_PlayerJoined(object sender, EventArgs args)
		{
			ConnectionInfo info = (ConnectionInfo)sender;
			PlayerInfo pi = new PlayerInfo() { Name = info.Name, IP = info.Connection.RemoteEndPoint.Address.ToString(), Port = 8000 };

			//This type of CollectionView does not support changes to its SourceCollection from a thread different from the Dispatcher thread
			// hence the below
			Application.Current.Dispatcher.Invoke(() => PlayerList.Add(pi));

		}


	}
}
