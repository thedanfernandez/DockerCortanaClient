using Docker.Cortana;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfConsoleTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DockerCortanaClient dc;


        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerInfo();
            textBox1.Text += "Containers:" + result.Containers + Environment.NewLine;
        }

        private async void run_click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerRun("redis");
            textBox1.Text = "new container id:" + result; 

        }

        private async void getcontainers_Click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerGetContainers();
            foreach (var item in result)
            {
                textBox1.Text += "Container:" + item.Id + ", Status:" + item.Status;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //var result = await dc.ShowLogs()
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            dc = new DockerCortanaClient(txtHost.Text);
        }
    }
}
