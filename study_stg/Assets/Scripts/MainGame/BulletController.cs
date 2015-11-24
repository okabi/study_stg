using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   敵弾の制御に関連するクラス
/// </summary> 
public class BulletController : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;

    ///<summary>アタッチされているBulletStatusスクリプト</summary>
    private BulletStatus bulletStatus;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        bulletStatus = GetComponent<BulletStatus>();
        bulletStatus.isDespawnable = false;

        // 親オブジェクトの設定
        transform.SetParent(GameObject.Find("Bullets").transform);
    }
	
	
	void Update () {
        Move();
        OutOfScreen();
        bulletStatus.count += 1;
    }


    ///<summary>移動制御</summary>
    void Move()
    {
        float speed = bulletStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * bulletStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
        // 座標変位(スクリーン座標系)
        Vector2 deltaPosition = new Vector2(speed * (float)System.Math.Cos(radian), speed * (float)System.Math.Sin(radian));

        // 座標を更新
        drawingStatus.PositionScreen += deltaPosition;
    }


    /// <summary>画面外に出た時の処理</summary>
    void OutOfScreen()
    {
        // オブジェクトのパラメータ
        float x = drawingStatus.PositionScreen.x;
        float y = drawingStatus.PositionScreen.y;
        float scale = drawingStatus.Scale;
        float sizex = scale * drawingStatus.sprite.sprite.textureRect.width / 2.0f;
        float sizey = scale * drawingStatus.sprite.sprite.textureRect.height / 2.0f;

        // 領域のパラメータ
        Vector2 GameScreenMin = new Vector2(StudySTG.Define.GameScreenCenterX - StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY - StudySTG.Define.GameScreenSizeY / 2);
        Vector2 GameScreenMax = new Vector2(StudySTG.Define.GameScreenCenterX + StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY + StudySTG.Define.GameScreenSizeY / 2);

        // 画面外に完全に出ている
        if (x + sizex < GameScreenMin.x || x - sizex > GameScreenMax.x || y + sizey < GameScreenMin.y || y - sizey > GameScreenMax.y)
        {
            if (bulletStatus.isDespawnable == true)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            bulletStatus.isDespawnable = true;
        }
    }


    /// <summary>
    ///   プレイヤーに当たった時に呼び出される
    /// </summary>
    /// <param name="other">プレイヤーの情報</param>
    void OnTriggerStay2D(Collider2D other)
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
    /// <param name="imageType">弾画像</param>
    /// <param name="position">スクリーン座標</param>
    /// <param name="speed">スクリーン座標系での速度</param>
    /// <param name="angle">移動角度(度)</param>
    public void Initialize(Define.BulletImageType imageType, Vector2 position, float speed, float angle)
    {
        drawingStatus.sprite.sprite = gameStatus.bulletSprites[imageType];
        drawingStatus.PositionScreen = position;
        bulletStatus.speed = speed;
        bulletStatus.angle = angle;
        CircleCollider2D cc2d = GetComponent<CircleCollider2D>();
        cc2d.offset = new Vector2(-0.005f, 0.005f);
        cc2d.radius = 0.07f;
        if (imageType == Define.BulletImageType.MediumGreen)
        {
            cc2d.offset = new Vector2();
        }
        else if (imageType == Define.BulletImageType.BigRed || imageType == Define.BulletImageType.BigBlue)
        {
            cc2d.radius = 0.13f;
        }
    }

}
