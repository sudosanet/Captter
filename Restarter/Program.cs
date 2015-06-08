using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restarter
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10);
            string path = System.Environment.CurrentDirectory + "\\tmp\\";
            string copy = "copying...";
            string ex = "captter3.exe";
            Console.WriteLine(copy+ex);
            try
            {
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
                Console.WriteLine(copy + ex);
                ex = "captter3.exe.config";
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
                Console.WriteLine(copy + ex);
                ex = "CoreTweet.dll";
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
                Console.WriteLine(copy + ex);
                ex = "CoreTweet.xml";
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
                Console.WriteLine(copy + ex);
                ex = "Newtonsoft.Json.dll";
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
                Console.WriteLine(copy + ex);
                ex = "Newtonsoft.Json.xml";
                System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + "\\" + ex, true);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("このアプリケーションはシステムの更新時に自動実行されます。手動で実行する必要はありませんが削除しないでください");
                Environment.Exit(0);
            }
            catch(System.IO.IOException)
            {
                System.Threading.Thread.Sleep(100);
                return;
            }
            System.IO.Directory.Delete(System.Environment.CurrentDirectory + "\\tmp",true);
            System.Windows.Forms.MessageBox.Show("アップデートが完了しました。");
            System.Diagnostics.Process.Start("captter3.exe");
            Environment.Exit(0);
        }
    }
}
