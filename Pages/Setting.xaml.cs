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
                IEnumerable<String> infos = from item in xml.Elements("Setting")
                                            where item.Attribute("Name").Value == "SavePath"
                                            select item.Element("Path").Value;

                TB1.Text = infos.ToString();
            } 
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("./Setting.xml") == false)
            {

                XElement datas = new XElement("Setting",
                                 new XElement("Name","SavePath"),
                                 new XElement("Path", ""));
                datas.Save("Setting.xml");
            }
            XElement xml = XElement.Load("Setting.xml");

            XElement info = (from item in xml.Elements("Setting")
                             where item.Element("Name").Value == "SavePath"
                             select item).Single();
            info.Element("Path").Value = TB1.Text;
        }

        private void B3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
