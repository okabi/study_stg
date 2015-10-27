using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>自機・敵機の破壊時のオーラエフェクト</summary>
public class DestroyEffectController : MonoBehaviour {
    /// <summary>アタッチされているDestroyEffectStatus</summary>
    private DestroyEffectStatus destroyEffectStatus;

    /// <summary>アタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;


    void Awake()
    {
        destroyEffectStatus = GetComponent<DestroyEffectStatus>();
        drawingStatus = GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
    }


    void Update()
    {
        int c = destroyEffectStatus.count;

        if (c < 30)
        {
            destroyEffectStatus.speed -= 0.1f;
            drawingStatus.Alpha = (int)(255.0f * (30 - c) / 30);
            drawingStatus.Scale = 0.5f * (30 - c) / 30;
        }
        else
        {
            Destroy(this.gameObject);
        }
        float angle = destroyEffectStatus.angle * (float)System.Math.PI / 180.0f;
        drawingStatus.PositionScreen += destroyEffectStatus.speed * new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
        destroyEffectStatus.count += 1;
    }


    /// <summary>インスタンス生成時に呼ぶ初期化関数</summary>
    /// <param name="pos">生成座標(スクリーン座標系)</param>
    /// <param name="angle">エフェクトが進む角度(度)</param>
    public void Initialize(Vector2 pos, float angle)
    {
        drawingStatus.PositionScreen = pos;
        destroyEffectStatus.count = 0;
        destroyEffectStatus.speed = 3.0f + 0.1f * gameStatus.rand.Next(30);
        drawingStatus.Alpha = 255;
        drawingStatus.Blend = new Color(1.0f, 0.2f, 0.2f);
        drawingStatus.Scale = 0.5f;
        destroyEffectStatus.angle = angle;
        transform.parent = GameObject.Find("Effects").transform;
    }
}
