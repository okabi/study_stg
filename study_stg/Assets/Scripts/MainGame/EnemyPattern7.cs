using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   めっちゃ弱い敵(赤色ヘリ)
///   動き: 左(右)から登場した後，円弧を描いて上方へ退散する
///   ショット: 自機狙いを撃つ
/// </summary>
public class EnemyPattern7 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    /// <summary>ヘリのローター</summary>
    public GameObject rotorControl;

    /// <summary>敵モデルのy軸方向の回転角度</summary>
    public float modelAngle;

    /// <summary>0なら左から，1なら右から登場</summary>
    public int movePattern;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;


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
            modelAngle = 90.0f;
            enemyStatus.speed = 2.0f;
            enemyStatus.angle = 180 * movePattern;
        }
        else if (count < 120) { }
        else if (count < 180)
        {
            if (movePattern == 0)
            {
                enemyStatus.angle -= 2.0f;
            }
            else
            {
                enemyStatus.angle += 2.0f;
            }
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
            bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumGreen, drawingStatus.PositionScreen, 4.0f, angle, enemyStatus.tag);
        }
    }
}
