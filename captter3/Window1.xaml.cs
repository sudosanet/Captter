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
using System.Windows.Forms;
using captter3.Core;

namespace captter3
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        int cva = 0;
        //update
        private update update = new update();
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
            //form obuject load
            if(Properties.Settings.Default.img==".jpg")
            {
                jpg.IsChecked = true;
            }
            else if(Properties.Settings.Default.img==".png")
            {
                png.IsChecked = true;
            }
            pathbox.Text = Properties.Settings.Default.pass;
            syaro.IsChecked = Properties.Settings.Default.AllowSyaromode;

            //bild date
            var info = new System.IO.FileInfo(System.Windows.Forms.Application.ExecutablePath);
            string date = Convert.ToString(info.CreationTime);//LastWriteTime
            this.date.Content = "build date:" + date;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(jpg.IsChecked==true)
            {
                Properties.Settings.Default.img = ".jpg";
            }
            else if (png.IsChecked==true)
            {
                Properties.Settings.Default.img =".png";
            }
            else
            {
                return;
            }

            Properties.Settings.Default.AllowSyaromode =  (bool) syaro.IsChecked;

            //path
            Properties.Settings.Default.pass = pathbox.Text;

            Properties.Settings.Default.Save();
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            System.Windows.MessageBox.Show("設定を消去しました。終了します", "情報");
            Environment.Exit(0);
        }

        private void cv_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void cv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cva++;
            if (cva == 3)
            {
                cva = 0;
                EE f = new EE();

                f.ShowDialog();
            }
        }

        private void tana3n_Click(object sender, RoutedEventArgs e)
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //上部に表示する説明テキストを指定する
            fbd.Description = "tvtestの画像がキャプチャされるフォルダを指定してください。";
            //ルートフォルダを指定する
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"C:\";
            //ユーザーが新しいフォルダを作成できるようにする
            fbd.ShowNewFolderButton = true;
            //ダイアログを表示する
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //選択されたフォルダをtxetbox3に代入
                pathbox.Text = fbd.SelectedPath;
            }
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            //こうしたほうがいい感じがしたので
            update.download();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            high.IsEnabled = Convert.ToBoolean(acp.IsChecked);
        }

    }
}
