using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;


namespace SaveMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //public string tb1;
        //public string tb2;
        public MainWindow()
        {
            InitializeComponent();

            var bgs = new Window.BackgroundSrc();
            bgs.Show();

            if (File.Exists("./Setting.xml") == false)
            {

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Pages/Setting.xaml", UriKind.Relative);
            contentFrame.Source = uri;
        }
    }
}
