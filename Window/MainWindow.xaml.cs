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
using SaveMe.Properties;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace SaveMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //public string tb1;
        //public string tb2;
        NotifyIcon notifyIcon = new NotifyIcon
        {
            Icon = SaveMe.Properties.Resources.trayicon,
            Text = "SaveUs\nダブルクリックで表示を切り替えることが出来ます。",
        };
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();

        int pattern;
        public MainWindow()
        {
            InitializeComponent();
            var bgs = new Window.BackgroundSrc();
            bgs.Show();
            // NotifyIconのクリックイベント
            notifyIcon.MouseDoubleClick += NotifyIcon_Click;
            //コンテキ
            toolStripMenuItem.Text = "&終了";
            toolStripMenuItem.Click += ToolStripMenuItem_Click;
            contextMenuStrip.Items.Add(toolStripMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            //ウィンドウのドラック
            TitleBar.MouseLeftButtonDown += (o, e) => DragMove();
        }
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            // Formの表示/非表示を反転

            switch (pattern)
            {
                case 0:
                    this.ShowInTaskbar = false;
                    this.WindowState = System.Windows.WindowState.Minimized;
                    pattern = 1;
                    break;
                case 1:
                    this.ShowInTaskbar = true;
                    this.WindowState = System.Windows.WindowState.Normal;
                    pattern = 0;
                    break;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = true;
            // Create the interop host control.
            System.Windows.Forms.Integration.WindowsFormsHost host =
                new System.Windows.Forms.Integration.WindowsFormsHost();
            this.Activate();
            Uri uri = new Uri("/Pages/Home.xaml", UriKind.Relative);
            contentFrame.Source = uri;
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Pages/Setting.xaml", UriKind.Relative);
            contentFrame.Source = uri;
        }

        private void Border_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            
        }

        private void Border_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            XElement xml = XElement.Load("Setting.xml");
            XElement info = (from item in xml.Elements("Setting")
                             where item.Element("Name").Value == "SavePath"
                             select item).FirstOrDefault();

            string exit_op = info.Element("Exit").Value;
            if(exit_op == "true")
            {
                switch (pattern)
                {
                    case 0:
                        this.ShowInTaskbar = false;
                        this.WindowState = System.Windows.WindowState.Minimized;
                        pattern = 1;
                        break;
                    case 1:
                        this.ShowInTaskbar = true;
                        this.WindowState = System.Windows.WindowState.Normal;
                        pattern = 0;
                        break;
                }
            }
            else if(exit_op == "false")
            {
                this.Close();
            }
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/Pages/Home.xaml", UriKind.Relative);
            contentFrame.Source = uri;
        }
    }
}
