using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace captter3.Core
{

    public class RootObject
    {
        public int version { get; set; }
        public string change { get; set; }
        public string url { get; set; }
    }

    public class update
    {
        /*
        アップデート機能実装予定仕様

        サーバー(11514.jpなど)からJSONなどでデーターを受け取る
        データーの内容は
        ・最新のバージョン
        ・更新内容
        ・ファイルのURL
        内部バージョンと照らし合わせて最新バージョンのほうが数が大きい値の場合はアップデート確認画面を表示
        そのさい更新内容を表示する
        その後URLからファイルをダウンロードしてファイルを書き換え再起動
        */
        
        //どうもツイッター用にNewtonsoft.JSON(JSON.NET)を参照してるようなのでそれを使うことにする
        /*
         * JSONはこんなかんじにすると思う
        {
        "version" : "114514",
        "change" : "・ファイル読み込みバグを直した \n ・動作を高速化した \n",
        "url" : "https://github.com/sudosan/Captter/releases/download/star/captter3.zip"
        }
         */

        public void download()
        {
            try
            {
                string jsonurl = "https://114514.jp/update.json";
                System.Net.WebClient data = new System.Net.WebClient();
                byte[] bdata = data.DownloadData(jsonurl);
                data.Dispose();
                string json = System.Text.Encoding.UTF8.GetString(bdata);
                var deserializedList = JsonConvert.DeserializeObject<RootObject>(json);
                //MessageBox.Show(deserializedList.version.ToString());
                if(114513<deserializedList.version)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show(
                    "新しいバージョンがあります。アップデートしますか？" + Environment.NewLine + "更新内容" + Environment.NewLine + deserializedList.change, "updater",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        //アップデート処理
                        var update = new updater();
                        update.url = deserializedList.url;
                        update.download();
                        update.ShowDialog();
                    }
                }
            }
            catch(Exception)
            {

            }
        }

    }
}
