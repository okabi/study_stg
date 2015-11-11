using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   めっちゃ弱い敵(赤色ヘリ)
///   動き: 画面左(右)前方に向かった後，自機の方に突撃する
///   ショット: たまに自機狙い弾を撃つ
/// </summary>
public class EnemyPattern4 : MonoBehaviour
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
        if (count == 0)
        {
            destination = new Vector2(drawingStatus.PositionScreen.x - 300 * (float)System.Math.Pow(-1, movePattern), 100);
            modelAngle = 90.0f;
            enemyStatus.speed = 8.0f;
            Vector2 deltaPos = destination - drawingStatus.PositionScreen;
            enemyStatus.angle = (float)System.Math.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        }
        else if (count < 80)
        {
            enemyStatus.speed -= 0.1f;
        }
        else if (count < 120) { }
        else if (count == 120)
        {
            Vector2 deltaPos = playerStatus.drawingStatus.PositionScreen - drawingStatus.PositionScreen;
            enemyStatus.angle = (float)System.Math.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
        }
        else if (count < 150)
        {
            enemyStatus.speed += 0.05f;
        }

        // モデル回転
        Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
        float deltaAngle = (float)System.Math.Atan2(playerPos.y - drawingStatus.PositionScreen.y, playerPos.x - drawingStatus.PositionScreen.x) * Mathf.Rad2Deg;
        transform.Rotate(0.0f, -(modelAngle - deltaAngle), 0.0f);
        modelAngle -= modelAngle - deltaAngle;
        rotorControl.transform.Rotate(0.0f, 0.0f, 15.0f);

        // ショット
        if (count == 90)
        {
            // 自機狙い弾を撃つ
            float dx = playerPos.x - drawingStatus.PositionScreen.x;
            float dy = playerPos.y - drawingStatus.PositionScreen.y;
            float angle = (float)System.Math.Atan2((float)dy, (float)dx) * 180.0f / (float)System.Math.PI;
            GameObject bullet = Instantiate(bullet0);
            bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumGreen, drawingStatus.PositionScreen, 4.0f, angle);
        }
    }
}
