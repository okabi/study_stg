using UnityEngine;
using System.Collections;

/// <summary>
///   敵弾の制御に関連するクラス
/// </summary> 
public class BulletController : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    protected DrawingStatus drawingStatus;

    ///<summary>アタッチされているBulletStatusスクリプト</summary>
    protected BulletStatus bulletStatus;



    void Awake () {
        drawingStatus = GetComponent<DrawingStatus>();
        bulletStatus = GetComponent<BulletStatus>();
        bulletStatus.despawnable = false;
    }
	
	
	void FixedUpdate () {
        Pattern();
        Move();
        OutOfScreen();
        bulletStatus.count += 1;
    }


    /// <summary>
    ///   動きを決定する．
    /// </summary>
    protected virtual void Pattern(){}


    ///<summary>移動制御</summary>
    void Move()
    {
        float speed = bulletStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * bulletStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
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
            if (bulletStatus.despawnable == true)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            bulletStatus.despawnable = true;
        }
    }


    /// <summary>
    ///   プレイヤーに当たった時に呼び出される
    /// </summary>
    /// <param name="other">プレイヤーの情報</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        other.SendMessage("Damage");
        Destroy(gameObject);
    }


    /// <summary>
    ///   ダミー
    /// </summary>
    void Damage()
    {
    }


    /// <summary>
    ///   各パラメータを設定．敵弾生成時に呼び出すこと
    /// </summary>
    /// <param name="position">スクリーン座標</param>
    /// <param name="speed">スクリーン座標系での速度</param>
    /// <param name="angle">移動角度(度)</param>
    public void Initialize(Vector2 position, float speed, float angle)
    {
        drawingStatus.PositionScreen = position;
        bulletStatus.speed = speed;
        bulletStatus.angle = angle;
    }

}
