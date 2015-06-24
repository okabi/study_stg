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


    void Awake () {
        drawingStatus = GetComponent<DrawingStatus>();
        playerStatus = GetComponent<PlayerStatus>();
    }
	
	
	void FixedUpdate () {
        Move();
        OutOfScreen();
        Shot();
        playerStatus.count += 1;
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
            // なにもしない
        }

        // 一部が画面外に出ている時は位置を補正する
        if (x - sizex < GameScreenMin.x)
        {
            drawingStatus.PositionScreen = new Vector2(GameScreenMin.x + sizex, drawingStatus.PositionScreen.y);
        }
        if (x + sizex > GameScreenMax.x)
        {
            drawingStatus.PositionScreen = new Vector2(GameScreenMax.x - sizex, drawingStatus.PositionScreen.y);
        }
        if (y - sizey < GameScreenMin.y)
        {
            drawingStatus.PositionScreen = new Vector2(drawingStatus.PositionScreen.x, GameScreenMin.y + sizey);
        }
        if (y + sizey > GameScreenMax.y)
        {
            drawingStatus.PositionScreen = new Vector2(drawingStatus.PositionScreen.x, GameScreenMax.y - sizey);
        }
    }


    /// <summary>
    ///   被弾して残機が1つ減る
    /// </summary>
    void Damage()
    {
        playerStatus.life -= 1;
        drawingStatus.PositionScreen = new Vector2(StudySTG.Define.GameScreenCenterX, StudySTG.Define.GameScreenCenterY);
        if (playerStatus.life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
