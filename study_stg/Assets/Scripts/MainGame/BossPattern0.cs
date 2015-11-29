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

    /// <summary>発狂弾</summary>
    public GameObject bullet2;

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

    /// <summary>ボスの各部位の当たり判定</summary>
    public BoxCollider2D[] partCollision;

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

    /// <summary>現在の攻撃パターン</summary>
    private int pattern;

    /// <summary>現在の攻撃ランク(パターンが一周するたびに1増える)</summary>
    private int rank;

    /// <summary>現在の攻撃パターンが開始してからのフレーム数</summary>
    private int patternCount;

    /// <summary>黄色ヘリ</summary>
    public GameObject enemy8;

    /// <summary>0なら左，1なら右に寄る</summary>
    private int movePattern;

    /// <summary>発狂状態か(パーツが全て破壊されたか)</summary>
    private bool isCrazy;

    /// <summary>ボス撃破時のエフェクトのプレハブ</summary>
    public GameObject destroyBossEffectPrefab;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        partDeltaPosition = new Vector2[partObject.Length];

        // HPゲージに関して
        gages = new UIImage[enemyStatus.life / (playerStatus.lockonMaxNum * playerStatus.laserPower) + 2];
        for (int i = 0; i < gages.Length; i++)
        {
            if (i == 0)
            {
                gages[i] = Instantiate(UIImagePrefab).GetComponent<UIImage>();
                gages[i].gameObject.transform.SetParent(GameObject.Find("UI Canvas").transform);
                gages[i].Init(gageSprite, new Vector2(0, 0), new Vector2(0, 0));
                gages[i].Blend = new Color(0.2f, 0, 0);
            }
            else
            {
                gages[i] = Instantiate(UIImagePrefab).GetComponent<UIImage>();
                gages[i].gameObject.transform.SetParent(GameObject.Find("UI Canvas").transform);
                gages[i].Init(gageSprite, new Vector2(0, 0), new Vector2(0, 0));
                float value = 0.4f * (i - 1) / (enemyStatus.maxLife / (playerStatus.lockonMaxNum * playerStatus.laserPower));
                gages[i].Blend = new Color(0.6f + value, 0.6f + value, 0.1f);
            }
        }
    }


    void Update()
    {
        Move();
        Shot();

        // 発狂状態への移行
        if (!isCrazy)
        {
            isCrazy = true;
            for (int i = 0; i < partObject.Length; i++)
            {
                if (partObject[i] != null)
                {
                    isCrazy = false;
                }
                else
                {
                    partCollision[i].enabled = true;
                }
            }
            if (isCrazy)
            {
                pattern = 7;
                patternCount = 0;
            }
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
        int count = enemyStatus.count;
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


    /// <summary>
    ///   移動に関する制御
    /// </summary>
    void Move()
    {
        int count = enemyStatus.count;
        int c = patternCount;

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
        else
        {
            switch (pattern)
            {
                case 0:
                    enemyStatus.speed = 0.2f;
                    enemyStatus.angle = count;
                    break;
                case 1:
                    enemyStatus.angle = 180;
                    enemyStatus.speed = 1.4f * (float)System.Math.Cos(1.5f * c * Mathf.Deg2Rad);
                    break;
                case 2:
                    enemyStatus.speed = 0.2f;
                    enemyStatus.angle = count;
                    break;
                case 3:
                    if (c == 0)
                    {
                        movePattern = gameStatus.rand.Next(2);
                        enemyStatus.speed = 1.0f;
                        enemyStatus.angle = 180 * movePattern;
                    }
                    else if (c < 120) { }
                    else
                    {
                        enemyStatus.speed = 0.1f;
                        enemyStatus.angle = count;
                    }
                    break;
                case 4:
                    if (c == 0)
                    {
                        start = drawingStatus.PositionScreen;
                        destination = new Vector2(start.x, Define.GameScreenSizeY);
                        enemyStatus.speed = 0.0f;
                    }
                    else if (c < 150)
                    {
                        Vector2 deltaPos = start - destination;
                        Vector2 a = deltaPos / (150 * 150);
                        drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((150 - c), 2);
                    }
                    else
                    {
                        enemyStatus.speed = 0.2f;
                        enemyStatus.angle = count;
                    }
                    break;
                case 5:
                    if (c == 0)
                    {
                        enemyStatus.speed = 2.0f;
                        enemyStatus.angle = 180 * (movePattern + 1);
                    }
                    else if (c < 120) { }
                    else
                    {
                        enemyStatus.speed = 0.2f;
                        enemyStatus.angle = count;
                    }
                    break;
                case 6:
                    if (rank == 0)
                    {
                        if (c == 0)
                        {
                            start = drawingStatus.PositionScreen;
                            destination = new Vector2(start.x, 170);
                            enemyStatus.speed = 0.0f;
                        }
                        else if (c < 150)
                        {
                            Vector2 deltaPos = start - destination;
                            Vector2 a = deltaPos / (150 * 150);
                            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((150 - c), 2);
                        }
                        else if (c == 150)
                        {
                            start = drawingStatus.PositionScreen;
                            destination = new Vector2(Define.GameScreenCenterX, 170);
                        }
                        else
                        {
                            Vector2 deltaPos = start - destination;
                            Vector2 a = deltaPos / (120 * 120);
                            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((270 - c), 2);
                        }
                    }
                    else if (rank == 1)
                    {
                        if (c == 0)
                        {
                            start = drawingStatus.PositionScreen;
                            destination = new Vector2(start.x, -200);
                            enemyStatus.speed = 0.0f;
                        }
                        else if (c < 240)
                        {
                            Vector2 deltaPos = start - destination;
                            Vector2 a = deltaPos / (240 * 240);
                            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((240 - c), 2);
                        }
                    }
                    break;
                case 7:
                    if (c == 0)
                    {
                        start = drawingStatus.PositionScreen;
                        destination = new Vector2(Define.GameScreenCenterX, 170);
                        enemyStatus.speed = 0.0f;
                    }
                    else if (c < 90)
                    {
                        Vector2 deltaPos = start - destination;
                        Vector2 a = deltaPos / (90 * 90);
                        drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((90 - c), 2);
                    }
                    else
                    {
                        enemyStatus.speed = 1.0f + 3.0f * Mathf.Abs(Mathf.Cos(2 * (c - 90) * Mathf.Deg2Rad)) * Mathf.Pow(-1, (c - 90) / 180);
                        enemyStatus.angle = 2 * c;
                    }
                    break;
            }
        }
    }


    /// <summary>
    ///   ショットに関する制御
    /// </summary>
    void Shot()
    {
        if (enemyStatus.count < 90) { }
        else if (enemyStatus.count == 90)
        {
            pattern = 0;
            patternCount = 0;
        }
        else
        {
            int c = patternCount;
            Vector2 pos = drawingStatus.PositionScreen;
            bool patternEnd = false;

            switch (pattern)
            {
                // メイン主砲: 左右交互にway弾　本体: 真下に数発撃つ
                case 0:
                    if (c < 360)
                    {
                        if (c >= 120)
                        {
                            if (rank == 0)
                            {
                                if (c % 120 < 20 && c % 5 == 0)
                                {
                                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                        Define.BulletImageType.MediumPurple, pos + new Vector2(0, 90), 5.0f, 90.0f);
                                }
                            }
                            else if (rank == 1)
                            {
                                if (c % 120 < 40 && c % 5 == 0)
                                {
                                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                        Define.BulletImageType.MediumPurple, pos + new Vector2(0, 90), 5.0f, 90.0f);
                                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                        Define.BulletImageType.MediumPurple, pos + new Vector2(0, 70), 5.0f, 130.0f);
                                    Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                        Define.BulletImageType.MediumPurple, pos + new Vector2(0, 110), 5.0f, 50.0f);
                                }
                            }
                        }
                        if (partObject[0] != null)
                        {
                            if (c % 80 == 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    int num = 2 * (c / 80) + 1 + rank * 2;
                                    WayShot(pos + new Vector2(-58, 40), Define.BulletImageType.MediumGreen, 2.0f + i, num, 210.0f / num);
                                }
                            }
                        }
                        if (partObject[1] != null)
                        {
                            if (c % 80 == 40)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    int num = 2 * (c / 80) + 1 + rank * 2;
                                    WayShot(pos + new Vector2(58, 40), Define.BulletImageType.MediumGreen, 2.0f + i, num, 210.0f / num);
                                }
                            }
                        }
                    }
                    else if (c < 420) { }
                    else patternEnd = true;
                    break;

                // サブ主砲: 上下に落とす弾，機体が横に大きく揺れる
                case 1:
                    if (rank == 1)
                    {
                        if (enemyStatus.count % 120 < 20 && enemyStatus.count % 5 == 0)
                        {
                            WayShot(pos + new Vector2(0, 90), Define.BulletImageType.MediumGreen, 5.0f, 3, 70.0f);
                        }
                    }
                    if (c < 640)
                    {
                        if (c % 20 == 0)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (partObject[2 + j] != null)
                                    {
                                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                            Define.BulletImageType.MediumPurple,
                                            pos + new Vector2((-280 + 40 * i) * (float)System.Math.Pow(-1, j), -90),
                                            3.0f,
                                            -90);
                                        if (rank == 1)
                                        {
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                Define.BulletImageType.MediumPurple,
                                                pos + new Vector2((-280 + 40 * i) * (float)System.Math.Pow(-1, j), -90),
                                                4.0f,
                                                -90);
                                        }
                                    }
                                }
                            }
                        }
                        if (c % 40 == 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (partObject[2 + j] != null)
                                    {
                                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                 Define.BulletImageType.MediumPurple,
                                                 pos + new Vector2((-30 - 80 * i) * (float)System.Math.Pow(-1, j), -18 * i),
                                                 2.0f,
                                                 90);
                                        if (rank == 1)
                                        {
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                     Define.BulletImageType.MediumPurple,
                                                     pos + new Vector2((-30 - 80 * i) * (float)System.Math.Pow(-1, j), -18 * i),
                                                     3.0f,
                                                     90);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else if (c < 700) { }
                    else patternEnd = true;
                    break;

                // 黄色ヘリ召喚: 自機狙いを撃ちつつ近づく．倒すと撃ち返しで全方位　メイン主砲:数体ヘリを召喚したら矢じり形の回転砲台
                case 2:
                    if (rank == 1)
                    {
                        if (enemyStatus.count % 120 < 30 && enemyStatus.count % 5 == 0)
                        {
                            WayShot(pos + new Vector2(0, 90), Define.BulletImageType.MediumGreen, 5.0f, 3, 70.0f);
                        }
                    }
                    if (c < 620)
                    {
                        if (c >= 120)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                if (c % 30 == 15 * k)
                                {
                                    if (partObject[k] != null)
                                    {
                                        int sign = (int)System.Math.Pow(-1, k);
                                        for (int i = 0; i < 3 + rank; i++)
                                        {
                                            for (int j = 0; j < i + 1; j++)
                                            {
                                                float wayAngle = 5.0f;
                                                float angle = 2 * enemyStatus.count * sign;
                                                if (i % 2 == 0)
                                                {
                                                    angle += wayAngle / 2;
                                                    if (j % 2 == 0) angle -= wayAngle * (j / 2);
                                                    else angle += wayAngle * (j / 2 + 1);
                                                }
                                                else
                                                {
                                                    if (j % 2 == 0) angle -= wayAngle * (j / 2);
                                                    else angle += wayAngle * (j / 2 + 1);
                                                }
                                                Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                         Define.BulletImageType.MediumPurple,
                                                         pos + new Vector2(-58 * sign, 40),
                                                         4.0f - 0.5f * i,
                                                         angle);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < 2; k++)
                        {
                            if (c % 120 == 60 * k)
                            {
                                if (partObject[2 + k] != null)
                                {
                                    Instantiate(enemy8).GetComponent<EnemyController>().Initialize(pos + partDeltaPosition[2 + k]);
                                }
                            }
                        }
                    }
                    else patternEnd = true;
                    break;

                // 体当たり準備: 左右のどちらかに少し寄る　メイン主砲:矢じり形の回転砲台　サブ主砲:後ろに弾を飛ばし続ける
                case 3:
                    if (rank == 1)
                    {
                        if (enemyStatus.count % 120 < 30 && enemyStatus.count % 5 == 0)
                        {
                            WayShot(pos + new Vector2(0, 90), Define.BulletImageType.MediumGreen, 5.0f, 3, 70.0f);
                        }
                    }
                    if (c < 180)
                    {
                        if (enemyStatus.count % 40 == 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (partObject[2 + j] != null)
                                    {
                                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                            Define.BulletImageType.MediumPurple,
                                            pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                            2.0f,
                                            -90);
                                        if (rank == 1)
                                        {
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                Define.BulletImageType.MediumPurple,
                                                pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                                3.0f,
                                                -90);
                                        }
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < 2; k++)
                        {
                            if (c % 30 == 15 * k)
                            {
                                if (partObject[k] != null)
                                {
                                    int sign = (int)System.Math.Pow(-1, k);
                                    for (int i = 0; i < 3 + rank; i++)
                                    {
                                        for (int j = 0; j < i + 1; j++)
                                        {
                                            float wayAngle = 5.0f;
                                            float angle = 2 * enemyStatus.count * sign;
                                            if (i % 2 == 0)
                                            {
                                                angle += wayAngle / 2;
                                                if (j % 2 == 0) angle -= wayAngle * (j / 2);
                                                else angle += wayAngle * (j / 2 + 1);
                                            }
                                            else
                                            {
                                                if (j % 2 == 0) angle -= wayAngle * (j / 2);
                                                else angle += wayAngle * (j / 2 + 1);
                                            }
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                     Define.BulletImageType.MediumPurple,
                                                     pos + new Vector2(-58 * sign, 40),
                                                     4.0f - 0.5f * i,
                                                     angle);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else patternEnd = true;
                    break;

                // 体当たり: 画面下まで前進．本体にロックオン出来ないところまで　サブ主砲:後ろに弾を飛ばし続ける
                case 4:
                    if (c < 270)
                    {
                        if (enemyStatus.count % 40 == 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (partObject[2 + j] != null)
                                    {
                                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                            Define.BulletImageType.MediumPurple,
                                            pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                            2.0f,
                                            -90);
                                        if (rank == 1)
                                        {
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                Define.BulletImageType.MediumPurple,
                                                pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                                3.0f,
                                                -90);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else patternEnd = true;
                    break;

                // サブ主砲:後ろに弾を飛ばし続ける　本体:さっき寄ってない方に寄る
                case 5:
                    if (c < 180)
                    {
                        if (enemyStatus.count % 40 == 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (partObject[2 + j] != null)
                                    {
                                        Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                            Define.BulletImageType.MediumPurple,
                                            pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                            2.0f,
                                            -90);
                                        if (rank == 1)
                                        {
                                            Instantiate(bullet0).GetComponent<BulletController>().Initialize(
                                                Define.BulletImageType.MediumPurple,
                                                pos + new Vector2((-280 + 80 * i) * (float)System.Math.Pow(-1, j), -90),
                                                3.0f,
                                                -90);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else patternEnd = true;
                    break;

                // 戻る: 真後ろに戻ってから真中付近に位置する．その後，最初の攻撃に戻る．二周目の場合は逃げる
                case 6:
                    if (rank == 1)
                    {
                        if (enemyStatus.count % 2 == 0)
                        {
                            WayShot(pos + new Vector2(0, 90), Define.BulletImageType.MediumGreen, 7.0f, 7, 20.0f);
                        }
                    }
                    if (c < 270) { }
                    else patternEnd = true;
                    break;

                // 発狂: ぐるぐる回りながら分裂弾を撒く
                case 7:
                    if (c % 4 == 0)
                    {
                        float angle = -5.0f * enemyStatus.count;
                        float angle2 = 2.0f * enemyStatus.count;
                        GameObject obj = Instantiate(bullet2);
                        obj.GetComponent<BulletController>().Initialize(
                            Define.BulletImageType.BigRed,
                            pos + new Vector2(0, 90),
                            3.0f,
                            angle);
                        obj.GetComponent<BulletPattern2>().angle = angle2;
                    }
                    break;
            }

            if (patternEnd)
            {
                pattern = (pattern + 1) % 7;
                if (pattern == 0)
                {
                    rank += 1;
                }
                patternCount = 0;
            }
            else
            {
                patternCount += 1;
            }
        }
    }


    /// <summary>
    ///   way弾を出す
    /// </summary>
    /// <param name="pos">発射地点</param>
    /// <param name="image">弾の画像</param>
    /// <param name="speed">弾の速さ</param>
    /// <param name="wayNum">wayの数</param>
    /// <param name="wayAngle">way同士の間の角度(度)</param>
    void WayShot(Vector2 pos, Define.BulletImageType image, float speed, int wayNum, float wayAngle)
    {
        Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
        float dx = playerPos.x - pos.x;
        float dy = playerPos.y - pos.y;
        float baseAngle = (float)System.Math.Atan2((float)dy, (float)dx) * 180.0f / (float)System.Math.PI;
        for (int i = 0; i < wayNum; i++)
        {
            float angle = baseAngle;
            if (wayNum % 2 == 0)
            {
                angle -= wayAngle / 2.0f;
                if (i % 2 == 0)
                {
                    angle -= wayAngle * (i / 2);
                }
                else
                {
                    angle += wayAngle * (i / 2 + 1);
                }
            }
            else
            {
                if (i % 2 == 0)
                {
                    angle -= wayAngle * (i / 2);
                }
                else
                {
                    angle += wayAngle * (i / 2 + 1);
                }
            }
            GameObject bullet = Instantiate(bullet0);
            bullet.GetComponent<BulletController>().Initialize(image, pos, speed, angle);
        }
    }


    void OnDestroy()
    {
        // HPゲージを消す
        foreach (UIImage gage in gages)
        {
            if (gage != null)
            {
                Destroy(gage.gameObject);
            }
        }

        if (enemyStatus.life <= 0)
        {
            // 画面上の敵機，敵弾を消す
            foreach (Transform child in GameObject.Find("Enemies").transform)
            {
                child.GetComponent<EnemyController>().Disappear();
            }
            foreach (Transform child in GameObject.Find("Bullets").transform)
            {
                child.GetComponent<BulletController>().Disappear();
            }

            // ボスの撃破エフェクトを出す
            Instantiate(destroyBossEffectPrefab).GetComponent<DestroyBossEffect>().Initialize(
                drawingStatus.PositionScreen,
                5000);
        }

        // 結果発表カウントを進める
        if (enemyStatus.life <= 0)
        {
            gameStatus.resultCount = 60;
        }
        else
        {
            gameStatus.resultCount = 1;
        }
    }


    /// <summary>現在のLifeに合わせてHPゲージを変動させる</summary>
    void UpdateLifeGage(int life)
    {
        int remainLife = life;
        Vector2 min = new Vector2(40, 25);  // ゲージを表示する左上座標
        Vector2 max = new Vector2(Define.GameScreenSizeX - 20, 35);  // ゲージを表示する右下座標
        float perLifePos = (max - min).x / enemyStatus.maxLife;  // Life1当たりの座標値
        float perLaserPos = playerStatus.lockonMaxNum * playerStatus.laserPower * perLifePos;  // レーザー8本で削れるLife当たりの座標値
        if (life < enemyStatus.maxLife && enemyStatus.life == life)
        {
            gages[0].Position = (min + max) / 2;
            gages[0].Size = max - min;
        }
        for (int i = 1; i < gages.Length; i++)
        {
            Vector2 minPos = min + new Vector2(perLaserPos * (i - 1), 0);  // このゲージを表示する左上座標
            Vector2 maxPos = minPos + new Vector2(perLaserPos, 10);  // このゲージを表示する右下座標
            if (remainLife < 0)
            {
                maxPos = minPos;
            }
            else if (remainLife < playerStatus.lockonMaxNum * playerStatus.laserPower)
            {
                maxPos = minPos + new Vector2(remainLife * perLifePos, 10);
            }
            gages[i].Position = (minPos + maxPos) / 2.0f;
            gages[i].Size = maxPos - minPos;
            remainLife -= playerStatus.lockonMaxNum * playerStatus.laserPower;
        }
    }
}
