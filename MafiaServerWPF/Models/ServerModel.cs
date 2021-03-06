﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Diagnostics;
using System.Threading;

namespace MafiaServerWPF.Models
{
  struct ConnectionInfo
  {
    public string Name;
    public NetConnection Connection;
  }
  internal class ServerModel
  {
    private static List<ConnectionInfo> _connectionInfoList = new List<ConnectionInfo>();
    NetServer _server;
    CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
    public bool IsCancelled { get; set; }
    public string Status { get; private set; }
    public ServerModel()
    {
      
      var config = new NetPeerConfiguration("Mafia") { Port = 1234 };
      _server = new NetServer(config);
        
      
    }

    public bool Initialize()
    {
      try
      {
        _server.Start();
        IsCancelled = false;
        Debug.WriteLine("Server has started");
        RunListener();
        Debug.WriteLine("Server is listening");
        return true;
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.ToString());
        Status = ex.ToString();
      }
      return false;
    }
    private void UpdateConnectionsList(NetConnection connection, NetConnectionStatus status)
    {
      if (status == NetConnectionStatus.Connected)
      {
        string name = connection.RemoteHailMessage.ReadString();
        Output(name + " @" + connection.RemoteEndPoint + " has connected");
        _connectionInfoList.Add(new ConnectionInfo { Name = name, Connection = connection });
      }
      else if (status == NetConnectionStatus.Disconnected)
      {
        ConnectionInfo info = _connectionInfoList.Find((x) => { return x.Connection == connection; });
        Output(info.Name + " @" + connection.RemoteEndPoint + " has disconnected");
        _connectionInfoList.Remove(info);
        // _connectionInfoList.RemoveAll((x)=> { return x.Connection == connection;});
      }

    }

    void ProcessCommand(string action, string data)
    {

    }

    void Output(string txt)
    {
      Debug.WriteLine(txt);
    }

    private async void RunListener()
    {
      await RunListenerAsync();
    }
    private Task RunListenerAsync()
    {
      return Task.Factory.StartNew(() =>
        {
          while (!_cancelTokenSource.IsCancellationRequested)
          {
            NetIncomingMessage im;
            while ((im = _server.ReadMessage()) != null)
            {
              // handle incoming message
              switch (im.MessageType)
              {
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.ErrorMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.VerboseDebugMessage:
                  string text = im.ReadString();
                  Output(text);
                  break;

                case NetIncomingMessageType.StatusChanged:
                  NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();

                  string reason = im.ReadString();
                  Output(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                  UpdateConnectionsList(im.SenderConnection, status);
                  break;
                case NetIncomingMessageType.Data:
                  string action = im.ReadString();
                  string data = im.ReadString();
                  Output(action + ":" + data);
                  // incoming chat message from a client
                  //string chat = im.ReadString();

                  //Output("Broadcasting '" + chat + "'");

                  //// broadcast this to all connections, except sender
                  //List<NetConnection> all = _server.Connections; // get copy
                  //all.Remove(im.SenderConnection);

                  //if (all.Count > 0)
                  //{
                  //  NetOutgoingMessage om = _server.CreateMessage();
                  //  om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
                  //  _server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                  //}
                  break;
                default:
                  Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                  break;
              }
              _server.Recycle(im);
            }
            Thread.Sleep(1);
          }
        }
      );

    }


  }
}
