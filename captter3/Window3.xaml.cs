using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace captter3
{
    /// <summary>
    /// Window3.xaml の相互作用ロジック
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
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
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog();
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
                pass.Content = fbd.SelectedPath;
                Properties.Settings.Default.pass = fbd.SelectedPath;
                next.IsEnabled = true;
            }

        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if(jpg.IsChecked==true)
            {
                Properties.Settings.Default.img = ".jpg";
            }
            else if (png.IsChecked==true)
            {
                Properties.Settings.Default.img = "png";
            }
            else
            {
                return;
            }
            Properties.Settings.Default.Save();
            var win = new Window4();
            win.Show();
            this.Close();
        }

    }
}
