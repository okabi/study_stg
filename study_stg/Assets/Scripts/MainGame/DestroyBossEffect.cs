using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ボス撃破後の演出
/// </summary>
public class DestroyBossEffect : MonoBehaviour
{
    /// <summary>アタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>部位破壊のエフェクトのプレハブ</summary>
    public GameObject destroyPartEffectPrefab;

    /// <summary>取得するスコア</summary>
    private int score;

    /// <summary>生成されてからのフレーム数</summary>
    private int count;


    void Awake()
    {
        drawingStatus = GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        drawingStatus.Blend = new Color(1, 0.1f, 0.1f);
    }


    void Update()
    {
        if (count < 180)
        {
            for (int i = 0; i < 8; i++)
            {
                Instantiate(destroyPartEffectPrefab).GetComponent<DestroyPartEffect>().Initialize(
                    drawingStatus.PositionScreen + new Vector2(-280, -30) + new Vector2(gameStatus.rand.Next(560), gameStatus.rand.Next(30)),
                    -115 + gameStatus.rand.Next(50));
                Instantiate(destroyPartEffectPrefab).GetComponent<DestroyPartEffect>().Initialize(
                    drawingStatus.PositionScreen + new Vector2(-15, -260) + new Vector2(gameStatus.rand.Next(30), gameStatus.rand.Next(400)),
                    -115 + gameStatus.rand.Next(50));
            }
            drawingStatus.PositionScreen += 0.4f * new Vector2(1, 1);
        }
        else
        {
            Destroy(gameObject);
        }

        count += 1;
    }


    /// <summary>
    ///   初期化関数．
    /// </summary>
    /// <param name="pos">生成する座標(スクリーン座標系)</param>
    /// <param name="score">取得するスコア</param>
    public void Initialize(Vector2 pos, int score)
    {
        drawingStatus.PositionScreen = pos;
        this.score = score;
    }
}
