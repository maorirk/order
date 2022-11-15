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
using GrpcClient;
using OrderMonitor.Interfaces;

namespace OrderMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow(IOrder orderMonitorViewModel)
        {
            InitializeComponent();
            this.DataContext = orderMonitorViewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //string reply = await GrpcClient.Class1.GrpcCallService();
            //this.t1.Text = reply;
        }
    }
}
