using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   少し進んだ後爆発して，そこから弾が出るタイプ
/// </summary>
public class BulletPattern1 : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    ///<summary>アタッチされているBulletStatusスクリプト</summary>
    private BulletStatus bulletStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;

    /// <summary>元々の弾の速度</summary>
    private float originalSpeed;

    /// <summary>まっすぐに飛ぶ弾</summary>
    public GameObject bullet0;

    /// <summary>爆発後，どのような弾に変化するか</summary>
    public int pattern;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        bulletStatus = GetComponent<BulletStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
    }


    void Update()
    {
        int count = bulletStatus.count;

        if (count == 0)
        {
            originalSpeed = bulletStatus.speed;
        }
        if (count < 60)
        {
            bulletStatus.speed = originalSpeed * (60 - count) / 60.0f;
        }
        else if (count == 60)
        {
            if (pattern == 0)
            {
                Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
                float deltaAngle = (float)System.Math.Atan2(playerPos.y - drawingStatus.PositionScreen.y, playerPos.x - drawingStatus.PositionScreen.x) * Mathf.Rad2Deg;
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                        Define.BulletImageType.MediumGreen,
                        drawingStatus.PositionScreen,
                        3.0f,
                        deltaAngle + 120 * i);
                }
            }
            else if(pattern == 1)
            {
                float rand = 0.1f * gameStatus.rand.Next(3600);
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                        Define.BulletImageType.MediumPurple,
                        drawingStatus.PositionScreen,
                        2.5f,
                        120 * i + rand);
                }
            }
            Destroy(gameObject);
        }
    }
}
