using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoreTweet;

namespace captter3
{
    /// <summary>
    /// Window2.xaml の相互作用ロジック
    /// </summary>
    public partial class Window2 : Window
    {
        //coreTweet
        public CoreTweet.OAuth.OAuthSession session;
        public CoreTweet.Tokens token;
        public const string CK = "bTLiIE3LOyadvojbBf5AEXIsU";
        public const string CS = "ztjWDrWJ9YOsfPKMRw9Q0uqDywEjmmCKSIIK6eoXP81tWYe9H6";
        public string code;

        public Window2()
        {
            InitializeComponent();
        }
        //閉じるボタン非表示にするためWin32APIを使う
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //Windowsロード時に閉じるボタンを非表示にする
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //token発行
            session = OAuth.Authorize(CK, CS);
            string url = session.AuthorizeUri.ToString();
            System.Diagnostics.Process.Start(url);
            pin.IsEnabled = true;
        }

        private void pin_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ボタン有効化
            code = pin.Text;
            if (code.Length == 7)
            {
                try
                {
                    int n = int.Parse(code);
                    if (auth.IsEnabled == false)
                    {
                        auth.IsEnabled = true;
                    }
                }
                catch(System.FormatException)
                {

                }

            }
            else
            {
                if (auth.IsEnabled == true)
                {
                    auth.IsEnabled = false;
                }
            }
        }

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            //認証
            code = pin.Text;
            try
            {
                token = session.GetTokens(code);
            }
            catch(CoreTweet.TwitterException)
            {
                MessageBox.Show("エラーが発生しました、もう一度やり直してください","info");
            }
            Properties.Settings.Default.AccessToken = token.AccessToken;
            Properties.Settings.Default.TokenSecret = token.AccessTokenSecret;
            //debug
            //dev.Content = "debug情報をツイート中";
            ah.IsEnabled = false;
            pin.IsEnabled = false;
            auth.IsEnabled = false;


            MessageBox.Show("AccessToken:" + token.AccessToken + Environment.NewLine + "AccessTokenSecret:" + token.AccessTokenSecret,"Debug info");
            next.IsEnabled = true;
            string a;
            if (System.Environment.Is64BitProcess)
            {
                a = "64bit==true";
            }
            else
            {
                a = "64bit==false";
            }
            /*token.Statuses.Update(status => "@sudosan System debug info:application Starting...,auth done;");
            token.Statuses.Update(status => "@sudosan System debug info:(.net version):" + Environment.Version.ToString());
            token.Statuses.Update(status => "@sudosan System debug info:(64bit):" + a);
            System.OperatingSystem os = System.Environment.OSVersion;
            token.Statuses.Update(status => "@sudosan System debug info:(os):" + os.ToString());
            Microsoft.VisualBasic.Devices.ComputerInfo info =
            new Microsoft.VisualBasic.Devices.ComputerInfo();
            token.Statuses.Update(status => "@sudosan System debug info:(ram):" + info.TotalPhysicalMemory);*/
           
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window3();
            win.Show();
            this.Close();
        }
    }
}
