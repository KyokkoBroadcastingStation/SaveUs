using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SaveMe.Window
{
    public partial class BackgroundSrc : Form
    {   
        public BackgroundSrc()
        {
            InitializeComponent();
        }
        private void BackgroundSrc_Load(object sender, EventArgs e)
        {

        }
        private const int WM_DEVICECHANGE = 0x219;              // デバイスまたはコンピューターのハードウェア構成への変更
        private const int DBT_DEVICEARRIVAL = 0x8000;           // USBの挿入
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;    // USBの取り外し
        private const int DBT_DEVTYP_VOLUME = 0x00000002;       // デバイスの種類がボリューム

        // 論理ボリュームに関する情報の構造体
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

        private static string GetDriveLetter(int dnum)
        {
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int pos = ((int)Math.Log2(dnum));//指数

            string letter = drives.Substring(pos, 1) + ":";

            return letter;
        }

        protected override void WndProc(ref Message m)
        {
            // 
            if (m.Msg == WM_DEVICECHANGE)
            {
                if (m.WParam.ToInt64() == DBT_DEVICEARRIVAL)
                {
                    // USB等の挿入
                    int devType = Marshal.ReadInt32(m.LParam, 4);
                    if (devType == DBT_DEVTYP_VOLUME)           // デバイスの種類がボリュームか？
                    {
                        // ボリュームの情報を取得
                        DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                        // ドライブレターの取得
                        string dLetter = GetDriveLetter(vol.dbcv_unitmask);

                        //処理
                        string path = Path.Combine(dLetter + "\\set.ini");
                        if(File.Exists(path) == true)
                        {
                            DialogResult result = MessageBox.Show("登録されたSDカードが検出されました。バックアップしますか？", "SaveMe", MessageBoxButtons.YesNo);
                            
                            if(result == DialogResult.Yes)
                            {
                                XElement xml = XElement.Load("Setting.xml");
                                XElement info = (from item in xml.Elements("Setting")
                                                 where item.Element("Name").Value == "SavePath"
                                                 select item).FirstOrDefault();

                                string xml_src = info.Element("Path").Value;

                                string xml_path = Path.Combine(xml_src);

                                string src_path = Path.Combine(dLetter);
                                var window = new SaveMe.Window.copying(src_path, xml_path);
                                window.Show();
                            }
                            //FileStream fs = File.Create(path);
                        }
                        

                    }
                }
                else if (m.WParam.ToInt64() == DBT_DEVICEREMOVECOMPLETE)
                {
                    // USB等デバイス取り外し完了
                }
            }

            base.WndProc(ref m);
        }

        private void BackgroundSrc_Activated(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BackgroundSrc_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
