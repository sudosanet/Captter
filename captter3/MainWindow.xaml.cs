using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using captter3.Core;


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
        string homo = Properties.Settings.Default.imgpass;
        //壁紙の画像のファイルパス
        string backgroundImageFileName = null;
        //coreTweet
        public CoreTweet.OAuth.OAuthSession session;
        public CoreTweet.Tokens token;
        //画像検索
        private ImageSearch imageSearch = new ImageSearch();
        //update
        private update update = new update();
        public MainWindow()
        {
            InitializeComponent();
            //移行処理
            if (Properties.Settings.Default.IsUpgrade == false)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgrade = true;
                Properties.Settings.Default.Save();
            }
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
                if (Properties.Settings.Default.AllowSyaromode)
                {
                    syaro = true;
                    mode.Content = "true";
                }
                else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show(
                        "この機能はあなたのタイムラインにいる人に迷惑がかかる可能性があります。\n\n有効にしますか？", "警告",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        syaro = true;
                        mode.Content = "true";
                    }
                }

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

            token = Tokens.Create(twitter.CK,twitter.CS,
                Properties.Settings.Default.AccessToken,
                Properties.Settings.Default.TokenSecret);

            imageSearch.foundNewImage += ImageSearch_NewImageFound;
            imageSearch.Start();
            update.download();
        }

        /// <summary>
        /// 背景画像の更新をします。
        /// </summary>
        /// <param name="photoFileName">画像ファイルのパス</param>
        private void UpdateBackground(string photoFileName)
        {
            if (backgroundImageFileName != photoFileName)
            {
                //画像の読み込み
                var img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(photoFileName);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = img;

                // ブラシを背景に設定する
                this.Background = imageBrush;

                backgroundImageFileName = photoFileName;
            }
        }

        /// <summary>
        /// ツイートします。
        /// </summary>
        /// <param name="photoFileName">画像ファイルのパス</param>
        private void StatusesUpdate(string photoFileName)
        {
            var mediaUploadTask = token.Media.UploadAsync(
                media => new FileInfo(photoFileName));
            string statusText = tweet.Text;

            mediaUploadTask.ContinueWith((x) =>
            {
                if (x.IsCompleted)
                {
                    token.Statuses.UpdateAsync(
                        status => statusText + Environment.NewLine + has.Text,
                        media_ids => x.Result.MediaId);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            homo = Properties.Settings.Default.imgpass = photoFileName;
            Properties.Settings.Default.Save();
            tweet.Clear();
        }

        /// <summary>
        /// 画像が見つかったときの動作
        /// </summary>
        private void ImageSearch_NewImageFound(object sender, Core.FoundNewImageEventArgs e)
        {
            if (e.FileName != null)
            {
                if (Dispatcher.CheckAccess())
                {
                    UpdateBackground(e.FileName);
                    if (syaro && homo != e.FileName) StatusesUpdate(e.FileName);
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ImageSearch_NewImageFound(sender, e);
                    }));
                }
            }
        }

        //手動ツイート
        private void tweet_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                string image = imageSearch.Search();
                if (image != null)
                {
                    UpdateBackground(image);
                    if (homo != image) StatusesUpdate(image);
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
            try
            {
                System.IO.Directory.Delete(System.Environment.CurrentDirectory + "\\tmp", true);
            }
            catch (DirectoryNotFoundException) { return; }
        }

        private void keep_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
