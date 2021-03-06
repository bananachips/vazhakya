﻿using System;
using Lidgren.Network;
using System.Web.Script.Serialization;
using CommonUtils;

namespace MafiaClient.Model
{
  public delegate void RoleSetEventHandler(object sender, EventArgs args);

	public class MafiaClientModel
  {
    private NetClient _client;
    private Action<string, int> _statusCallback;
    private NetPeerConfiguration _config;
    public int _port = 8000;
    public string _name = "lazybum";
		private JavaScriptSerializer _jSerializer;
    public  string StatusMessage;
    public event RoleSetEventHandler RoleSetEvent;
    public MafiaClientModel()
    {
      _config = new NetPeerConfiguration("Mafia");
     // _config.Port = 23456;
      _client = new NetClient(_config);
      _client.RegisterReceivedCallback(GotMessage);
			_jSerializer = new JavaScriptSerializer();
			_jSerializer.MaxJsonLength = Int32.MaxValue;
		}

    public string Name
    {
      get { return _name; }
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

    void OnMessage(string message, int type =1)
    {
      if (_statusCallback != null)
        _statusCallback(message, type);

    }

    void ProcessMessage(string data)
    {
      KeyValue<string> x = new KeyValue<string>(data);
      if (String.Compare(x.key, "Role", true) == 0)
      {
        if (RoleSetEvent != null)
        {
          RoleSetEvent.Invoke(x.value, null);
          return;
        }
      }
      if (String.Compare(x.key, "ChatMessage", true) == 0)
      {
        KeyValue<string> messageKV = new KeyValue<string>(x.value);

        

      }
    }
    public void SendMessage(string action, string data)
    {
      NetOutgoingMessage om = _client.CreateMessage();
			string message = new KeyValue<string>() { key = action, value = data }.GetXml();
			//Tuple<string, string> message = new Tuple<string, string>(action, data);
			//XmlSerializer xmlSerializer = new XmlSerializer(message.GetType());
			//StringWriter textWriter = new StringWriter();
			//xmlSerializer.Serialize(textWriter, message);
			//string zz =  textWriter.ToString();
			//string m = _jSerializer.Serialize(message);
      om.Write(message);
      
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
              OnMessage("Connected");
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
            ProcessMessage(im.ReadString());
            string data = im.ReadString();
            if (!String.IsNullOrEmpty(data))
            {
              KeyValue<string> x = new KeyValue<string>(data);
              OnMessage("data from client - key:" + x.key + " value:" + x.value);
            }
            //OnMessage(chat);
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
