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
        else if (count < 1290) { }
        else if (count == 1290)
        {
            GameObject obj = Instantiate(enemy2);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX - 600, Define.GameScreenCenterY));
            obj.GetComponent<EnemyPattern2>().movePattern = 0;
            obj = Instantiate(enemy2);
            obj.GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX + 600, Define.GameScreenCenterY));
            obj.GetComponent<EnemyPattern2>().movePattern = 1;
        }
        else if (count < 1800) { }
        else if (count < 2160)
        {
            // クッション時間の10秒，赤ヘリ
            int c = count - 1800;
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

        if (count >= 930 && count < 1800)
        {
            if (count % 90 == 0)
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
