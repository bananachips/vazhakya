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

namespace MafiaClient.Views
{
    /// <summary>
    /// Interaction logic for ClientLogin.xaml
    /// </summary>
    public partial class ClientLogin : UserControl
    {
        public ClientLogin()
        {
            InitializeComponent();
        }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !IsTextAllowed(e.Text);
    }

    private Boolean IsTextAllowed(String text)
    {
      return Array.TrueForAll<Char>(text.ToCharArray(),
          delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
    }
  }
}
