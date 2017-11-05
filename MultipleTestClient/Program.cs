using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;
using CommonUtils;

namespace MultipleTestClient
{
  class Program
  {
    static List<string> players = new List<string>() { "Anjali", "Rhea", "Karishma", "Chayanika", "Reshmi", "Abhilash", "Augustus" };
    static void Main(string[] args)
    {
      CancellationTokenSource ct = new CancellationTokenSource();
      foreach(var x in players)
        CreateClientAsync(x, ct);
      
      Console.WriteLine("Press q to quit");
      char z = (char)(Console.Read());
      if ((z == 'q') || (z == 'Q'))
        ct.Cancel();

    }

    async void CreateClient(string name, CancellationTokenSource ct)
    {
      await CreateClientAsync(name, ct);
    }

    static Task CreateClientAsync(string name, CancellationTokenSource ct)
    {

      return Task.Factory.StartNew(() => 
      {
        Console.WriteLine("Starting client for " + name);
        NetClient _client;
        NetPeerConfiguration _config;
        _config = new NetPeerConfiguration("Mafia");
        // _config.Port = 23456;
        _client = new NetClient(_config);
        _client.RegisterReceivedCallback(GotMessage, new System.Threading.SynchronizationContext());
        _client.Start();
        
        NetOutgoingMessage hail = _client.CreateMessage(name);
        NetConnection nc = _client.Connect("127.0.0.1", 8090, hail);
        do
        {
          if (ct.IsCancellationRequested)
            break;

          Thread.Sleep(2000);
        }
        while (true);
      });
    }

    static void OnMessage(string text, NetClient peer)
    {
      Console.WriteLine(peer.Port + ":" + text);
    }

    static void GotMessage(object peer)
    {
      NetClient _client = (NetClient)peer;
      if (_client == null) 
        return;
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
            OnMessage(text, _client);
            break;
          case NetIncomingMessageType.StatusChanged:
            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

            if (status == NetConnectionStatus.Connected)
            {
              OnMessage("Connected", _client);
              //SendMessage("play", "data");
            }
            if (status == NetConnectionStatus.Disconnected)
            {
              OnMessage("Disconnected", _client);
            }
            string reason = im.ReadString();

            //OnMessage(status.ToString() + ": " + reason);

            break;
          case NetIncomingMessageType.Data:
            string chat = im.ReadString();
            KeyValue<string> x = new KeyValue<string>(chat);
            OnMessage(x.key + " is " + x.value, _client);
            break;
          default:
            OnMessage("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes", _client);
            break;
        }
        _client.Recycle(im);
      }
    }
  }
}
