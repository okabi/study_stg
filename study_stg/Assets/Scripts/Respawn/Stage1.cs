using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>敵の出現等を制御する(ステージ1)</summary>
public class Stage1 : MonoBehaviour {
    /// <summary>ゲームの総合情報</summary>
    private GameStatus gameStatus;

    /// <summary>背景(真ん中)のDrawingStatus</summary>
    public DrawingStatus[] BackgroundMiddle;

    /// <summary>背景(手前)のDrawingStatus</summary>
    public DrawingStatus[] BackgroundFront;

    /// <summary>赤ヘリ(指定位置に行った後まっすぐ降りる．自機狙い弾を撃つ．)</summary>
    public GameObject enemy0;

    /// <summary>青ヘリ(左(右)前方へ行って自機を狙って動く)</summary>
    public GameObject enemy1;

    /// <summary>迷彩飛行機(横から現れてまっすぐ下りながら真下に弾を打つ)</summary>
    public GameObject enemy2;

    /// <summary>青ヘリ(まっすぐ降りた後円弧を描いて退散する)</summary>
    public GameObject enemy3;

    /// <summary>赤ヘリ(左(右)前方へ行って自機を狙って動く)</summary>
    public GameObject enemy4;

    /// <summary>白飛行機(強い感じの中ボス)</summary>
    public GameObject enemy5;

    /// <summary>青ヘリ(左(右)から登場した後円弧を描いて上方へ退散する)</summary>
    public GameObject enemy6;

    /// <summary>赤ヘリ(左(右)から登場した後円弧を描いて上方へ退散する)</summary>
    public GameObject enemy7;

    /// <summary>ボス</summary>
    public GameObject boss0;

    /// <summary>ボス登場時のエフェクト</summary>
    public GameObject bossEffect;

    /// <summary>生成した中ボスのインスタンス</summary>
    private GameObject midBoss;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        gameStatus = GetComponent<GameStatus>();
    }


    void Update()
    {
        Background();
        Respawn();
    }


    /// <summary>背景の操作</summary>
    void Background()
    {
        for (int i = 0; i < BackgroundMiddle.Length; i++)
        {
            float plus = gameStatus.count % 480;
            BackgroundMiddle[i].PositionScreen = new Vector2(320, 240 + plus - 480 * i);
        }
        for (int i = 0; i < BackgroundFront.Length; i++)
        {
            float plus = 3 * gameStatus.count % 480;
            BackgroundFront[i].PositionScreen = new Vector2(320, 240 + plus - 480 * i);
        }
    }


    /// <summary>敵機の出現</summary>
    void Respawn()
    {
        int count = gameStatus.count;

        if (count < 60) { }
        else if (count < 360)
        {
            // クッション時間の10秒，青ヘリ
            if (count % 60 == 0)
            {
                int n = (count - 60) / 60;
                GameObject obj = Instantiate(enemy1);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 50.0f + 30.0f * n, -50));
                obj.GetComponent<EnemyPattern1>().movePattern = 0;
            }
            else if (count % 60 == 30)
            {
                int n = (count - 60) / 60;
                GameObject obj = Instantiate(enemy1);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 50.0f - 30.0f * n, -50));
                obj.GetComponent<EnemyPattern1>().movePattern = 1;
            }
        }
        else if (count < 480) { }
        else if (count < 930)
        {
            // スコア重視時間の25秒，赤ヘリ
            int c = count - 480;
            if (c == 0)
            {
                GameObject obj;
                obj= Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, 0));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 230, 120);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, -50));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 150, 70);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, 50));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 190, 170);
            }
            else if (c == 180)
            {
                GameObject obj;
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, 0));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 230, 120);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, -50));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 150, 70);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, 50));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 190, 170);
            }
            else if (c == 360)
            {
                GameObject obj;
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX, -100));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 30, 120);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX, -150));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 30, 120);
                obj = Instantiate(enemy0);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX, -50));
                obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX, 170);
            }
        }
        else if (count < 960) { }
        else if (count < 1410)
        {
            // 道中前半の円形青ヘリ
            int c = count - 960;
            if (c % 60 == 0)
            {
                GameObject obj;
                obj = Instantiate(enemy3);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 150, -50));
                obj.GetComponent<EnemyPattern3>().movePattern = 2;
            }
            else if (c % 60 == 30)
            {
                GameObject obj;
                obj = Instantiate(enemy3);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 150, -50));
                obj.GetComponent<EnemyPattern3>().movePattern = 3;
            }
        }
        else if (count == 1410)
        {
            GameObject obj;
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, 0));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 230, 120);
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, -50));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 150, 70);
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, 50));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX - 190, 170);
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, 0));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 230, 120);
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, -50));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 150, 70);
            obj = Instantiate(enemy0);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, 50));
            obj.GetComponent<EnemyPattern0>().destination = new Vector2(Define.GameScreenCenterX + 190, 170);

        }
        else if (count < 1890) { }
        else if (count == 1890)
        {
            // 道中前半の飛行機
            GameObject obj = Instantiate(enemy2);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, Define.GameScreenCenterY));
            obj.GetComponent<EnemyPattern2>().movePattern = 0;
            obj = Instantiate(enemy2);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, Define.GameScreenCenterY));
            obj.GetComponent<EnemyPattern2>().movePattern = 1;
        }
        else if (count < 2400) { }
        else if (count < 2760)
        {
            // クッション時間の10秒，赤ヘリ
            int c = count - 2400;
            if (c % 60 == 0)
            {
                int n = (c - 60) / 60;
                GameObject obj = Instantiate(enemy4);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 50.0f + 30.0f * n, -50));
                obj.GetComponent<EnemyPattern4>().movePattern = 0;
            }
            else if (c % 60 == 30)
            {
                int n = (c - 60) / 60;
                GameObject obj = Instantiate(enemy4);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 50.0f - 30.0f * n, -50));
                obj.GetComponent<EnemyPattern4>().movePattern = 1;
            }
        }
        else if (count < 3000) { }
        else if (count == 3000)
        {
            // 中ボス
            midBoss = Instantiate(enemy5);
            midBoss.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX, -100));
        }
        else if (count < 3840)
        {
            // 早回し
            int c = count - 3000;
            if (midBoss == null)
            {
                if (c % 120 == 0)
                {
                    int n = (c % 600) / 120;
                    GameObject obj = Instantiate(enemy4);
                    obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 50.0f + 30.0f * n, -50));
                    obj.GetComponent<EnemyPattern4>().movePattern = 0;
                }
                else if (c % 120 == 60)
                {
                    int n = (c % 600) / 120;
                    GameObject obj = Instantiate(enemy4);
                    obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 50.0f - 30.0f * n, -50));
                    obj.GetComponent<EnemyPattern4>().movePattern = 1;
                }
            }
        }
        else if (count < 4380)
        {
            // 中ボス後の青ヘリ
            int c = count - 3840;
            if (c % 90 == 0)
            {
                GameObject obj;
                obj = Instantiate(enemy6);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(-50, Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern6>().movePattern = 0;
                obj = Instantiate(enemy6);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenSizeX + 50, Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern6>().movePattern = 1;
            }
        }
        else if (count < 4920)
        {
            // 中ボス後の赤ヘリ
            int c = count - 4380;
            if (c % 90 == 0)
            {
                GameObject obj;
                obj = Instantiate(enemy7);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(-50, Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern7>().movePattern = 0;
                obj = Instantiate(enemy7);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenSizeX + 50, Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern7>().movePattern = 1;
            }
        }
        else if (count < 4980) { }
        else if (count == 4980)
        {
            // ボス前の飛行機2体
            for (int i = 0; i < 2; i++)
            {
                GameObject obj = Instantiate(enemy2);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600 * Mathf.Pow(-1, i), Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern2>().movePattern = i;
            }
        }
        else if (count < 5040) { }
        else if (count == 5040)
        {
            // ボス前の飛行機2体
            for (int i = 0; i < 2; i++)
            {
                GameObject obj = Instantiate(enemy2);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600 * Mathf.Pow(-1, i), Define.GameScreenCenterY));
                obj.GetComponent<EnemyPattern2>().movePattern = i + 2;
            }
        }
        else if (count < 5460) { }
        else if (count == 5460)
        {
            // ボス登場エフェクト
            Instantiate(bossEffect).GetComponent<BossEffect>().Initialize(
                new Vector2(Define.GameScreenCenterX, Define.GameScreenSizeY + 300));
        }
        else if (count < 5580) { }
        else if (count == 5580)
        {
            // ボス
            GameObject obj = Instantiate(boss0);
            obj.GetComponent<EnemyController>().Initialize(
                new Vector2(Define.GameScreenCenterX, -200));
        }

        if (count >= 1530 && count < 2400)
        {
            // 道中前半の飛行機が出てくる場面での青ヘリ
            int c = count - 1410;
            if (c % 90 == 0)
            {
                GameObject obj;
                obj = Instantiate(enemy3);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 30, -50));
                obj.GetComponent<EnemyPattern3>().movePattern = 1;
                obj = Instantiate(enemy3);
                obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 30, -50));
                obj.GetComponent<EnemyPattern3>().movePattern = 0;
            }
        }
    }
}
