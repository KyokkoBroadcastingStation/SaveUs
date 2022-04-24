using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Diagnostics;

namespace CopyPaste
{
    public static class CopyPaste
    {
        async internal static void CP(string src_str, string dst_str)
        {
            DirectoryInfo src_info = new DirectoryInfo(src_str);
            DirectoryInfo dst_info = new DirectoryInfo(dst_str);

            string[] extensions = { ".mp3", ".wma", ".mp4" ,".MP4" ,".wav" };

            // コピー元の存在チェック
            if (!src_info.Exists)
            {
                MessageBoxResult result = MessageBox.Show("バックアップ元として指定されたフォルダ及びファイルが見つかりません。再試行しますか？", "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    CP(src_str, dst_str);
                }
                return;
            }

            //表示関係
            int length = 0;
            int did = 0;
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == Path.GetExtension(s))))
            {
                length++;
            }
            SaveMe.Window.copying cping = new SaveMe.Window.copying();
            cping.PB2.Maximum = length;
            cping.PB2.Minimum = 0;

            cping.Show();


            // コピー先のフォルダ作成
            Directory.CreateDirectory(dst_str);

            // コピー元のファイルを全てコピー先のフォルダに上書きコピー
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == Path.GetExtension(s))))
            {
                //コピー先サブフォルダのパス取得+フォルダ作成(保存先パス\(ファイル作成年)\(ファイル作成月)\(ファイル作成日))
                DateTime dt = File.GetLastWriteTime(file);
                string subfolder = Path.Combine(dst_str, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                Directory.CreateDirectory(subfolder);

                //コピー先パス生成((サブフォルダパス)\(コピーするファイル名))
                FileInfo src_file = new FileInfo(file);
                string dst_path = Path.Combine(subfolder, src_file.Name);

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
                if(result2 == MessageBoxResult.OK)
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
                    CP(src_str, dst_str);
                }
                return;
            }
        }


        /*static void CpShow(string befpass , string aftpass, double size ,int did , int length)
        {
            SaveMe.Window.copying cping = new SaveMe.Window.copying();
            FileInfo fileinfo = new FileInfo(befpass);
            cping.L1.Content = fileinfo.Name;
            cping.L2.Content = did + "/" + length;
            cping.PB2.Value++;

            double per = Math.Round(fileinfo.Length / size)*100;
            cping.PB1.Value = per;
            cping.L4.Content = per + "%";

        }*/
    }
}
