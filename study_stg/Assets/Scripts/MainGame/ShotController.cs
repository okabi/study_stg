using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   プレイヤー弾の制御に関連するクラス
/// </summary> 
public class ShotController : MonoBehaviour {
    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>アタッチされているShotStatusスクリプト</summary>
    private ShotStatus shotStatus;

    /// <summary>被弾時のエフェクトのプレハブ</summary>
    public GameObject DestroyEffectPrefab;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        shotStatus = GetComponent<ShotStatus>();

        // 親オブジェクトの設定
        transform.SetParent(GameObject.Find("Shots").transform);
    }
	
	
	void Update () {
        Move();
        OutOfScreen();
    }


    /// <summary>移動制御</summary>
    void Move()
    {
        float speed = shotStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * shotStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
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
            Destroy(gameObject);
        }
    }


    /// <summary>
    ///   ショットが敵に当たった時に呼び出される
    /// </summary>
    /// <param name="other">敵の情報</param>
    void OnTriggerStay2D(Collider2D other)
    {
        other.SendMessage("Damage", shotStatus.power);
        Vector2 pos = drawingStatus.PositionScreen;
        float angle = -90;
        Instantiate(DestroyEffectPrefab).GetComponent<DestroyEffect>().Initialize(
            pos,
            angle);
        Destroy(gameObject);
    }


    /// <summary>
    ///   ダミー
    /// </summary>
    void Damage()
    {
    }


    /// <summary>
    ///   各パラメータを設定．ショット生成時に呼び出すこと
    /// </summary>
    /// <param name="position">スクリーン座標</param>
    /// <param name="speed">スクリーン座標系での速度</param>
    /// <param name="angle">移動角度(度)</param>
    /// <param name="power">攻撃力</param>
    public void Initialize(Vector2 position, float speed, float angle, int power)
    {
        drawingStatus.PositionScreen = position;
        shotStatus.speed = speed;
        shotStatus.angle = angle;
        shotStatus.power = power;
    }
}
