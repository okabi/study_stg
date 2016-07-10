using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   タイトル画面の制御
/// </summary>
public class TitleController : MonoBehaviour
{
    /// <summary>UIテキストのプレハブ</summary>
    public GameObject UITextPrefab;


    void Start()
    {
        GameObject save = GameObject.Find("SaveController");
        SaveStatus saveStatus = save.GetComponent<SaveStatus>();
        SaveController saveController = save.GetComponent<SaveController>();
        var scoreText = new UIOutlinedText[saveController.scoreNum];
        for (int i = 0; i < saveController.scoreNum; i++)
        {
            GameObject obj = Instantiate(UITextPrefab);
            obj.transform.SetParent(GameObject.Find("Canvas").transform);
            scoreText[i] = obj.GetComponent<UIOutlinedText>();
            string rankText = "GameOver";
            switch (saveStatus.rank[i])
            {
                case 1:
                    rankText = "Clear (Rank: C)";
                    break;
                case 2:
                    rankText = "Clear (Rank: B)";
                    break;
                case 3:
                    rankText = "Clear (Rank: A)";
                    break;
                case 4:
                    rankText = "Clear (Rank: S)";
                    break;
            }
            scoreText[i].Init(
                string.Format("{0, 2}.   {1, 7} pts    {2}", i + 1, saveStatus.hiscore[i], rankText),
                new Vector2(120, 80 + 30 * i),
                i + 1 == saveStatus.nowScoreRanking ? new Color(1, 1, 0.2f) : new Color(1, 1, 1),
                new Color(0, 0, 0),
                20);
            scoreText[i].Style = FontStyle.Bold;
        }
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject.Find("SaveController").GetComponent<SaveStatus>().replaying = false;
            Application.LoadLevel("MainGame");
        }
        else
        {
            // replay.rpy があれば削除する
            const string ReplayRpy = "replay.rpy";
            if (System.IO.File.Exists(ReplayRpy))
            {
                System.IO.File.Delete(ReplayRpy);
            }
            // 同じ階層にあるリプレイファイル名を全て取得する
            var replayFileNames = System.IO.Directory.GetFiles("./", "*.rpy");
            if (replayFileNames.Length == 0)
            {
                Application.Quit();
            }
            else
            {
                System.Array.Sort(replayFileNames);
                System.IO.File.Move(replayFileNames[0], ReplayRpy);
                GameObject.Find("SaveController").GetComponent<SaveStatus>().replaying = true;
                Application.LoadLevel("MainGame");
            }
        }
    }
}
