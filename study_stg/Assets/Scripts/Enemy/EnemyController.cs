using UnityEngine;
using System.Collections;

/// <summary>
///   敵の制御に関連するクラス
/// </summary> 
public class EnemyController : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    protected DrawingStatus drawingStatus;

    ///<summary>アタッチされているEnemyStatusスクリプト</summary>
    protected EnemyStatus enemyStatus;

    ///<summary>プレイヤーの座標等の情報を持つDrawingStatusスクリプト</summary>
    protected DrawingStatus playerStatus;



    void Awake () {
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        enemyStatus.despawnable = false;
        playerStatus = GameObject.Find("Player").GetComponent<DrawingStatus>();
    }
	
	
	void FixedUpdate () {
        Pattern();
        Move();
        OutOfScreen();
        enemyStatus.count += 1;
    }


    /// <summary>
    ///   動きを決定する．
    /// </summary>
    protected virtual void Pattern(){}


    ///<summary>移動制御</summary>
    void Move()
    {
        float speed = enemyStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * enemyStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
        // 座標変位(スクリーン座標系)
        Vector2 deltaPosition = new Vector2(speed * (float)System.Math.Cos(radian), speed * (float)System.Math.Sin(radian));

        // 座標を更新
        drawingStatus.TranslateScreen(deltaPosition);
    }


    /// <summary>画面外に出た時の処理</summary>
    void OutOfScreen()
    {
        // オブジェクトのパラメータ
        float x = drawingStatus.PositionScreen.x;
        float y = drawingStatus.PositionScreen.y;
        float scale = drawingStatus.Scale;
        float sizex = scale * drawingStatus.SpriteSize.x / 2.0f;
        float sizey = scale * drawingStatus.SpriteSize.y / 2.0f;

        // 領域のパラメータ
        Vector2 GameScreenMin = new Vector2(StudySTG.Define.GameScreenCenterX - StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY - StudySTG.Define.GameScreenSizeY / 2);
        Vector2 GameScreenMax = new Vector2(StudySTG.Define.GameScreenCenterX + StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY + StudySTG.Define.GameScreenSizeY / 2);

        // 画面外に完全に出ている
        if (x + sizex < GameScreenMin.x || x - sizex > GameScreenMax.x || y + sizey < GameScreenMin.y || y - sizey > GameScreenMax.y)
        {
            if (enemyStatus.despawnable == true)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            enemyStatus.despawnable = true;
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
        if (enemyStatus.life <= 0)
        {
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
    }
}
