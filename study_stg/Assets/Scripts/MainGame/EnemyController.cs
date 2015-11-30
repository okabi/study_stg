using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   敵の制御に関連するクラス．全ての敵に取り付ける
/// </summary> 
public class EnemyController : MonoBehaviour {
    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>アタッチされているEnemyStatusスクリプト</summary>
    public EnemyStatus enemyStatus;

    /// <summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>敵機破壊時のエフェクトのプレハブ</summary>
    public GameObject destroyEffectPrefab;

    /// <summary>UITextのプレハブ</summary>
    public GameObject UITextPrefab;

    /// <summary>倍率表示</summary>
    private UIOutlinedText multiplyEffect;

    /// <summary>スコア表示UIのプレハブ</summary>
    public GameObject UIScorePrefab;

    /// <summary>アタッチされているLifeGageスクリプト</summary>
    private LifeGage lifeGage;

    /// <summary>保存データ</summary>
    private SaveStatus saveStatus;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        lifeGage = GetComponent<LifeGage>();
        if (GameObject.Find("SaveController") != null)
        {
            saveStatus = GameObject.Find("SaveController").GetComponent<SaveStatus>();
        }

        // 親オブジェクトの設定
        if (transform.parent == null)
        {
            transform.SetParent(GameObject.Find("Enemies").transform);
        }

        // 初期設定
        enemyStatus.isDespawnable = false;  // 画面外に出ても消えない(初期座標が画面外なので)
        enemyStatus.lockonDamage = 0;  // プレイヤーにロックオンされていない
        enemyStatus.lockonMultiply = 1;  // プレイヤーにロックオンされていない
        enemyStatus.lockonEffectPosition = drawingStatus.PositionScreen;
        enemyStatus.lockonTargetPosition = new Vector2[1];
        enemyStatus.lockonTargetPosition[0] = drawingStatus.PositionScreen;
        enemyStatus.isDamage = false;  // 直前フレームでダメージを受けていない
    }
	
	
	void Update () {
        Move();
        Animation();
        OutOfScreen();
        enemyStatus.count += 1;
    }


    ///<summary>移動制御</summary>
    void Move()
    {
        float speed = enemyStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * enemyStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
        // 座標変位(スクリーン座標系)
        Vector2 deltaPosition = new Vector2(speed * (float)System.Math.Cos(radian), speed * (float)System.Math.Sin(radian));

        // 座標を更新
        drawingStatus.PositionScreen += deltaPosition;
        enemyStatus.lockonEffectPosition = drawingStatus.PositionScreen;
        enemyStatus.lockonTargetPosition[0] = enemyStatus.lockonEffectPosition;
        if (multiplyEffect != null)
        {
            multiplyEffect.Text = "x" + enemyStatus.lockonMultiply.ToString();
            multiplyEffect.Position = enemyStatus.lockonEffectPosition + new Vector2(20, -20);
        }

        // ライフゲージを更新
        if (lifeGage != null)
        {
            lifeGage.UpdateParameter(drawingStatus.PositionScreen + new Vector2(20.0f, -20.0f), enemyStatus.life, enemyStatus.maxLife);
        }
    }


    /// <summary>アニメーション制御</summary>
    void Animation()
    {
        float lp = (float)enemyStatus.life / enemyStatus.maxLife;
        int c = enemyStatus.count;

        if (enemyStatus.isDamage)
        {
            drawingStatus.Blend = new Color(0.1f, 0.1f, 1.0f);
        }
        else if (lp < 0.2f)
        {
            if (c % 10 < 5)
            {
                drawingStatus.Blend = new Color(1.0f, 0.1f, 0.1f);
            }
            else
            {
                drawingStatus.Blend = enemyStatus.originalBlend;
            }
        }
        else if (lp < 0.4f)
        {
            if (c % 20 < 5)
            {
                drawingStatus.Blend = new Color(1.0f, 0.1f, 0.1f);
            }
            else
            {
                drawingStatus.Blend = enemyStatus.originalBlend;
            }
        }
        else if (lp < 0.6f)
        {
            if (c % 30 < 5)
            {
                drawingStatus.Blend = new Color(1.0f, 0.1f, 0.1f);
            }
            else
            {
                drawingStatus.Blend = enemyStatus.originalBlend;
            }
        }
        else
        {
            drawingStatus.Blend = enemyStatus.originalBlend;
        }

        enemyStatus.isDamage = false;
    }


    /// <summary>画面外に出た時の処理</summary>
    void OutOfScreen()
    {
        // オブジェクトのパラメータ
        float x = drawingStatus.PositionScreen.x;
        float y = drawingStatus.PositionScreen.y;
        float scale = drawingStatus.Scale;
        float sizex = scale * 20.0f;
        float sizey = scale * 20.0f;

        // 領域のパラメータ
        Vector2 GameScreenMin = new Vector2(StudySTG.Define.GameScreenCenterX - StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY - StudySTG.Define.GameScreenSizeY / 2);
        Vector2 GameScreenMax = new Vector2(StudySTG.Define.GameScreenCenterX + StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY + StudySTG.Define.GameScreenSizeY / 2);

        // 画面外に完全に出ている
        if (x + sizex < GameScreenMin.x - 50 || x - sizex > GameScreenMax.x + 50 || y + sizey < GameScreenMin.y - 50 || y - sizey > GameScreenMax.y + 50)
        {
            if (enemyStatus.isDespawnable)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (!enemyStatus.isBossPart)
            {
                enemyStatus.isDespawnable = true;
            }
        }
    }


    /// <summary>
    ///   プレイヤーに当たった時に呼び出される
    /// </summary>
    /// <param name="other">プレイヤーの情報</param>
    void OnTriggerStay2D(Collider2D other)
    {
        other.SendMessage("Damage");
    }


    /// <summary>
    ///   オブジェクトが消滅するときに呼ばれる
    /// </summary>
    void OnDestroy()
    {
        if (multiplyEffect != null)
        {
            Destroy(multiplyEffect.gameObject);
        }
    }


    /// <summary>
    ///   ダメージを受ける
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    void Damage(int damage)
    {
        enemyStatus.life -= damage;
        enemyStatus.isDamage = true;
        if (damage != playerStatus.laserPower)
        {
            // メインショットの場合撃ち込み点を入れる
            playerStatus.score += damage * 20;
        }
        if (enemyStatus.life <= 0)
        {
            // スコア処理
            int score;
            if (damage == playerStatus.laserPower)
            {
                score = enemyStatus.score * enemyStatus.lockonMultiply;
                playerStatus.score += score;
                if (saveStatus.replaying)
                {
                    Instantiate(UIScorePrefab).GetComponent<ScoreController>().Initialize(enemyStatus.lockonEffectPosition + new Vector2(0, 20), score);
                }
            }
            else
            {
                score = enemyStatus.score;
                playerStatus.score += enemyStatus.score;
                if (saveStatus.replaying)
                {
                    Instantiate(UIScorePrefab).GetComponent<ScoreController>().Initialize(enemyStatus.lockonEffectPosition + new Vector2(0, 20), score);
                }
            }
            // 死亡時のエフェクト
            for (int i = 0; i < 2 * score / 100; i++)
            {
                Vector2 pos = drawingStatus.PositionScreen;
                pos += new Vector2(-20 + gameStatus.rand.Next(41), -20 + gameStatus.rand.Next(41));
                float angle = 0.1f * gameStatus.rand.Next(3600);
                Instantiate(destroyEffectPrefab).GetComponent<DestroyEffect>().Initialize(
                    pos,
                    angle,
                    score,
                    new Color(1.0f, 0.2f, 0.2f));
            }
            Destroy(gameObject);
        }
    }


    /// <summary>
    ///   各パラメータを設定．敵生成時に呼び出すこと
    /// </summary>
    /// <param name="position">スクリーン座標</param>
    public void Initialize(Vector2 position)
    {
        drawingStatus.PositionScreen = position;
        enemyStatus.lockonEffectPosition = position;
        enemyStatus.lockonTargetPosition[0] = position;
        enemyStatus.originalBlend = drawingStatus.Blend;
        playerStatus.enemyController.Add(this);
    }


    /// <summary>
    ///   プレイヤーがこの敵をロックオンしようとする．ロックオンするかを返す．
    ///   ロックオンは，この敵が現在のレーザーロックオン数で倒しきれない場合成される．
    /// </summary>
    /// <param name="playerPos">プレイヤーのスクリーン座標</param>
    /// <param name="radius">ロックオン半径</param>
    /// <param name="laserPower">ホーミングレーザーの威力</param>
    /// <param name="multiply">かける倍率</param>
    /// <returns>新たにロックオンするか</returns>
    public bool Lockon(Vector2 playerPos, float radius, int laserPower, int multiply)
    {
        bool retval = false;
        if (enemyStatus.lockonDamage >= enemyStatus.life)
        {
            retval = false;
        }
        else
        {
            foreach (Vector2 pos in enemyStatus.lockonTargetPosition)
            {
                float sx = playerPos.x - pos.x;
                float sy = playerPos.y - pos.y;
                float d = (float)System.Math.Sqrt(sx * sx + sy * sy);
                if (d < radius)
                {
                    enemyStatus.lockonDamage += laserPower;
                    enemyStatus.lockonMultiply = multiply;
                    if (multiplyEffect != null)
                    {
                        Destroy(multiplyEffect.gameObject);
                    }
                    multiplyEffect = Instantiate(UITextPrefab).GetComponent<UIOutlinedText>();
                    multiplyEffect.gameObject.transform.SetParent(GameObject.Find("Game Canvas").transform);
                    multiplyEffect.Init(
                        "x" + multiply.ToString(),
                        enemyStatus.lockonEffectPosition + new Vector2(20, -20),
                        new Color(1, 1, 1),
                        new Color(0, 0, 0),
                        16);
                    retval = true;
                    break;
                }
            }
        }
        return retval;
    }


    /// <summary>この敵のロックオンを解除する</summary>
    public void LockonReset()
    {
        enemyStatus.lockonDamage = 0;
        if (multiplyEffect != null)
        {
            Destroy(multiplyEffect.gameObject);
        }
    }


    /// <summary>
    ///   弾消し効果で消滅する
    /// </summary>
    public void Disappear()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 pos = drawingStatus.PositionScreen;
            float angle = 0.1f * gameStatus.rand.Next(3600);
            Instantiate(destroyEffectPrefab).GetComponent<DestroyEffect>().Initialize(
                pos,
                angle,
                0,
                new Color(1.0f, 1.0f, 0.2f));
        }
        Destroy(gameObject);
    }

}
