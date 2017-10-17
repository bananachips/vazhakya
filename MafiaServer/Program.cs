using Lidgren.Network;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace MafiaServer
{
  class Program
  {
    private struct ConnectionInfo
    {
      public string Name;
      public NetConnection Connection;
    }
    private static List<ConnectionInfo> _connectionInfoList = new List<ConnectionInfo>();
    private static void UpdateConnectionsList(NetConnection connection, NetConnectionStatus status)
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
      //foreach (NetConnection conn in conList)
      //{
      //  string str = NetUtility.ToHexString(conn.RemoteUniqueIdentifier) + " from " + conn.RemoteEndPoint.ToString() + " [" + conn.Status + "]";
      //  Console.WriteLine(str);
      //}
    }

    static void ProcessCommand(string action, string data)
    {

    }

    static void Output(string txt)
    {
      Console.WriteLine(txt);
    }
    static void RunServer(CancellationTokenSource token)
    {
      var config = new NetPeerConfiguration("Mafia") { Port = 12345 };
      var server = new NetServer(config);
      server.Start();
      while (!token.IsCancellationRequested)
      {
        
        NetIncomingMessage im;
        while ((im = server.ReadMessage()) != null)
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

              //if (status == NetConnectionStatus.Connected)
              //{
              //  string hail = im.SenderConnection.RemoteHailMessage.ReadString();
              //  Output("Remote hail: " + hail);
              //}
              //if (status == NetConnectionStatus.Disconnected)
              //{

              //}
              UpdateConnectionsList(im.SenderConnection, status );
              break;
            case NetIncomingMessageType.Data:
              string action = im.ReadString();
              string data = im.ReadString();
              Output(action + ":" + data);
              // incoming chat message from a client
              //string chat = im.ReadString();

              //Output("Broadcasting '" + chat + "'");

              //// broadcast this to all connections, except sender
              //List<NetConnection> all = server.Connections; // get copy
              //all.Remove(im.SenderConnection);

              //if (all.Count > 0)
              //{
              //  NetOutgoingMessage om = server.CreateMessage();
              //  om.Write(NetUtility.ToHexString(im.SenderConnection.RemoteUniqueIdentifier) + " said: " + chat);
              //  server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
              //}
              break;
            default:
              Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
              break;
          }
          server.Recycle(im);
        }
        Thread.Sleep(1);
      }
      Console.WriteLine("Server is exiting..");
      server.FlushSendQueue();
     
    }
    static void Main(string[] args)
    {
      CancellationTokenSource cancellationTokenSrc = new CancellationTokenSource();
      
      Task.Factory.StartNew(() => RunServer(cancellationTokenSrc));

      Console.WriteLine("Server is running - Press q to quit");
      char z = (char)(Console.Read());
      if ((z == 'q') || (z == 'Q'))
        cancellationTokenSrc.Cancel();

            
    }
  }
}
