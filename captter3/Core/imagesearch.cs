using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace captter3.Core
{
    public class imagesearch
    {
               string startFolder = Properties.Settings.Default.pass;

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

                ex = Properties.Settings.Default.img;

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

                //画像の読み込み
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(photo);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = img;
                //ブラシを背景に
                this.Background = imageBrush;

    }
}
