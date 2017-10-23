using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageViewer.Views
{
	/// <summary>
	/// Interaction logic for ImageControl.xaml
	/// </summary>
	public partial class ImageControl : UserControl
	{
		public ImageControl()
		{
			InitializeComponent();
		}

		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			var imageControl = this.ImageX;
			var coord = Mouse.GetPosition(this.ImageX);
			var dc = this.DataContext as ImageViewer.ViewModels.ImageViewModel;
			
			var x = Math.Floor(coord.X * imageControl.Source.Width / imageControl.ActualWidth);
			var y = Math.Floor(coord.Y * imageControl.Source.Height / imageControl.ActualHeight);
			if (dc != null)
				dc.Coord = String.Format("{0}:{1}", x, y);
		}
	}
}
