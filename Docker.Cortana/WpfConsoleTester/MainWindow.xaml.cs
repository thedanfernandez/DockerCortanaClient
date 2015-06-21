using Docker.Cortana;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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


        private async void info_Click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerInfo();
            textBox1.Text += "Containers:" + result.Containers + Environment.NewLine;
        }

        private async void run_click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerRun("consoleascii");
            textBox1.Text += "new container id:" + result;

            //if (result.Warnings !=null)
            //{
            //    foreach (var item in result.Warnings)
            //    {
            //        textBox1.Text += item + Environment.NewLine;
            //    }

            //}


        }

        private async void getcontainers_Click(object sender, RoutedEventArgs e)
        {
            var result = await dc.DockerGetContainers();
            foreach (var item in result)
            {
                textBox1.Text += "Container:" + item.Id + ", Status:" + item.Status;
            }
        }

        private async void showLogs_Click(object sender, RoutedEventArgs e)
        {
            //use if you're following and want to stop following
            CancellationToken token = new CancellationToken(false);

            var containers = await dc.DockerGetContainers();

            //use ID from first container
            Stream s = await dc.ShowLogs(containers[0].Id, token);
            StreamReader sr = new StreamReader(s);

            string log = "";
            while (!sr.EndOfStream)
            {
                log = await sr.ReadToEndAsync();
                Debug.WriteLine(log);
            }

            textBox1.Text = log; 
            
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            dc = new DockerCortanaClient(txtHost.Text);
        }

        private void btnLogs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
