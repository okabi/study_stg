using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

namespace StudySTG
{
    /// <summary>
    ///   全シーンで残り続ける，セーブデータを扱うクラス
    /// </summary>
    public class SaveController : MonoBehaviour
    {
        /// <summary>既にインスタンス化されたか</summary>
        private static bool created = false;

        /// <summary>アタッチされているSaveStatus</summary>
        private SaveStatus saveStatus;

        /// <summary>保存するスコアデータの数</summary>
        public readonly int scoreNum = 10;

        /// <summary>セーブデータの外部ファイル</summary>
        private const string fileName = @"savedata.dat";

        /// <summary>セーブデータのパスワード</summary>
        private const string password = "卒業";


        void Awake()
        {
            // 既に作成されたインスタンスか？
            if (created)
            {
                Destroy(gameObject);
            }
            created = true;
            DontDestroyOnLoad(gameObject);

            // セーブデータの読み込み
            saveStatus = GetComponent<SaveStatus>();
            saveStatus.hiscore = new int[scoreNum];
            saveStatus.cleared = new bool[scoreNum];
            saveStatus.replaying = false;
            bool resetFlag = true;
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string st = sr.ReadToEnd();
                    var data = Json.Deserialize(Crypt.DecryptString(st, password)) as Dictionary<string, object>;
                    if ((long)data["scoreNum"] == scoreNum)
                    {
                        var scoreData = (List<object>)data["score"];
                        var clearData = (List<object>)data["cleared"];
                        for (int i = 0; i < scoreNum; i++)
                        {
                            saveStatus.hiscore[i] = (int)(long)scoreData[i];
                            saveStatus.cleared[i] = System.Convert.ToBoolean(clearData[i].ToString());
                        }
                        resetFlag = false;
                    }
                }
            }
            if (resetFlag)
            {
                var scoreData = new int[scoreNum];
                for (int i = 0; i < scoreNum; i++)
                {
                    int score = (scoreNum - i) * 1000;
                    saveStatus.hiscore[i] = score;
                    saveStatus.cleared[i] = false;
                    scoreData[i] = score;
                }
                Save();
            }
        }


        /// <summary>
        ///   スコアを保存する
        /// </summary>
        /// <param name="score">スコア</param>
        /// <returns>何位にランクインしたか(1~10)．0はランクインせず</returns>
        public int SaveScore(int score)
        {
            int retval = 0;
            for (int i = 0; i < scoreNum; i++)
            {
                if (score >= saveStatus.hiscore[i])
                {
                    for (int j = scoreNum - 1; j >= i + 1; j--)
                    {
                        saveStatus.hiscore[j] = saveStatus.hiscore[j - 1];
                    }
                    saveStatus.hiscore[i] = score;
                    retval = i + 1;
                    break;
                }
            }
            Save();
            return retval;
        }


        /// <summary>
        ///   外部ファイルにデータを保存する
        /// </summary>
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["scoreNum"] = scoreNum;
                var scoreData = new int[scoreNum];
                var clearData = new string[scoreNum];
                for (int i = 0; i < scoreNum; i++)
                {
                    scoreData[i] = saveStatus.hiscore[i];
                    clearData[i] = saveStatus.cleared[i].ToString();
                }
                data["score"] = scoreData;
                data["cleared"] = clearData;
                sw.Write(Crypt.EncryptString(Json.Serialize(data), password));
            }

        }
    }
}
