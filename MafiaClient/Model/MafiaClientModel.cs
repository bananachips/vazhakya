using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Threading;

namespace MafiaClient.Model
{
  public class MafiaClientModel
  {
    private static NetClient _client;
    private NetPeerConfiguration _config;
    private int _port = 12345;
    public MafiaClientModel()
    {
      _config = new NetPeerConfiguration("Mafia");
      _config.Port = 23456;
      _client = new NetClient(_config);
      _client.RegisterReceivedCallback(new SendOrPostCallback(GotMessage));
    }
    public bool Connect(string host)
    {
      _client.Start();
      NetConnection nc = _client.Connect("127.0.0.1", _port);

      return true;
    }

    public static void GotMessage(object peer)
    {
      NetIncomingMessage im;
      while ((im = _client.ReadMessage()) != null)
      {
        // handle incoming message
        switch (im.MessageType)
        {
          case NetIncomingMessageType.DebugMessage:
          case NetIncomingMessageType.ErrorMessage:
          case NetIncomingMessageType.WarningMessage:
          case NetIncomingMessageType.VerboseDebugMessage:
            string text = im.ReadString();
            //Output(text);
            break;
          case NetIncomingMessageType.StatusChanged:
            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

            if (status == NetConnectionStatus.Connected)
            {
              
            }
            if (status == NetConnectionStatus.Disconnected)
            {
             
            }
            string reason = im.ReadString();
            //Output(status.ToString() + ": " + reason);

            break;
          case NetIncomingMessageType.Data:
            string chat = im.ReadString();
           //Output(chat);
            break;
          default:
            //Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
            break;
        }
        _client.Recycle(im);
      }
    }

  }
}
