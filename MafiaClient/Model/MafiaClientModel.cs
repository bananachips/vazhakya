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
    private NetClient _client;
    private Action<string, int> _statusCallback;
    private NetPeerConfiguration _config;
    private int _port = 12345;
    public string _name = "lazybum";
    public  string StatusMessage;
    public MafiaClientModel()
    {
      _config = new NetPeerConfiguration("Mafia");
      _config.Port = 23456;
      _client = new NetClient(_config);
      _client.RegisterReceivedCallback(GotMessage);
    }

    
    public bool Connect(string host, Action<string, int> callback)
    {
      _client.Start();
      _statusCallback = callback;
      NetOutgoingMessage hail = _client.CreateMessage(_name);
      NetConnection nc = _client.Connect("127.0.0.1", _port, hail);
      //SendMessage("play", "ready");
      return true;
    }

    public void Disconnect()
    {
      _client.Disconnect("");
    }

    void OnMessage(string message, int type)
    {
      if (_statusCallback != null)
        _statusCallback(message, type);

    }

    public void SendMessage(string action, string data)
    {
      NetOutgoingMessage om = _client.CreateMessage();
      om.Write(action);
      om.Write(data);
      _client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
      OnMessage("Sending '" + action + ":" + data, 10);
      _client.FlushSendQueue();
    }

    public  void GotMessage(object peer)
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
            OnMessage(text);
            break;
          case NetIncomingMessageType.StatusChanged:
            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

            if (status == NetConnectionStatus.Connected)
            {
              OnMessage("Connected", NetConnectionStatus.Connected);
              SendMessage("play", "data");
            }
            if (status == NetConnectionStatus.Disconnected)
            {
              OnMessage("Disconnected");
            }
            string reason = im.ReadString();
            OnMessage(status.ToString() + ": " + reason);

            break;
          case NetIncomingMessageType.Data:
            string chat = im.ReadString();
            OnMessage(chat);
            break;
          default:
            OnMessage("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
            break;
        }
        _client.Recycle(im);
      }
    }

  }
}
