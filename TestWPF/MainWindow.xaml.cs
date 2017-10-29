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
using TestWPF.ViewModels;

namespace TestWPF
{
    public class PlayerInfo
    {
        public PlayerInfo()
        {
            Name = "";
            IP = "localhost";
            Port = "8000";
            something = 2;
        }
        public string Name{get; set;}
        public string IP{get; set;}
        string Port{get; set;}
        int something { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            List<PlayerInfo> items = new List<PlayerInfo>();
            items.Add(new PlayerInfo() { Name = "Anjali" });
            items.Add(new PlayerInfo() { Name = "Sachin" });
            items.Add(new PlayerInfo() { Name = "Meera" });
           // List<String> items = new List<String>() { "asad", "ASdas" };



                        //PlayerListBox.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new BlueVM();
        }
    }
}
