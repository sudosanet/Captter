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

        private string _url;
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }
        public void download()
        {

            //ダウンロードしたファイルの保存先
            string fileName = System.Environment.CurrentDirectory + "\\tmp\\update.zip";
            Console.WriteLine(url);
            Console.WriteLine(fileName);
            System.Net.WebClient down = new System.Net.WebClient();
            down.DownloadFile(url, fileName);
            down.Dispose();
        }
    }
}
