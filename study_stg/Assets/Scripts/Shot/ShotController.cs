using UnityEngine;
using System.Collections;

/// <summary>
///   プレイヤー弾に関連するクラス
/// </summary> 
public class ShotController : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    ///<summary>アタッチされているShotStatusスクリプト</summary>
    private ShotStatus shotStatus;

    ///<summary>アタッチされているCollisionStatusスクリプト</summary>
    //private CollisionStatus collisionStatus;



    void Awake () {
        drawingStatus = GetComponent<DrawingStatus>();
        shotStatus = GetComponent<ShotStatus>();
    }
	
	
	void Update () {
        Move();
    }


    ///<summary>移動制御</summary>
    void Move()
    {
        float speed = shotStatus.speed;  // 移動スピード
        float radian = (float)System.Math.PI * shotStatus.angle / 180.0f;  // 移動角度(ラジアン)
        
        // 座標変位(スクリーン座標系)
        Vector2 deltaPosition = new Vector2(speed * (float)System.Math.Cos(radian), speed * (float)System.Math.Sin(radian));

        // 座標を更新
        drawingStatus.TranslateScreen(deltaPosition);
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
