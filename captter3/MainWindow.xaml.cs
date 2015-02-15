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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using CoreTweet;
using System.IO;
using CoreTweet.Streaming;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Security.Cryptography;


//tana3nはホモ
/*
 * tana3n is gay
 * tana3n is gay
 * tana3n is gay
 * tana3n is gay
 * tana3n is gay
 * tana3n is gay
 * tana3n is gay
 */
//tana3nはホモ

/*
 * アップデート機能実装
 * バージョン、変更ないよう取得
 * ZIP解凍
 * 置き換え
 */
namespace captter3
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        bool syaro = false;
        //連投防止
        string homo;
        DateTime dirWriteDateTime;
        //coreTweet
        public CoreTweet.OAuth.OAuthSession session;
        public CoreTweet.Tokens token;
        public const string CK = "bTLiIE3LOyadvojbBf5AEXIsU";
        public const string CS = "ztjWDrWJ9YOsfPKMRw9Q0uqDywEjmmCKSIIK6eoXP81tWYe9H6";
        public MainWindow()
        {
            InitializeComponent();

           
        }
        /// <summary>
        /// 起動後にドラッグで移動できるようにする
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        /// <summary>
        /// ボタンをクリックしたときの動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 f = new Window1();
            f.ShowDialog();
        }

        /// <summary>
        /// 終了ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("閉じますか？", "警告",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(!syaro)
            {
                syaro = true;
                mode.Content = "true";
            }
            else
            {
                mode.Content = "false";
                syaro = false;
            }

        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.AccessToken=="")
            {
                try
                {
                    Window2 f = new Window2();
                    f.ShowDialog();
                }
                catch(Exception)
                {
                    System.Windows.MessageBox.Show("エラーが発生しました、もう一度やり直してください","info");
                }
            }
            else
            {
                //timer
                DispatcherTimer testTimer;
                testTimer = new DispatcherTimer();
                testTimer.Interval = new TimeSpan(0, 0, 1);
                testTimer.Tick += new EventHandler(testTimer_Tick);
                testTimer.Start();
                token = Tokens.Create(CK,
                    CS,
                    Properties.Settings.Default.AccessToken,
                    Properties.Settings.Default.TokenSecret);
            }
        }
        public void testTimer_Tick(object sender, EventArgs e)
        {
            string startFolder = Properties.Settings.Default.pass;
            try
            {
                //別スレッドで実行してUIのフリーズをなくすべき？
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

                //ディレクトリに画像が書き込まれているかどうかのチェック
                if (dirWriteDateTime == dir.LastWriteTime)
                {
                    return;
                }
                else
                {
                    dirWriteDateTime = dir.LastWriteTime;
                }
                IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);
                string ex = null;

                ex = ".jpg";//Properties.Settings.Default.img;

                IEnumerable<System.IO.FileInfo> fileQuery =
                    from file in fileList
                    where file.Extension == ex
                    orderby file.Name
                    select file;

                //画像があるかどうかのチェック
                if (!fileQuery.Any())
                {
                    return;
                }

                var newestFile =
                    (from file in fileQuery
                    orderby file.CreationTime
                    select new { file.FullName, file.CreationTime }).Last();

                string photo = newestFile.FullName; // ツイートする画像のパス
                if (Properties.Settings.Default.imgpass == photo)
                {
                    //連投防止
                    return;
                }
                else if (photo != homo)
                {
                    // イメージブラシの作成
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(photo, UriKind.Relative));
                    // ブラシを背景に設定する
                    this.Background = imageBrush;
                    if (syaro)
                    {
                        token.Statuses.UpdateWithMedia(
                        status => tweet.Text + Environment.NewLine + has.Text,
                        media => new FileInfo(photo));
                        homo = photo;
                        Properties.Settings.Default.imgpass = photo;
                        tweet.Text = null;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        //手動ツイート
        private void tweet_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                string startFolder = Properties.Settings.Default.pass;
                try
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);


                    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);
                    string ex = null;

                    ex = ".jpg";//Properties.Settings.Default.img;

                    IEnumerable<System.IO.FileInfo> fileQuery =
                        from file in fileList
                        where file.Extension == ex
                        orderby file.Name
                        select file;

                    var newestFile =
                        (from file in fileQuery
                         orderby file.CreationTime
                         select new { file.FullName, file.CreationTime })
                        .Last();

                    string photo = newestFile.FullName; // ツイートする画像のパス
                    if (Properties.Settings.Default.imgpass == photo)
                    {
                        //連投防止
                        return;
                    }

                    else if (photo != homo)
                    {
                        // イメージブラシの作成
                        ImageBrush imageBrush = new ImageBrush();
                        imageBrush.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(photo, UriKind.Relative));
                        // ブラシを背景に設定する
                        this.Background = imageBrush;
                            token.Statuses.UpdateWithMedia(
                                status => tweet.Text + Environment.NewLine + has.Text,
                                media => new FileInfo(photo));

                            homo = photo;
                            Properties.Settings.Default.imgpass = photo;
                            tweet.Text = null;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*起動時画面移動の処理*/
            var KBTIT = SystemInformation.WorkingArea.Height; //ありがとナス
            var hide = SystemInformation.WorkingArea.Width; //ぼくひで

            //大嘘サイズ習得
            PresentationSource source = PresentationSource.FromVisual(this);
            double dpiX =1;
            double dpiY =1;
            if (source != null && source.CompositionTarget != null)
            {
                dpiX = source.CompositionTarget.TransformToDevice.M11;
                dpiY = source.CompositionTarget.TransformToDevice.M22;
            }
            //計算
            var X = hide / dpiX;
            var Y = KBTIT / dpiY;
            //画面の移動

            this.Top = Y - (this.Height);
            this.Left = X - (this.Width);
            //string i = Convert.ToString(dpiY);
            //System.Windows.MessageBox.Show(i);
        }
    }
}
