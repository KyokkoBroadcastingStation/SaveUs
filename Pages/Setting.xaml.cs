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
using MessageBox = System.Windows.Forms.MessageBox;

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
                    if(File.Exists(path)!=true)
                    {
                        StreamWriter sw = new StreamWriter(path);
                        System.Windows.MessageBox.Show("登録が完了しました。", "完了");
                    }
                    else if (File.Exists(path)==true)
                    {
                        System.Windows.MessageBox.Show("このSDカードは既に登録されています。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {  
            //Runキーを開く
            Microsoft.Win32.RegistryKey regkey =Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            //値の名前に製品名、値のデータに実行ファイルのパスを指定し、書き込む
            regkey.SetValue(System.Windows.Forms.Application.ProductName, System.Windows.Forms.Application.ExecutablePath);
            //閉じる
            regkey.Close();
            MessageBox.Show("スタートアップに登録されました。");
        }
    }
}
