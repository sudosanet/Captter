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

namespace captter3
{
    /// <summary>
    /// EE.xaml の相互作用ロジック
    /// </summary>
    public partial class EE : Window
    {
        public EE()
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

        int stc = 0;
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            System.IO.Stream inari = Properties.Resources.inari;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(inari);
            player.Play();
            player.Dispose();
            stc++;
            ver.Text = "Captter 3.1 inari";
            if(stc==1)
            {
                st.ScaleX = 0.6;
                st.ScaleY = 0.6;
            }
            else if(stc==2)
            {
                st.ScaleX = 0.7;
                st.ScaleY = 0.7;
            }
            else if (stc == 3)
            {
                st.ScaleX = 0.8;
                st.ScaleY = 0.8;
            }
            else if (stc == 4)
            {
                st.ScaleX = 0.9;
                st.ScaleY = 0.9;
            }
            else if (stc == 5)
            {
                st.ScaleX = 1;
                st.ScaleY = 1;
            }
            else if (stc == 6)
            {
                st.ScaleX = 1.1;
                st.ScaleY = 1.1;
            }
            else
            {
                this.Close();
            }

        }
    }
}
