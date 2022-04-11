using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace CopyPaste
{
    public static class CopyPaste
    {
        internal static void CP(string src_str, string dst_str)
        {
            DirectoryInfo src_info = new DirectoryInfo(src_str);
            DirectoryInfo dst_info = new DirectoryInfo(dst_str);

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

            // コピー先のフォルダ作成
            Directory.CreateDirectory(dst_str);

            // コピー元のファイルを全てコピー先のフォルダに上書きコピー
            string[] extensions = { ".mp3", ".wma", ".mp4", ".wav" };
            foreach (string file in Directory.EnumerateFiles(src_str, "*.*", SearchOption.AllDirectories).Where(s => extensions.Any(ext => ext == Path.GetExtension(s))))
            {
                //コピー先サブフォルダのパス取得+フォルダ作成(保存先パス\(ファイル作成年)\(ファイル作成月)\(ファイル作成日))
                DateTime dt = File.GetLastWriteTime(file);
                string subfolder = Path.Combine(dst_str, dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"));
                Directory.CreateDirectory(subfolder);

                //コピー先パス生成((サブフォルダパス)\(コピーするファイル名))
                FileInfo src_file = new FileInfo(file);
                string dst_path = Path.Combine(subfolder,src_file.Name);

                //コピー実行
                File.Copy(file ,dst_path, true);
            }

            //コピー後の成功
            /*    foreach()
                {

                }*/

            if (dst_info.Exists)
            {
                MessageBoxResult result2 = MessageBox.Show("バックアップが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
    }
}
