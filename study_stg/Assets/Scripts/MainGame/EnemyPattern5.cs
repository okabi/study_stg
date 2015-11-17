using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   強い敵(白色飛行機)
///   動き: 画面中央に，機体回転しながら向かった後，まっすぐ降りる
///   ショット: 
/// </summary>
public class EnemyPattern5 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    /// <summary>少し進んで弾が変化するやつ</summary>
    public GameObject bullet1;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;

    /// <summary>目的位置</summary>
    private Vector2 destination;

    /// <summary>初期位置</summary>
    private Vector2 start;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
    }


    void Update()
    {
        int count = enemyStatus.count;

        // 移動
        if (count == 0)
        {
            enemyStatus.lockonTargetPosition = new Vector2[5];
            destination = new Vector2(Define.GameScreenCenterX, 70);
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
            enemyStatus.speed = 0.1f;
        }
        else if (count < 890)
        {
            enemyStatus.angle = 3 * count;
        }
        else if (count == 890)
        {
            enemyStatus.angle = 90;
        }
        else if (count < 920)
        {
            enemyStatus.speed += 0.02f;
        }
        enemyStatus.lockonTargetPosition[0] = enemyStatus.lockonEffectPosition;
        enemyStatus.lockonTargetPosition[1] = enemyStatus.lockonEffectPosition + new Vector2(20, 5) * 0.3f / 0.2f;
        enemyStatus.lockonTargetPosition[2] = enemyStatus.lockonEffectPosition + new Vector2(-20, 5) * 0.3f / 0.2f;
        enemyStatus.lockonTargetPosition[3] = enemyStatus.lockonEffectPosition + new Vector2(0, -10) * 0.3f / 0.2f;
        enemyStatus.lockonTargetPosition[4] = enemyStatus.lockonEffectPosition + new Vector2(0, 10) * 0.3f / 0.2f;

        // ショット
        if (count < 90) { }
        else if (count < 930)
        {
            if (playerStatus.drawingStatus.PositionScreen.y < drawingStatus.PositionScreen.y + 80)
            {
                // ペナルティ弾
                if (count % 6 == 0)
                {
                    float rand = 0.1f + gameStatus.rand.Next(360);
                    for (int i = 0; i < 10; i++)
                    {
                        float angle = 36 * i + rand;
                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                            Define.BulletImageType.MediumGreen,
                            drawingStatus.PositionScreen + new Vector2(0, 90),
                            6.0f,
                            angle);
                    }
                }
            }

            int c = count - 90;
            if (c < 60)
            {
                // 自機狙い弾に変化するやつ
                if (c % 8 == 0)
                {
                    int angle = 165 - 20 * c / 8;
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject b = Instantiate(bullet1);
                        b.GetComponent<BulletController>().Initialize(
                            Define.BulletImageType.BigBlue,
                            drawingStatus.PositionScreen + new Vector2(-42 * (float)System.Math.Pow(-1, i), 45),
                            2.5f,
                            angle);
                        b.GetComponent<BulletPattern1>().pattern = 0;
                    }
                }
            }
            else if (c < 210) { }
            else if(c < 270)
            {
                // ランダム弾に変化する奴
                c -= 210;
                if (c % 8 == 0)
                {
                    int angle = 15 + 20 * c / 8;
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject b = Instantiate(bullet1);
                        b.GetComponent<BulletController>().Initialize(
                            Define.BulletImageType.BigRed,
                            drawingStatus.PositionScreen + new Vector2(-42 * (float)System.Math.Pow(-1, i), 45),
                            2.5f,
                            angle);
                        b.GetComponent<BulletPattern1>().pattern = 1;
                    }
                }
            }
            else if (c < 420) { }
            else if (c < 480)
            {
                c -= 420;
                // 自機狙い弾 + ランダム
                if (c % 5 == 0)
                {
                    int angle = 165 - 12 * c / 5;
                    for (int i = 0; i < 2; i++)
                    {
                        Define.BulletImageType imageType = Define.BulletImageType.BigBlue;
                        if (i % 2 == 1) imageType = Define.BulletImageType.BigRed;
                        GameObject b = Instantiate(bullet1);
                        b.GetComponent<BulletController>().Initialize(
                            imageType,
                            drawingStatus.PositionScreen + new Vector2(-42 * (float)System.Math.Pow(-1, i), 45),
                            2.5f,
                            angle);
                        b.GetComponent<BulletPattern1>().pattern = i % 2;
                    }
                }
            }
            else if (c < 630) { }
            else if (c < 690)
            {
                c -= 630;
                // 自機狙い弾 + ランダム の2層構造
                if (c % 5 == 0)
                {
                    int angle = 15 + 12 * c / 5;
                    for (int i = 0; i < 2; i++)
                    {
                        Define.BulletImageType imageType = Define.BulletImageType.BigBlue;
                        if (i % 2 == 1) imageType = Define.BulletImageType.BigRed;
                        for (int j = 0; j < 2; j++)
                        {
                            GameObject b = Instantiate(bullet1);
                            b.GetComponent<BulletController>().Initialize(
                                imageType,
                                drawingStatus.PositionScreen + new Vector2(-42 * (float)System.Math.Pow(-1, j), 45),
                                2.5f + 2.5f * ((j + i) % 2),
                                angle);
                            b.GetComponent<BulletPattern1>().pattern = i % 2;
                        }
                    }
                }
            }
        }
        else
        {
            int c = count - 930;
            if (c % 120 < 20)
            {
                if (c % 5 == 0)
                {
                    // 真下にまっすぐに弾を撃つ
                    for (int i = 0; i < 2; i++)
                    {
                        GameObject bullet = Instantiate(bullet0);
                        bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumPurple, drawingStatus.PositionScreen + new Vector2(-42 + 84 * i, 45), 5.0f, 90);
                    }
                }
            }
        }
    }
}
