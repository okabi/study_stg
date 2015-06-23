using UnityEngine;
using System.Collections;

/// <summary>
///   プレイヤーの操作に関連するクラス
/// </summary> 
public class PlayerController : MonoBehaviour {
    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    ///<summary>アタッチされているPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    ///<summary>アタッチされているCollisionStatusスクリプト</summary>
    //private CollisionStatus collisionStatus;



    void Awake () {
        drawingStatus = GetComponent<DrawingStatus>();
        playerStatus = GetComponent<PlayerStatus>();
    }
	
	
	void Update () {
        Move();
        Shot();
    }


    ///<summary>移動制御</summary>
    void Move()
    {
        const float Sqrt2 = 1.414f;  // ルート2
        float speed = playerStatus.speed;  // プレイヤーキャラのスピード
        Vector2 deltaPosition;  // 座標変位(スクリーン座標系)

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                deltaPosition = new Vector2(-speed / Sqrt2, -speed / Sqrt2);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                deltaPosition = new Vector2(-speed / Sqrt2, speed / Sqrt2);
            }
            else
            {
                deltaPosition = new Vector2(-speed, 0.0f);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                deltaPosition = new Vector2(speed / Sqrt2, -speed / Sqrt2);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                deltaPosition = new Vector2(speed / Sqrt2, speed / Sqrt2);
            }
            else
            {
                deltaPosition = new Vector2(speed, 0.0f);
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaPosition = new Vector2(0.0f, -speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaPosition = new Vector2(0.0f, speed);
        }
        else
        {
            deltaPosition = new Vector2(0.0f, 0.0f);
        }

        //collisionStatus.beforePositionScreen = drawingStatus.PositionScreen;
        drawingStatus.TranslateScreen(deltaPosition);
    }


    /// <summary>ショット制御</summary>
    void Shot()
    {
        if (playerStatus.rapid <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                playerStatus.rapid = 15;
            }
        }
        if (playerStatus.rapid > 0)
        {
            if (playerStatus.rapid % 5 == 0)
            {
                Vector2 pos = drawingStatus.PositionScreen;
                GameObject shot1 = Instantiate(playerStatus.MainShot);
                shot1.GetComponent<ShotController>().Initialize(pos + new Vector2(-6.0f, 0.0f), 10.0f, -90.0f, 1);
                GameObject shot2 = Instantiate(playerStatus.MainShot);
                shot2.GetComponent<ShotController>().Initialize(pos + new Vector2(6.0f, 0.0f), 10.0f, -90.0f, 1);
            }
            playerStatus.rapid -= 1;
        }
    }
}
