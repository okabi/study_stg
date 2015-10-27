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
    private EnemyStatus enemyStatus;

    /// <summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>敵機破壊時のエフェクトのプレハブ</summary>
    public GameObject destroyEffectPrefab;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();

        // 親オブジェクトの設定
        transform.SetParent(GameObject.Find("Enemies").transform);

        // 初期設定
        enemyStatus.isDespawnable = false;  // 画面外に出ても消えない(初期座標が画面外なので)
        enemyStatus.isLockon = false;  // プレイヤーにロックオンされていない
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
                drawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
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
                drawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
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
                drawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            drawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
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
        if (x + sizex < GameScreenMin.x || x - sizex > GameScreenMax.x || y + sizey < GameScreenMin.y || y - sizey > GameScreenMax.y)
        {
            if (enemyStatus.isDespawnable)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            enemyStatus.isDespawnable = true;
        }
    }


    /// <summary>
    ///   プレイヤーに当たった時に呼び出される
    /// </summary>
    /// <param name="other">プレイヤーの情報</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("Damage");
    }


    /// <summary>
    ///   ダメージを受ける
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    void Damage(int damage)
    {
        enemyStatus.life -= damage;
        enemyStatus.isDamage = true;
        if (enemyStatus.life <= 0)
        {
            // 死亡時のエフェクト，スコア処理
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos = drawingStatus.PositionScreen;
                pos += new Vector2(-20 + gameStatus.rand.Next(41), -20 + gameStatus.rand.Next(41));
                float angle = 0.1f * gameStatus.rand.Next(3600);
                Instantiate(destroyEffectPrefab).GetComponent<DestroyEffectController>().Initialize(
                    pos,
                    angle);
            }
            playerStatus.score += enemyStatus.score;
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
        playerStatus.enemyController.Add(this);
    }


    /// <summary>
    ///   プレイヤーがこの敵をロックオンする
    /// </summary>
    /// <param name="playerPos">プレイヤーのスクリーン座標</param>
    /// <param name="radius">ロックオン半径</param>
    /// <returns>新たにロックオンするか</returns>
    public bool Lockon(Vector2 playerPos, float radius)
    {
        bool retval;
        if (enemyStatus.isLockon)
        {
            retval = false;
        }
        else
        {
            float sx = playerPos.x - drawingStatus.PositionScreen.x;
            float sy = playerPos.y - drawingStatus.PositionScreen.y;
            float d = (float)System.Math.Sqrt(sx * sx + sy * sy);
            if (d < radius)
            {
                enemyStatus.isLockon = true;
                retval = true;
            }
            else
            {
                retval = false;
            }
        }
        return retval;
    }


    /// <summary>この敵のロックオンを解除する</summary>
    public void LockonReset()
    {
        enemyStatus.isLockon = false;
    }
}
