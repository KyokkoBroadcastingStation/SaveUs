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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

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

            string[] extensions = { ".mp3",".MP3",".wma",".WMA",".mp4",".MP4",".wav",".WAV",".txt",".TXT",".mov",".MOV",".mkv",".MKV",".avi",".AVI",".mts",".MTS",".png",".PNG","jpg","JPG", };
            int suc = 0;
            // コピー元の存在チェック
            if (!src_info.Exists)
            {
                MessageBoxResult result = MessageBox.Show("コピー元として指定されたフォルダ及びファイルが見つかりません。再試行しますか？\nパス："+tb1+" → "+ tb2, "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
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
            string gr_name = null;
            genre gr = new genre(dst_str);
            System.Windows.Forms.DialogResult dr = gr.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                gr_name = gr._genre;
            }
            else if (dr == System.Windows.Forms.DialogResult.Cancel)
            {
                MessageBox.Show("キャンセルされました。処理を中断します。","SaveUs");
                return;
            }
            // コピー元のファイルを全てコピー先のフォルダに上書きコピー
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == System.IO.Path.GetExtension(s))))
            {
                //コピー先サブフォルダのパス取得+フォルダ作成(保存先パス\(ファイル作成年)\(ファイル作成月)\(ファイル作成日))
                DateTime dt = File.GetLastWriteTime(file);
                string subfolder = System.IO.Path.Combine(dst_str, gr_name ,dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
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

            //コピー後の成功判定、削除
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == System.IO.Path.GetExtension(s))))
            {
                DateTime dt = File.GetLastWriteTime(file);
                string subfolder = System.IO.Path.Combine(dst_str, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                Directory.CreateDirectory(subfolder);

                //コピー先パス生成((サブフォルダパス)\(コピーするファイル名))
                FileInfo src_file = new FileInfo(file);
                string dst_path = System.IO.Path.Combine(subfolder, src_file.Name);

                FileInfo fileinfo = new FileInfo(file);
                FileInfo dst_fileinfo = new FileInfo(dst_path);

                if(fileinfo.Name == dst_fileinfo.Name)
                {
                    //成功回数
                    suc++;
                }
                else if(fileinfo.Name == dst_fileinfo.Name)
                {
                    System.Windows.MessageBox.Show("コピーが失敗したかキャンセルされました。");
                    return;
                }
                
            }
            MessageBoxResult result3 = MessageBox.Show(suc + "個のファイルのコピーに成功しました。コピー元のファイルを削除しますか？","SaveUs",MessageBoxButton.YesNo);
            if (result3 == MessageBoxResult.Yes)
            {
                foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == System.IO.Path.GetExtension(s))))
                {

                    DateTime dt = File.GetLastWriteTime(file);
                    string subfolder = System.IO.Path.Combine(dst_str, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                    Directory.CreateDirectory(subfolder);

                    //コピー先パス生成((サブフォルダパス)\(コピーするファイル名))
                    FileInfo src_file = new FileInfo(file);
                    string dst_path = System.IO.Path.Combine(subfolder, src_file.Name);

                    FileInfo fileinfo = new FileInfo(file);
                    FileInfo dst_fileinfo = new FileInfo(dst_path);
                    fileinfo.Delete();
                }
            }

            if (dst_info.Exists)
            {
                MessageBoxResult result2 = MessageBox.Show("コピーが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
