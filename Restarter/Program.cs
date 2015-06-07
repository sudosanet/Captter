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
            Console.WriteLine("ファイルをコピーします。何かのキーを押してください");
            Console.ReadKey();
            string path = System.Environment.CurrentDirectory + "\\tmp\\";
            string copy = "copying...";
            string ex = "captter3.exe";
            Console.WriteLine(copy+ex);
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            Console.WriteLine(copy + ex);
            ex = "captter3.exe.config";
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            Console.WriteLine(copy + ex);
            ex = "CoreTweet.dll";
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            Console.WriteLine(copy + ex);
            ex = "CoreTweet.xml";
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            Console.WriteLine(copy + ex);
            ex = "Newtonsoft.Json.dll";
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            Console.WriteLine(copy + ex);
            ex = "Newtonsoft.Json.xml";
            System.IO.File.Copy(path + ex, System.Environment.CurrentDirectory + ex, true);
            System.IO.File.Delete(System.Environment.CurrentDirectory + "\\tmp");
            System.Diagnostics.Process.Start("captter3.exe");
            Environment.Exit(0);
        }
    }
}
