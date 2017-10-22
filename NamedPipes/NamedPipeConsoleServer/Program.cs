using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace NamedPipeConsoleServer
{
    class Program
    {
        public class ImageInfo
        {
            public UInt32 width;
            public UInt32 height;

        }
        static void Main(string[] args)
        {
            
            NamedPipeServerStream serverStream = new NamedPipeServerStream("ANKsImageViewer", PipeDirection.In, 1, PipeTransmissionMode.Message);
            Console.WriteLine("waiting for connection..");
            serverStream.WaitForConnection();
            Console.WriteLine("received connection..");

            ReadImageInfo(serverStream);

            Console.Read();
            serverStream.Close();
            //RunServer();
        }

        static void  ReadMessage(NamedPipeServerStream serverStream)
        {
            MemoryStream memStream = new MemoryStream();
            
            do
            {

                Byte[] buffer = new Byte[1024];
                serverStream.Read(buffer, 0, 1024);
            }
            while (!serverStream.IsMessageComplete);
        }
        static void ReadImageInfo(NamedPipeServerStream serverStream)
        {
            int width, height, channels;
            Byte[] buffer = new Byte[sizeof(int)];
            serverStream.Read(buffer, 0, 4);
            width = BitConverter.ToInt32(buffer, 0);
            serverStream.Read(buffer, 0, 4);
            height = BitConverter.ToInt32(buffer, 0);
            serverStream.Read(buffer, 0, 4);
            channels = BitConverter.ToInt32(buffer, 0);
        

            Console.WriteLine("client sent {0} x {1} x {2}", width, height, channels);

        
        }
        static async void RunServer()
        {
            await NamedServer();
        }

        static Task NamedServer()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    NamedPipeServerStream serverStream = new NamedPipeServerStream("ANKsImageViewer", PipeDirection.In);
                    serverStream.WaitForConnection();

                    int z = 0;
                    do
                    {
                        Console.WriteLine("received connection..");
                        //Byte[] buffer = new Byte[4];
                        //serverStream.Read(buffer, 0, 4);
                        z = serverStream.ReadByte();
                        Console.WriteLine("client sent " + z);

                    }
                    while (z != 9);
                    serverStream.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }
    }
}
