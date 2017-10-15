using Lidgren.Network;

namespace MafiaServer
{
  class Program
    {
        static void Main(string[] args)
        {
            var config = new NetPeerConfiguration("Mafia") { Port = 12345 };
            var server = new NetServer(config);
            server.Start();
            NetIncomingMessage im;
            while ((im = server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    
                }
            }
        }
    }
}
