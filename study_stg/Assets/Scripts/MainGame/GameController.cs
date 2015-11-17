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

    /// <summary>フェードアウト</summary>
    public UIImage UIFade;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;

    /// <summary>録画用．1以上ならそのフレーム数ステージ動作を止める</summary>
    public int waitCount;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        gameStatus = GetComponent<GameStatus>();
        gameStatus.rand = new System.Random();
        gameStatus.count = 0;

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
        UIScore.Text = "Score " + playerStatus.score.ToString();
        if (waitCount > 0)
        {
            waitCount -= 1;
        }
        else
        {
            gameStatus.count += 1;
            TimeCount.Text = string.Format("Time {0} (frame: {1})", gameStatus.count / 60, gameStatus.count);
        }

        if (gameStatus.gameoverCount > 0)
        {
            int c = gameStatus.gameoverCount;
            UIGameOver.Text = "GameOver!!";
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
                Application.LoadLevel("Title");
            }

            gameStatus.gameoverCount += 1;
        }
    }
}
