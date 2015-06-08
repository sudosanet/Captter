using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;

namespace captter3
{
    /// <summary>
    /// updater.xaml の相互作用ロジック
    /// </summary>
    public partial class updater : Window
    {
        public updater()
        {
            InitializeComponent();
        }

        private string _url;
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }
        
        System.Net.WebClient down = null;
        string uppath = System.Environment.CurrentDirectory + "\\tmp\\update.zip";
        string path = System.Environment.CurrentDirectory + "\\tmp";
        public void download()
        {
            Uri uri = new Uri(url);
            
            if (System.IO.File.Exists(path)){ }
            else
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
                di.Create();
            }
            if (down == null)
            {
                down = new System.Net.WebClient();
                down.DownloadProgressChanged +=
                    new System.Net.DownloadProgressChangedEventHandler(sinchoku);
                down.DownloadFileCompleted +=
                    new System.ComponentModel.AsyncCompletedEventHandler(comp);
            }
            down.DownloadFileAsync(uri, uppath);
        }
        private void sinchoku(object sender,
        System.Net.DownloadProgressChangedEventArgs e)
        {
            downloading.Visibility = Visibility.Visible;
            pb.Value = e.ProgressPercentage;
            downloading.Content = "ファイルをダウンロードしています("+e.ProgressPercentage+"%)・・・";
        }

        private void comp(object sender,
            System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("キャンセルされました。");
                this.Close();
            }
            else if (e.Error != null)
            {
                MessageBox.Show("エラー:{0}", e.Error.Message);
                this.Close();
            }
            else
            {
                Console.WriteLine("ダウンロードが完了しました。");
                zip();
            }

        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            if (down != null)
                down.CancelAsync();
        }
        private void zip()
        {
            ext.Visibility = Visibility.Visible;
            ZipFile.ExtractToDirectory(uppath, path);
            System.IO.File.Delete(uppath);

            System.Diagnostics.Process.Start("Restarter.exe");
            Environment.Exit(0);
        }
    }
}
