using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace captter3.Core
{
    public class imagesearch
    {
        public void img (object sender, EventArgs e)
        {
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Properties.Settings.Default.pass);
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
        }

    }
}
