using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using StudySTG;

/// <summary>
///   ゲームの制御
/// </summary>
public class GameController : MonoBehaviour {
    /// <summary>プレイヤーのステータス</summary>
    public PlayerStatus playerStatus;

    /// <summary>ゲーム時間表示用UI(デバッグ用)</summary>
    public UIOutlinedText TimeCount;
    
    /// <summary>スコア表示用UI</summary>
    public UIOutlinedText UIScore;

    /// <summary>ゲームオーバーのUI</summary>
    public UIOutlinedText UIGameOver;

    /// <summary>ゲームオーバー時のスコア表示のUI</summary>
    public UIOutlinedText UIGameOverScore;

    /// <summary>フェードアウト</summary>
    public UIImage UIFade;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;

    /// <summary>録画用．1以上ならそのフレーム数ステージ動作を止める</summary>
    public int waitCount;

    /// <summary>結果発表用のテキスト</summary>
    public UIOutlinedText[] resultTexts;

    /// <summary>現在リプレイ再生中か</summary>
    private bool replaying;

    /// <summary>クリア時のプレイランク</summary>
    private int rank;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        gameStatus = GetComponent<GameStatus>();
        var replayStatus = GameObject.Find("ReplayController").GetComponent<ReplayStatus>();
        DateTime dt = DateTime.Now;
        int seed = dt.Year + dt.Month + dt.Day + dt.Hour + dt.Minute + dt.Second;
        if (GameObject.Find("SaveController") != null && GameObject.Find("SaveController").GetComponent<SaveStatus>().replaying)
        {
            seed = replayStatus.seed;
        }
        else
        {
            replayStatus.dateTime = dt.ToBinary();
            replayStatus.seed = seed;
        }
        gameStatus.rand = new System.Random(seed);
        gameStatus.count = 0;
        var obj = GameObject.Find("SaveController");
        if (obj != null)
        {
            replaying = obj.GetComponent<SaveStatus>().replaying;
        }

        // 画像の読み込み
        Sprite[] bulletSprites = Resources.LoadAll<Sprite>("Graphics/Bullets/Enemy");
        gameStatus.bulletSprites = new Dictionary<Define.BulletImageType, Sprite>();
        gameStatus.bulletSprites[Define.BulletImageType.MediumGreen] = Array.Find(bulletSprites, x => x.name.Equals("bullet0"));
        gameStatus.bulletSprites[Define.BulletImageType.BigRed] = Array.Find(bulletSprites, x => x.name.Equals("bullet1"));
        gameStatus.bulletSprites[Define.BulletImageType.BigBlue] = Array.Find(bulletSprites, x => x.name.Equals("bullet2"));
        gameStatus.bulletSprites[Define.BulletImageType.MediumPurple] = Array.Find(bulletSprites, x => x.name.Equals("bullet3"));
    }


    void Update()
    {
        if (replaying)
        {
            UIScore.Text = "Score " + playerStatus.score.ToString();
        }
        if (waitCount > 0)
        {
            waitCount -= 1;
        }
        else
        {
            gameStatus.count += 1;
            if (replaying)
            {
                TimeCount.Text = string.Format("Time {0} (frame: {1})", gameStatus.count / 60, gameStatus.count);
            }
        }

        if (gameStatus.gameoverCount > 0)
        {
            // ゲームオーバー処理
            int c = gameStatus.gameoverCount;
            UIGameOver.Text = "GameOver!!";
            UIGameOverScore.Text = String.Format("Score: {0}", playerStatus.score);
            if (c < 180)
            {
                UIFade.Alpha = (int)(255.0f * c / 180);
            }
            else if (c < 360)
            {
                UIFade.Alpha = 255;
            }
            else
            {
                var obj = GameObject.Find("SaveController");
                SaveStatus save = obj.GetComponent<SaveStatus>();
                if (save.replaying)
                {
                    save.nowScoreRanking = 0;
                }
                else
                {
                    save.nowScoreRanking = save.GetComponent<SaveController>().SaveScore(playerStatus.score, 0);
                    GameObject.Find("ReplayController").GetComponent<ReplayController>().Save();
                }
                Application.LoadLevel("Title");
            }
            gameStatus.gameoverCount += 1;
        }
        else if (gameStatus.resultCount > 0)
        {
            // 結果発表処理
            int c = gameStatus.resultCount;
            if (c < 300) { }
            else if (c == 300)
            {
                resultTexts[0].Text = "Clear!!";
            }
            else if (c < 360) { }
            else if (c == 360)
            {
                resultTexts[1].Text = String.Format("Basic Score: {0}", playerStatus.score);
            }
            else if (c < 390) { }
            else if (c == 390)
            {
                int bonus = 20000;
                playerStatus.score += bonus * (playerStatus.life - 1);
                resultTexts[2].Text = String.Format("Stock Bonus: {0} x {1}", bonus, playerStatus.life - 1);
            }
            else if (c < 420) { }
            else if (c == 420)
            {
                resultTexts[3].Text = String.Format("Total Score: {0}", playerStatus.score);
            }
            else if (c < 450) { }
            else if (c == 450)
            {
                if (playerStatus.score < 90000)
                {
                    rank = 1;
                }
                else if (playerStatus.score < 130000)
                {
                    rank = 2;
                }
                else if (playerStatus.score < 170000)
                {
                    rank = 3;
                }
                else
                {
                    rank = 4;
                }
                GameObject.Find("ReplayController").GetComponent<ReplayStatus>().rank = rank;
                resultTexts[4].Text = String.Format("Rank: {0}", rank == 1 ? "C" : rank == 2 ? "B" : rank == 3 ? "A" : "S");
            }
            else if (c < 570) { }
            else
            {
                c -= 570;
                if (c < 180)
                {
                    UIFade.Alpha = (int)(255.0f * c / 180);
                }
                else if (c < 360)
                {
                    UIFade.Alpha = 255;
                }
                else
                {
                    var obj = GameObject.Find("SaveController");
                    SaveStatus save = obj.GetComponent<SaveStatus>();
                    if (save.replaying)
                    {
                        save.nowScoreRanking = 0;
                    }
                    else
                    {
                        save.nowScoreRanking = save.GetComponent<SaveController>().SaveScore(playerStatus.score, rank);
                        GameObject.Find("ReplayController").GetComponent<ReplayController>().Save();
                    }
                    Application.LoadLevel("Title");
                }
            }
            gameStatus.resultCount += 1;
        }
        else
        {
            // タイトルに戻る
            if (Input.GetKey(KeyCode.T))
            {
                Application.LoadLevel("Title");
            }
        }
    }
}
