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

        public updater(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }

        System.Net.WebClient downloadClient = null;
        private string p;
        public void download()
        {
            //ダウンロードしたファイルの保存先
            string fileName = System.Reflection.Assembly.GetExecutingAssembly().Location + "/tmp/update.zip";
            //ダウンロード基のURL
            Uri u = new Uri(p);

            //WebClientの作成
            if (downloadClient == null)
            {
                downloadClient = new System.Net.WebClient();
                //イベントハンドラの作成
                downloadClient.DownloadProgressChanged +=
                    new System.Net.DownloadProgressChangedEventHandler(
                        downloadClient_DownloadProgressChanged);
                downloadClient.DownloadFileCompleted +=
                    new System.ComponentModel.AsyncCompletedEventHandler(
                        downloadClient_DownloadFileCompleted);
            }
            //非同期ダウンロードを開始する
            downloadClient.DownloadFileAsync(u, fileName);
        }
        private void downloadClient_DownloadProgressChanged(object sender,
        System.Net.DownloadProgressChangedEventArgs e)
        {
            //Console.WriteLine("{0}% ({1}byte 中 {2}byte) ダウンロードが終了しました。",
                //e.ProgressPercentage, e.TotalBytesToReceive, e.BytesReceived);
            downloading.Content = e.ProgressPercentage;
        }

        private void downloadClient_DownloadFileCompleted(object sender,
            System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine("キャンセルされました。");
            else if (e.Error != null)
                Console.WriteLine("エラー:{0}", e.Error.Message);
            else
                Console.WriteLine("ダウンロードが完了しました。");
        }
    }
}
