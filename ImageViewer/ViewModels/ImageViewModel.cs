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
	public class ImageViewModel : INotifyPropertyChanged
	{
		[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
		public static extern void CopyMemory(IntPtr Destination, IntPtr Source, uint Length);

		public event PropertyChangedEventHandler PropertyChanged;
		private BitmapSource _source;
		private byte[] _pixelData;
		private String _coord;
		public BitmapSource ImageSource
		{
			get { return _source; }
			set { SetField(ref _source, value, "ImageSource"); }
		}

		public String Coord
		{
			get { return _coord; }
			set { SetField(ref _coord, value, "Coord"); }
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
			LoadFromBufferRGB();

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
			_pixelData = new byte[width * height * 3];

			for (int y = 0; y < height; ++y)
			{
				int yIndex = y * width * 3;
				for (int x = 0; x < width; ++x)
				{
					int xIndex = x * 3;
					_pixelData[xIndex++ + yIndex] = 255;
					_pixelData[xIndex++ + yIndex] = 0;
					_pixelData[xIndex++ + yIndex] = 0;
				}
			}

			ImageSource = BitmapSource.Create(width, height, dpi, dpi,
					PixelFormats.Rgb24, null, _pixelData, width * 3);

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

		void ShowPixelValue(int col, int row)
		{
			
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

		bool SetField<T>(ref T field, T value, string propertyName)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			field = value;
			RaisePropertyChanged(propertyName);
			return true;
		}
	}
}
