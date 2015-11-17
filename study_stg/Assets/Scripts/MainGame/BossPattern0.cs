using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ボス
/// </summary>
public class BossPattern0 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>目的位置</summary>
    private Vector2 destination;

    /// <summary>初期位置</summary>
    private Vector2 start;

    /// <summary>ボスの各部位</summary>
    public GameObject[] partObject;

    /// <summary>ボスの中心とボスの各部位との座標差</summary>
    private Vector2[] partDeltaPosition;

    /// <summary>部分破壊された場所から上がる炎</summary>
    public GameObject destroyPartEffect;

    /// <summary>HPゲージのスプライト</summary>
    public Sprite gageSprite;

    /// <summary>UIImageのプレハブ</summary>
    public GameObject UIImagePrefab;

    /// <summary>HPゲージ</summary>
    private UIImage[] gages;

    /// <summary>HPゲージに表示するLife値</summary>
    private int displayLife;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        partDeltaPosition = new Vector2[partObject.Length];

        // HPゲージに関して
        gages = new UIImage[enemyStatus.life / (playerStatus.lockonMaxNum * playerStatus.laserPower) + 1];
        for (int i = 0; i < gages.Length; i++)
        {
            gages[i] = Instantiate(UIImagePrefab).GetComponent<UIImage>();
            gages[i].gameObject.transform.SetParent(GameObject.Find("UI Canvas").transform);
            gages[i].Init(gageSprite, new Vector2(0, 0), new Vector2(0, 0));
            float value = 0.9f * i / (enemyStatus.maxLife / (playerStatus.lockonMaxNum * playerStatus.laserPower));
            gages[i].Blend = new Color(0.9f - value, 2 * value, value);
        }
    }


    void Update()
    {
        int count = enemyStatus.count;

        // 移動
        if (count == 0)
        {
            for (int i = 0; i < partObject.Length; i++)
            {
                Vector2 p = partObject[i].GetComponent<DrawingStatus>().PositionScreen;
                partObject[i].GetComponent<EnemyController>().Initialize(p);
                partDeltaPosition[i] = p - drawingStatus.PositionScreen;
            }
            start = drawingStatus.PositionScreen;
            destination = new Vector2(Define.GameScreenCenterX, 170);
        }
        else if (count < 90)
        {
            Vector2 deltaPos = start - destination;
            Vector2 a = deltaPos / (90 * 90);
            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((90 - count), 2);
        }
        else if (count == 90)
        {
            enemyStatus.speed = 0.2f;
        }
        else
        {
            enemyStatus.angle = count;
        }

        // ショット
        if (count == 90)
        {
            // 自機狙い弾を撃つ
            Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
            float dx = playerPos.x - drawingStatus.PositionScreen.x;
            float dy = playerPos.y - drawingStatus.PositionScreen.y;
            float angle = (float)System.Math.Atan2((float)dy, (float)dx) * 180.0f / (float)System.Math.PI;
            GameObject bullet = Instantiate(bullet0);
            bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumGreen, drawingStatus.PositionScreen, 4.0f, angle);
        }

        // 部分破壊のエフェクト
        for (int i = 0; i < partObject.Length; i++)
        {
            if (partObject[i] == null)
            {
                if (i < 2)
                {
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(0, 10),
                        -115 + gameStatus.rand.Next(50));
                }
                else if(i == 2)
                {
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(-20, 0),
                        -115 + gameStatus.rand.Next(50));
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(30, 20),
                        -115 + gameStatus.rand.Next(50));
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(-70, -20),
                        -115 + gameStatus.rand.Next(50));
                }
                else if (i == 3)
                {
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(20, 0),
                        -115 + gameStatus.rand.Next(50));
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(70, -20),
                        -115 + gameStatus.rand.Next(50));
                    Instantiate(destroyPartEffect).GetComponent<DestroyPartEffect>().Initialize(
                        drawingStatus.PositionScreen + partDeltaPosition[i] + new Vector2(gameStatus.rand.Next(10), gameStatus.rand.Next(10)) + new Vector2(-30, 20),
                        -115 + gameStatus.rand.Next(50));
                }
            }
        }

        // HPゲージの変動
        if (count < 120 && displayLife < enemyStatus.life)
        {
            displayLife = enemyStatus.maxLife * count / 120;
        }
        else
        {
            displayLife = enemyStatus.life;
        }
        UpdateLifeGage(displayLife);
    }


    void OnDestroy()
    {
        foreach (UIImage gage in gages)
        {
            Destroy(gage.gameObject);
        }
    }


    /// <summary>現在のLifeに合わせてHPゲージを変動させる</summary>
    void UpdateLifeGage(int life)
    {
        int remainLife = life;
        Vector2 min = new Vector2(40, 25);  // ゲージを表示する左上座標
        Vector2 max = new Vector2(Define.GameScreenSizeX - 20, 40);  // ゲージを表示する右下座標
        float perLifePos = (max - min).x / enemyStatus.maxLife;  // Life1当たりの座標値
        float perLaserPos = playerStatus.lockonMaxNum * playerStatus.laserPower * perLifePos;  // レーザー8本で削れるLife当たりの座標値
        for (int i = 0; i < gages.Length; i++)
        {
            Vector2 minPos = min + new Vector2(perLaserPos * i, 0);  // このゲージを表示する左上座標
            Vector2 maxPos = minPos + new Vector2(perLaserPos, 15);  // このゲージを表示する右下座標
            if (remainLife < 0)
            {
                maxPos = minPos;
            }
            else if (remainLife < playerStatus.lockonMaxNum * playerStatus.laserPower)
            {
                maxPos = minPos + new Vector2(remainLife * perLifePos, 15);
            }
            gages[i].Position = (minPos + maxPos) / 2.0f;
            gages[i].Size = maxPos - minPos;
            remainLife -= playerStatus.lockonMaxNum * playerStatus.laserPower;
        }
    }
}
