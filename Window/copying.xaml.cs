using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SaveMe.Window
{
    /// <summary>
    /// copying.xaml の相互作用ロジック
    /// </summary>
    public partial class copying
    {
        public copying()
        {
            var window = new MainWindow();
            //CP(window.tb2,window.tb2);
            CP();
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void CP()
        {
            var window = new MainWindow();
            string src_str = window.tb1;
            string dst_str = window.tb2;
            DirectoryInfo src_info = new DirectoryInfo(src_str);
            DirectoryInfo dst_info = new DirectoryInfo(dst_str);

            string[] extensions = { ".mp3", ".wma", ".mp4", ".MP4", ".wav" };

            // コピー元の存在チェック
            if (!src_info.Exists)
            {
                MessageBoxResult result = MessageBox.Show("バックアップ元として指定されたフォルダ及びファイルが見つかりません。再試行しますか？\nパス："+window.tb1+" → "+ window.tb2, "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    CP(/*src_str, dst_str*/);
                }
                else
                {
                    this.Close();
                }
                return;
            }

            //表示関係
            int length = 0;
            int did = 0;
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == System.IO.Path.GetExtension(s))))
            {
                length++;
            }
            SaveMe.Window.copying cping = new SaveMe.Window.copying();
            cping.PB2.Maximum = length;
            cping.PB2.Minimum = 0;

            // コピー先のフォルダ作成
            Directory.CreateDirectory(dst_str);

            // コピー元のファイルを全てコピー先のフォルダに上書きコピー
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == System.IO.Path.GetExtension(s))))
            {
                //コピー先サブフォルダのパス取得+フォルダ作成(保存先パス\(ファイル作成年)\(ファイル作成月)\(ファイル作成日))
                DateTime dt = File.GetLastWriteTime(file);
                string subfolder = System.IO.Path.Combine(dst_str, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                Directory.CreateDirectory(subfolder);

                //コピー先パス生成((サブフォルダパス)\(コピーするファイル名))
                FileInfo src_file = new FileInfo(file);
                string dst_path = System.IO.Path.Combine(subfolder, src_file.Name);

                FileInfo fileinfo = new FileInfo(file);
                FileInfo dst_fileinfo = new FileInfo(dst_path);
                cping.L1.Content = fileinfo.Name;
                cping.L2.Content = did + "/" + length;

                //コピー実行
                await Task.Run(() => File.Copy(file, dst_path, true));
                did++;
                cping.L2.Content = did + "/" + length;
                cping.PB2.Value++;
            }

            //コピー後の成功
            /*    foreach()
                {

                }*/

            if (dst_info.Exists)
            {
                MessageBoxResult result2 = MessageBox.Show("バックアップが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                if (result2 == MessageBoxResult.OK)
                {
                    cping.Hide();
                }
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("書き込みエラーです。再試行しますか？", "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    CP(/*src_str, dst_str*/);
                }
                else
                {
                    this.Close();
                }
                return;
            }
        }
    }
}
