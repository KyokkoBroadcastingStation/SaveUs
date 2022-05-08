using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = Properties.Resources.trayicon,
                Text = "タスクトレイ常駐アプリのテストです"
            };
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

            int pos = ((int)Math.Sqrt(dnum / 2) - 1);

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
    }
}
