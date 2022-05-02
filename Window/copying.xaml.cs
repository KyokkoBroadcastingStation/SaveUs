using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
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
        string tb1, tb2;
        bool close;
        public copying(string a, string b)
        {
            InitializeComponent();
            tb1 = a;
            tb2 = b;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            L3.Content = "終了処理中";
            close = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            //CP(window.tb2,window.tb2);
            close = false;
            this.Show();
            CP(tb1,tb2);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        private async void CP(string src_str, string dst_str)
        {
            DirectoryInfo src_info = new DirectoryInfo(src_str);
            DirectoryInfo dst_info = new DirectoryInfo(dst_str);

            string[] extensions = { ".mp3", ".wma", ".mp4", ".MP4", ".wav" };

            // コピー元の存在チェック
            if (!src_info.Exists)
            {
                MessageBoxResult result = MessageBox.Show("バックアップ元として指定されたフォルダ及びファイルが見つかりません。再試行しますか？\nパス："+tb1+" → "+ tb2, "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    CP(src_str, dst_str);
                }
                else
                {
                    this.Hide();
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

            PB2.Maximum = length;
            PB2.Minimum = 0;

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
                L1.Content = fileinfo.Name;
                L2.Content = did + "/" + length;

                //コピー実行
                await Task.Run(() => { 
                    File.Copy(file, dst_path, true);
                });
                did++;
                L2.Content = did + "/" + length;
                PB2.Value++;

                if (close == true)
                    {
                        Close();
                        return;
                    }
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
                    Hide();
                }
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("書き込みエラーです。再試行しますか？", "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    CP(src_str, dst_str);
                }
                else
                {
                    Hide();
                }
                return;
            }
        }
    }
}
