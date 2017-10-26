using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using System.Web.Script.Serialization;
using System.Windows.Threading;
using System.Threading;

namespace NamedPipeImageServer.ViewModels
{
    public class ImageInfo
    {
        public UInt32 width;
        public UInt32 height;
        
    }
    //Notes: The image source to the image in xaml can be a bitmapImage or bitmapSource
    public class ImageViewModel: INotifyPropertyChanged
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

        public event PropertyChangedEventHandler PropertyChanged;
        NamedPipeServerStream _serverStream;
        private BitmapSource _source;
        private int _height;
        private int _width;
        private int _channels;
        public BitmapSource ImageSource
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged("ImageSource");

                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged("Width");

                }
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RaisePropertyChanged("Height");

                }
            }
        }

        public ImageViewModel()
        {
            //Initialize Pipe Server
            Initialize();
            //Wait for connnections
            // Read image sent by client
            // Display Image
           // LoadFromBufferRGB();
            
        }
        bool ReadImageInfo()
        {
            
            Byte[] buffer = new Byte[sizeof(int)];
            int retVal = _serverStream.Read(buffer, 0, 4);
            if (retVal <= 0)
                return false;
            Width = BitConverter.ToInt32(buffer, 0);
            retVal = _serverStream.Read(buffer, 0, 4);
            if (retVal <= 0)
                return false;
            Height = BitConverter.ToInt32(buffer, 0);
            retVal = _serverStream.Read(buffer, 0, 4);
            if (retVal <= 0)
                return false;
            _channels = BitConverter.ToInt32(buffer, 0);
            return true;
        }

        BitmapSource ReadImage()
        {
            
            Byte[] buffer = new Byte[_height*_width*_channels];
            int retVal = _serverStream.Read(buffer, 0, _height * _width * _channels);
            if (retVal <= 0)
                return null;
            BitmapSource source = BitmapSource.Create(_width, _height, 96, 96,
                PixelFormats.Rgb24, null, buffer, _width * _channels);
            source.Freeze();
            //Thread.Sleep(2000);
            return source;
            
        }

        async void ListenForData()
        {
            while (_serverStream.IsConnected)
            {
                BitmapSource s = await ListenAsync();
								if (s != null)
									ImageSource = s;
            }
            //LoadFromBuffer();
        }

        Task<BitmapSource> ListenAsync()
        {
            return Task<BitmapSource>.Factory.StartNew(()=>
            {
                if (_serverStream.IsConnected)
                {
                    if (ReadImageInfo())
											return ReadImage();
										return null;
                }
                else
                    return null;
            });
        }

        async void WaitForConnections()
        {
            await WaitForConnectionsAsync();
        }

        Task WaitForConnectionsAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                do
                {
                    try
                    {
                        if (!_serverStream.IsConnected)
                        {

                            _serverStream.WaitForConnection();
                            ListenForData();

                        }
                        else
                        {
                            Thread.Sleep(1000);

                        }
                    }
                    catch(Exception ex)
                    {
                        _serverStream.Disconnect();
                        
                    }
                } while (true);
            });
        }

        public void Initialize()
        {
            _serverStream = new NamedPipeServerStream("AnksImageViewer", PipeDirection.In);
            //LoadFromBuffer();
            if (_serverStream != null)
            {
                WaitForConnections();
               
            }
            //_serverStream.WaitForConnection();
            //ReadImageInfo();
            //ReadImage();
            //var jss = new JavaScriptSerializer();
            //jss.MaxJsonLength = Int32.MaxValue;

            //StringBuilder messageBuilder = new StringBuilder();
            //string messageChunk = string.Empty;
            //byte[] messageBuffer = new byte[5];
            //do
            //{
            //    serverStream.Read(messageBuffer, 0, messageBuffer.Length);
            //    messageChunk = Encoding.UTF8.GetString(messageBuffer);
            //    messageBuilder.Append(messageChunk);
            //    messageBuffer = new byte[messageBuffer.Length];
            //}
            //while (!serverStream.IsMessageComplete);
            //ImageInfo imageInfo = jss.Deserialize<ImageInfo>(messageChunk);

            //serverStream.Close();

        }
        public void LoadFromBuffer()
        {
            double dpi = 96;
            Width = 128;
            Height = 128;
            byte[] pixelData = new byte[_width * _height];

            for (int y = 0; y < _height; ++y)
            {
                int yIndex = y * _width;
                for (int x = 0; x < _width; ++x)
                {
                    pixelData[x + yIndex] = (byte)(x + y);
                }
            }

            ImageSource = BitmapSource.Create(_width, _height, dpi, dpi,
                PixelFormats.Gray8, null, pixelData, _width);
            
        }

        public void LoadFromBufferRGB()
        {
            double dpi = 96;
            int width = 128;
            int height = 128;
            byte[] pixelData = new byte[width * height*3];

            for (int y = 0; y < height; ++y)
            {
                int yIndex = y * width * 3;
                for (int x = 0; x < width; ++x)
                {
                    int xIndex = x * 3;
                    pixelData[xIndex++ + yIndex] = 255;
                    pixelData[xIndex++ + yIndex] = 0;
                    pixelData[xIndex++ + yIndex] = 0;
                }
            }

            ImageSource = BitmapSource.Create(width, height, dpi, dpi,
                PixelFormats.Rgb24, null, pixelData, width*3);

        }
        
        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        
    }
}
