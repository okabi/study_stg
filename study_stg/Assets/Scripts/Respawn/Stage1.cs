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

    /// <summary>一番弱い敵</summary>
    public GameObject enemy0;


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
        if (gameStatus.count % 120 == 0)
        {
            Instantiate(enemy0).GetComponent<EnemyController>().Initialize(new Vector2(Define.GameScreenCenterX, -100));
        }
    }
}
