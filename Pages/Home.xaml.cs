using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace SaveMe.Pages
{
    /// <summary>
    /// Page1.xaml の相互作用ロジック
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            if (File.Exists("Setting.xml") == true)
            {
                XElement xml = XElement.Load("Setting.xml");
                XElement info = (from item in xml.Elements("Setting")
                                 where item.Element("Name").Value == "SavePath"
                                 select item).FirstOrDefault();
                DirectoryInfo di = new DirectoryInfo(info.Element("Path").Value);
                int fileCount = Directory.GetFiles(info.Element("Path").Value, "*", SearchOption.AllDirectories).Length;
                TB1.Text = "現在の素材の合計容量 ＝ " + GetDirectorySize(di) / 1000000 + "MB";
                TB2.Text = "現在の素材の合計数　 ＝ " + fileCount;
            }
            else if(File.Exists("Setting.xml") != true)
            {
                TB1.Text = "設定ファイルが存在しません。";
                TB2.Text = "設定タブから設定してください";
            }
        }
    public static long GetDirectorySize(DirectoryInfo dirInfo)
    {
        long size = 0;

        //フォルダ内の全ファイルの合計サイズを計算する
        foreach (FileInfo fi in dirInfo.GetFiles())
            size += fi.Length;

        //サブフォルダのサイズを合計していく
        foreach (DirectoryInfo di in dirInfo.GetDirectories())
            size += GetDirectorySize(di);

        return size;
    }
}
}
