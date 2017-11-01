using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaServerWPF.Models
{
	public class PlayerInfo
	{
		public PlayerInfo()
		{
			Name = "";
			IP = "localhost";
			Port = 8000;
		}
		public string Name { get; set; }
		public string IP { get; set; }
		public int Port { get; set; }

	}

	public class PlayManagerModel
	{
		public List<PlayerInfo> _playerList;
		
		ServerModel _server;
		public PlayManagerModel(ServerModel server)
		{
			_server = server;
			_server.PlayerJoined += _server_PlayerJoined;
		}

		private void _server_PlayerJoined(object sender, EventArgs args)
		{
			

		}

		
	}
}
