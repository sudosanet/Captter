using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Restarter
{
    class Program
    {
        static void Copy(string fileName)
        {
            try
            {
                Console.WriteLine("copying..." + fileName);
                System.IO.File.Copy(fileName, System.Environment.CurrentDirectory);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("このアプリケーションはシステムの更新時に自動実行されます。手動で実行する必要はありませんが削除しないでください");
                Environment.Exit(1);
            }
            catch (System.IO.IOException)
            {
                if (System.Windows.Forms.MessageBox.Show(fileName + "のコピーに失敗しました。", "エラー",
                    System.Windows.Forms.MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Retry)
                {
                    System.Threading.Thread.Sleep(100);
                    Copy(fileName);
                }
                else
                {
                    Environment.Exit(2);
                }
            }
        }

        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10);
            string path = System.Environment.CurrentDirectory + "\\tmp\\";

            foreach(string fileName in Directory.GetFiles(path))
            {
                Copy(fileName);
            }
            System.IO.Directory.Delete(path,true);
            System.Windows.Forms.MessageBox.Show("アップデートが完了しました。");
            System.Diagnostics.Process.Start("captter3.exe");
            Environment.Exit(0);
        }
    }
}
