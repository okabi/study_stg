using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>自機・敵機の破壊時のオーラエフェクト</summary>
public class DestroyEffect : MonoBehaviour {
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

    /// <summary>消滅までのフレーム数</summary>
    private int disappearCount;

    /// <summary>初期速度</summary>
    private float originalSpeed;


    void Awake()
    {
        drawingStatus = GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
    }


    void Update()
    {
        int c = count;

        if (c < disappearCount)
        {
            speed -= originalSpeed / disappearCount;
            drawingStatus.Alpha = (int)(255.0f * (disappearCount - c) / disappearCount);
            drawingStatus.Scale = 0.5f * (disappearCount - c) / disappearCount;
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
    /// <param name="score">何点を獲得した時のエフェクトか</param>
    /// <param name="color">エフェクトの色</param>
    public void Initialize(Vector2 pos, float angle, int score, Color color)
    {
        drawingStatus.PositionScreen = pos;
        count = 0;
        speed = 1.0f + Mathf.Min(4.0f, 0.1f * score / 200) + 0.1f * gameStatus.rand.Next(40);
        originalSpeed = speed;
        drawingStatus.Alpha = 255;
        drawingStatus.Blend = color;
        drawingStatus.Scale = 0.5f;
        this.angle = angle;
        disappearCount = 30 + Mathf.Min(score / 100, 90);
        transform.parent = GameObject.Find("Effects").transform;
    }
}
