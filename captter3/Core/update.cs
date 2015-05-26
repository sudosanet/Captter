﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace captter3.Core
{
    class update
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
         *JSON.NETを使う場合
        public class RootObject
        {
            public int version { get; set; }
            public string change { get; set; }
            public string url { get; set; }
        }
         */


    }
}