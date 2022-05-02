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
using System.Diagnostics;


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
        }
        
        private void Bt1_Click(object sender, RoutedEventArgs e)
        {
            var window = new SaveMe.Window.copying(TB1.Text,TB2.Text);
            window.Show();
            //window.ShowDialog();
            //CopyPaste.CopyPaste.CP(TB1.Text,TB2.Text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
