using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ボスの部分破壊時に発生する炎のエフェクト
/// </summary>
public class DestroyPartEffect : MonoBehaviour {
    /// <summary>アタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>生成からのカウント</summary>
    private int count;

    /// <summary>移動スピード</summary>
    private float speed;

    /// <summary>移動角度(度)</summary>
    private float angle;


    void Awake()
    {
        drawingStatus = GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
    }


    void Update()
    {
        int c = count;

        if (c < 60)
        {
            speed -= 0.05f;
            drawingStatus.Alpha = (int)(255.0f * (60 - c) / 30);
            drawingStatus.Scale = 0.5f * (60 - c) / 30;
        }
        else
        {
            Destroy(this.gameObject);
        }
        float a = angle * (float)System.Math.PI / 180.0f;
        drawingStatus.PositionScreen += speed * new Vector2((float)System.Math.Cos(a), (float)System.Math.Sin(a));
        count += 1;
    }


    /// <summary>インスタンス生成時に呼ぶ初期化関数</summary>
    /// <param name="pos">生成座標(スクリーン座標系)</param>
    /// <param name="angle">エフェクトが進む角度(度)</param>
    public void Initialize(Vector2 pos, float angle)
    {
        drawingStatus.PositionScreen = pos;
        count = 0;
        speed = 5.0f + 0.1f * gameStatus.rand.Next(20);
        drawingStatus.Alpha = 255;
        drawingStatus.Blend = new Color(1.0f, 0.2f, 1.0f);
        drawingStatus.Scale = 0.5f;
        this.angle = angle;
        transform.parent = GameObject.Find("Effects").transform;
    }
}
