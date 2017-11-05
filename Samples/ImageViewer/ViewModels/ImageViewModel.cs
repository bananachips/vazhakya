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

namespace ImageViewer.ViewModels
{

    //Notes: The image source to the image in xaml can be a bitmapImage or bitmapSource
    public class ImageViewModel: INotifyPropertyChanged
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

        public event PropertyChangedEventHandler PropertyChanged;
        private BitmapSource _source;
        private int _red = 255;
        private int _green;
        private int _blue;
        private int _alpha = 255;
        public BitmapSource ImageSource
        {
            get { return _source; }
            set {SetField(ref _source, value, "ImageSource");}
        }
        public int Red
        {
            get { return _red; }
            set { SetField(ref _red, value, "Red") ; }
        }

        public int Green
        {
            get { return _green; }
            set { SetField(ref _green, value, "Green"); }
        }

        public int Blue
        {
            get { return _blue; }
            set { SetField(ref _blue, value, "Blue"); }
        }

        public int Alpha
        {
            get { return _alpha; }
            set { SetField(ref _alpha, value, "Alpha"); }
        }


        private BitmapImage _bmpImage;
        public BitmapImage BMPImage
        {
            get { return _bmpImage; }
            set
            {
                if (_bmpImage != value)
                {
                    _bmpImage = value;
                    RaisePropertyChanged("BMPImage");

                }
            }
        }
        public ImageViewModel()
        {
            LoadFromBufferRGBA64();
            
        }


        public void LoadFromBuffer()
        {
            double dpi = 96;
            int width = 128;
            int height = 128;
            byte[] pixelData = new byte[width * height];

            for (int y = 0; y < height; ++y)
            {
                int yIndex = y * width;
                for (int x = 0; x < width; ++x)
                {
                    pixelData[x + yIndex] = (byte)(x + y);
                }
            }

            ImageSource = BitmapSource.Create(width, height, dpi, dpi,
                PixelFormats.Gray8, null, pixelData, width);
            
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

            ImageSource = BitmapSource.Create(width * 2, height*2, dpi, dpi,
                PixelFormats.Rgb24, null, pixelData, width*3);

        }

        public void LoadFromBufferRGBA()
        {
            double dpi = 96;
            int width = 128;
            int height = 128;
            byte[] pixelData = new byte[width * height * 4];

            for (int y = 0; y < height; ++y)
            {
                int yIndex = y * width * 4;
                for (int x = 0; x < width; ++x)
                {
                    int xIndex = x * 4;
                    pixelData[xIndex++ + yIndex] = (byte)_blue;
                    pixelData[xIndex++ + yIndex] = (byte)_green;
                    pixelData[xIndex++ + yIndex] = (byte)_red;
                    pixelData[xIndex++ + yIndex] = (byte)_alpha;
                }
            }

            ImageSource = BitmapSource.Create(width, height, dpi, dpi,
                PixelFormats.Pbgra32, null, pixelData, width * 4);
            
        }

        public void LoadFromBufferRGBA64()
        {
            double dpi = 96;
            int width = 128;
            int height = 128;
            Int16[] pixelData = new Int16[width * height * 4];
            for (int y = 0; y < height; ++y)
            {
                int yIndex = y * width * 4;
                for (int x = 0; x < width; ++x)
                {
                    int xIndex = x * 4;
                    pixelData[xIndex++ + yIndex] = (Int16)(_blue *2);
                    pixelData[xIndex++ + yIndex] = (Int16)(_green*2);
                    pixelData[xIndex++ + yIndex] = (Int16)(_red*2);
                    pixelData[xIndex++ + yIndex] = (Int16)(_alpha*2);
                }
            }

            ImageSource = BitmapSource.Create(width, height, dpi, dpi,
                PixelFormats.Prgba64, null, pixelData, width * 4*2);
        }
        public void LoadFromFile()
        {
            byte[] bufferx = File.ReadAllBytes(@"c:\work\data\Amberg_tif\090160.tif");
            MemoryStream memoryStream = new MemoryStream(bufferx);
            BitmapImage bitmap = new BitmapImage();
            try
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.BeginInit();
                bitmap.DecodePixelWidth = 200;
                bitmap.DecodePixelHeight = 200;
                bitmap.StreamSource = memoryStream;
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = null;
                bitmap.EndInit();
                bitmap.Freeze();
                ImageSource = bitmap;

            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        public static BitmapSource FromNativePointer(IntPtr pData, int w, int h, int ch)
        {
            PixelFormat format = PixelFormats.Default;

            if (ch == 1) format = PixelFormats.Gray8; //grey scale image 0-255
            if (ch == 3) format = PixelFormats.Bgr24; //RGB
            if (ch == 4) format = PixelFormats.Bgr32; //RGB + alpha


            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, format, null);
            CopyMemory(wbm.BackBuffer, pData, (uint)(w * h * ch));

            wbm.Lock();
            //wbm.AddDirtyRect(new Int32Rect(0, 0, wbm.PixelWidth, wbm.PixelHeight));
            wbm.Unlock();

            return wbm;
        }

        void RandomBitmap()
        {
            int lineSize = 2;
            int previewWidth = 200;
            int previewHeight = 200;

            Bitmap bmpPreview = new Bitmap(previewWidth + lineSize * 2, previewHeight + lineSize * 2, 
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
    
            //PixelFormat pf = PixelFormats.Bgr32;
            //int width = 200;
            //int height = 200;
            //int rawStride = (width * pf.BitsPerPixel + 7) / 8;
            //byte[] rawImage = new byte[rawStride * height];

            //// Initialize the image with data.
            //Random value = new Random();
            //value.NextBytes(rawImage);

            // Create a BitmapSource.
            //BitmapSource bitmap = BitmapSource.Create(width, height,
            //     96, 96, pf, null,
            //    rawImage, rawStride);
           // ImageSource.CopyPixels(bmpPreview.)
            //ImageSource = bmpPreview;
        }

        private void RaisePropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }


        internal void Update()
        {
            LoadFromBufferRGBA64();
        }
    }
}
