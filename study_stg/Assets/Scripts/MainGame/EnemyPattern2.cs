using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ちょっと強い敵(迷彩色飛行機)
///   動き: 画面左(右)前方に，機体回転しながら向かった後，まっすぐ降りる
///   ショット: まっすぐ下に撃つ
/// </summary>
public class EnemyPattern2 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    /// <summary>0なら左，1なら右に動く</summary>
    public int movePattern;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    /// <summary>目的位置</summary>
    private Vector2 destination;

    /// <summary>初期位置</summary>
    private Vector2 start;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
    }


    void Update()
    {
        int count = enemyStatus.count;

        // 移動
        if (count == 0)
        {
            enemyStatus.lockonTargetPosition = new Vector2[5];
            if (movePattern < 2)
            {
                destination = new Vector2(Define.GameScreenCenterX - 200 * (float)System.Math.Pow(-1, movePattern), 120);
            }
            else
            {
                destination = new Vector2(Define.GameScreenCenterX - 80 * (float)System.Math.Pow(-1, movePattern), 50);
            }
            start = drawingStatus.PositionScreen;
        }
        else if (count < 90)
        {
            Vector2 deltaPos = start - destination;
            Vector2 a = deltaPos / (90 * 90);
            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((90 - count), 2);
        }
        else if (count == 90)
        {
            enemyStatus.angle = 90;
        }
        else if (count < 120)
        {
            enemyStatus.speed += 0.02f;
        }
        enemyStatus.lockonTargetPosition[0] = enemyStatus.lockonEffectPosition;
        enemyStatus.lockonTargetPosition[1] = enemyStatus.lockonEffectPosition + new Vector2(20, 5);
        enemyStatus.lockonTargetPosition[2] = enemyStatus.lockonEffectPosition + new Vector2(-20, 5);
        enemyStatus.lockonTargetPosition[3] = enemyStatus.lockonEffectPosition + new Vector2(0, -20);
        enemyStatus.lockonTargetPosition[4] = enemyStatus.lockonEffectPosition + new Vector2(0, 20);

        // モデル回転
        if (count < 90)
        {
            transform.Rotate(0.0f, 4.0f * (float)System.Math.Pow(-1, movePattern), 0.0f);
        }

        // ショット
        if (count >= 150)
        {
            if ((count - 150) % 120 < 20)
            {
                if ((count - 150) % 5 == 0)
                {
                    // 真下にまっすぐに弾を撃つ
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject bullet = Instantiate(bullet0);
                        bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumPurple, drawingStatus.PositionScreen + new Vector2(-15 + 30 * i, 30), 5.0f, 90);
                    }
                }
            }
        }
    }
}
