using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   めっちゃ弱い敵(黄色ヘリ)
///   動き: 自機の方に突撃する
///   ショット: 撃ち返し
/// </summary>
public class EnemyPattern8 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    /// <summary>ヘリのローター</summary>
    public GameObject rotorControl;

    /// <summary>敵モデルのy軸方向の回転角度</summary>
    public float modelAngle;

    /// <summary>0なら左，1なら右に動く</summary>
    public int movePattern;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>敵機移動の目的地</summary>
    private Vector2 destination;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }


    void Update()
    {
        int count = enemyStatus.count;

        // 移動
        if (count < 60)
        {
            enemyStatus.angle = 90;
            enemyStatus.speed = 2.0f;
        }
        else if (count < 120)
        {
            Vector2 deltaPos = playerStatus.drawingStatus.PositionScreen - drawingStatus.PositionScreen;
            enemyStatus.angle = (float)System.Math.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
            enemyStatus.speed = 1.0f;
        }

        // モデル回転
        Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
        float deltaAngle = (float)System.Math.Atan2(playerPos.y - drawingStatus.PositionScreen.y, playerPos.x - drawingStatus.PositionScreen.x) * Mathf.Rad2Deg;
        transform.Rotate(0.0f, -(modelAngle - deltaAngle), 0.0f);
        modelAngle -= modelAngle - deltaAngle;
        rotorControl.transform.Rotate(0.0f, 0.0f, 15.0f);
    }


    /// <summary>
    ///   撃ち返し弾
    /// </summary>
    void OnDestroy()
    {
        if (enemyStatus.life <= 0)
        {
            float rand = GameObject.Find("GameController").GetComponent<GameStatus>().rand.Next(360);
            for (int i = 0; i < 3; i++)
            {
                float angle = rand + 120 * i;
                Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                    Define.BulletImageType.MediumGreen, drawingStatus.PositionScreen, 2.0f, angle, enemyStatus.tag);
            }
        }
    }
}
