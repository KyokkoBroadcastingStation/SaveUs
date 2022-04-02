using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace SaveMe
{
    internal static class BackUp
    {
        internal static void CP(string src_str, string dst_str)
        {
            DirectoryInfo src_info = new DirectoryInfo(src_str);
            DirectoryInfo dst_info = new DirectoryInfo(dst_str);

            // コピー元の存在チェック
            if (!src_info.Exists)
            {
                MessageBoxResult result = MessageBox.Show("ディレクトリが見つかりません。","エラー",MessageBoxButton.OK, MessageBoxImage.Error);
                if(result == MessageBoxResult.OK)
                {
                    CP(src_str, dst_str);
                }
                return;
            }

            // コピー先のフォルダ作成
            Directory.CreateDirectory(dst_str);

            // コピー元のフォルダとファイル情報を取得
            DirectoryInfo[] src_folders = src_info.GetDirectories();
            FileInfo[] src_files = src_info.GetFiles();

            // コピー元のファイルを全てコピー先のフォルダに上書きコピー
            foreach (FileInfo file in src_files)
            {
                string path = Path.Combine(dst_str, file.Name);
                file.CopyTo(path, true);
            }

            // 再起呼び出しによりコピー元のフォルダを全てコピー先のフォルダにコピー
            foreach (DirectoryInfo subfolder in src_folders)
            {
                string path = Path.Combine(dst_str, subfolder.Name);
                CP(subfolder.FullName, path);
            }

            //コピー後の成功
        /*    foreach()
            {

            }*/

            if(dst_info.Exists)
            {
                MessageBoxResult result2 = MessageBox.Show("バックアップが完了しました。", "完了", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("書き込みエラーです。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    CP(src_str, dst_str);
                }
                return;
            }
        }
    }
}
