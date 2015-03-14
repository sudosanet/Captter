using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace captter3
{
    namespace Core
    {
        public class FoundNewImageEventArgs : EventArgs
        {
            public FoundNewImageEventArgs(string fileName)
            {
                FileName = fileName;
            }

            /// <summary>
            /// 画像のファイルパスです。
            /// </summary>
            public string FileName
            {
                get;
                private set;
            }
        }

        public class ImageSearch
        {
            private System.Threading.Timer timer;
            private System.Threading.TimerCallback callback;
            private bool isRunning = false;
            private bool isStarted = false;
            private DateTime dirLastWriteTime = new DateTime(0);
            public delegate void FoundNewImageHandler(object sender, FoundNewImageEventArgs e);
            public event FoundNewImageHandler foundNewImage;

            public ImageSearch()
            {
                callback = new System.Threading.TimerCallback(this.Img);
            }

            /// <summary>
            /// 画像検索を開始します。
            /// </summary>
            public void Start()
            {
                if (!isRunning)
                {
                    isRunning = true;
                    timer = new System.Threading.Timer(callback, null, 0, 1000);
                }
            }

            /// <summary>
            /// 画像検索を停止します。
            /// </summary>
            public void Stop()
            {
                isRunning = false;
                timer.Dispose();
            }

            /// <summary>
            /// 見つけた一番新しい画像のファイルパスです。
            /// </summary>
            public string NewestFileName
            {
                get;
                private set;
            }

            /// <summary>
            /// 画像を検索します。
            /// </summary>
            /// <returns>画像のファイルパス</returns>
            public string Search()
            {
                var dir = new System.IO.DirectoryInfo(Properties.Settings.Default.pass);
                var fileList = dir.GetFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);
                string ex = Properties.Settings.Default.img;

                try
                {
                    if (dirLastWriteTime == dir.LastWriteTime && isStarted)
                    {
                        return NewestFileName;
                    }
                    else
                    {
                        dirLastWriteTime = dir.LastWriteTime;
                    }

                    IEnumerable<System.IO.FileInfo> fileQuery =
                        from file in fileList
                        where file.Extension == ex
                        orderby file.Name
                        select file;

                    //画像があるかどうかのチェック
                    if (!fileQuery.Any()) return null;

                    var newestFile =
                        (from file in fileQuery
                         orderby file.CreationTime
                         select new { file.FullName, file.CreationTime }).Last();

                    FoundNewImage(new FoundNewImageEventArgs(newestFile.FullName));
                    NewestFileName = newestFile.FullName;
                    isStarted = true;

                    return newestFile.FullName;
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine(exception.ToString());
                    return null;
                }
            }

            private void Img(object sender)
            {
                Search();
            }

            private void FoundNewImage(FoundNewImageEventArgs e)
            {
                if (e != null) foundNewImage(this, e);
            }
        }
    }
}
