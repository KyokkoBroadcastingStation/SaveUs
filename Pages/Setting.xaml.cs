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
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace SaveMe.Pages
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class Setting : Page
    {
        public Setting()
        {
            InitializeComponent();
            if(File.Exists("Setting.xml")==true)
            {
                XElement xml = XElement.Load("Setting.xml");
                XElement info = (from item in xml.Elements("Setting")
                                 where item.Element("Name").Value == "SavePath"
                                 select item).FirstOrDefault();
                TB1.Text = info.Element("Path").Value;
            } 
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(TB1.Text);
            if(Directory.Exists(path) != true)
            {
                System.Windows.MessageBox.Show("パスが入力されていないか、存在しません","エラー", MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (File.Exists("./Setting.xml") == false)
            {

                XElement datas = new XElement("Root",
                                 new XElement("Setting",
                                 new XElement("Name","SavePath"),
                                 new XElement("Path", "")));
                datas.Save("Setting.xml");
            }
            XElement xml = XElement.Load("Setting.xml");

            XElement info = (from item in xml.Elements("Setting")
                             where item.Element("Name").Value == "SavePath"
                             select item).FirstOrDefault();

            info.Element("Path").Value = TB1.Text;
            xml.Save("Setting.xml");
            System.Windows.MessageBox.Show("設定の保存が完了しました。");
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "登録するSDカードを選択してください。";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(fbd.SelectedPath) != true)
                {
                    System.Windows.MessageBox.Show("パスが入力されていないか、存在しません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if(Directory.Exists(fbd.SelectedPath) == true)
                {
                    string root_pass = System.IO.Path.GetPathRoot(fbd.SelectedPath);
                    string path = System.IO.Path.Combine(fbd.SelectedPath + "set.ini");
                    StreamWriter sw = new StreamWriter(path);
                    System.Windows.MessageBox.Show("登録が完了しました。", "完了");
                }

            }
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.ShowNewFolderButton = true;
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                TB1.Text = fbd.SelectedPath;
            }
        }
    }
}
