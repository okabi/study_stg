using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StudySTG;

/// <summary>
///   プレイヤーの操作に関連するクラス
/// </summary> 
public class PlayerController : MonoBehaviour {
    /// <summary>プレイヤー弾(メイン)のプレハブ</summary>
    public GameObject MainShot;

    /// <summary>レーザーのプレハブ</summary>
    public GameObject LaserPrefab;

        /// <summary>ロックオンのプレハブ</summary>
    public GameObject LockonPrefab;
    
    /// <summary>アタッチされているPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>サブショット用円形エフェクトのDrawingStatus</summary>
    private DrawingStatus circleDrawingStatus;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        playerStatus = GetComponent<PlayerStatus>();
        playerStatus.drawingStatus = GetComponent<DrawingStatus>();
        playerStatus.enemyController = new List<EnemyController>();
        playerStatus.lockonDrawingStatus = new List<DrawingStatus>();
        playerStatus.command = new Dictionary<PlayerStatus.CommandType, int>();
        foreach (PlayerStatus.CommandType type in System.Enum.GetValues(typeof(PlayerStatus.CommandType)))
        {
            playerStatus.command[type] = 0;
        }
        drawingStatus = playerStatus.drawingStatus;
        circleDrawingStatus = GameObject.Find("Circle").GetComponent<DrawingStatus>();
        playerStatus.score = 0;
    }
	
	
	void Update () {
        InputManager();
        Move();
        Animation();
        OutOfScreen();
        Shot();
        Laser();
        UpdateParameters();
        playerStatus.count += 1;
    }


    /// <summary>プレイヤーの入力を受け取る</summary>
    void InputManager()
    {
        if (Input.GetKey(KeyCode.UpArrow)) playerStatus.command[PlayerStatus.CommandType.Up] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Up] = 0;
        if (Input.GetKey(KeyCode.DownArrow)) playerStatus.command[PlayerStatus.CommandType.Down] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Down] = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) playerStatus.command[PlayerStatus.CommandType.Left] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Left] = 0;
        if (Input.GetKey(KeyCode.RightArrow)) playerStatus.command[PlayerStatus.CommandType.Right] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Right] = 0;
        if (Input.GetKey(KeyCode.Z)) playerStatus.command[PlayerStatus.CommandType.Shot] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Shot] = 0;
        if (Input.GetKey(KeyCode.X)) playerStatus.command[PlayerStatus.CommandType.Charge] += 1;
        else playerStatus.command[PlayerStatus.CommandType.Charge] = 0;
    }


    /// <summary>移動制御</summary>
    void Move()
    {
        const float Sqrt2 = 1.414f;  // ルート2
        float speed = playerStatus.speed;  // プレイヤーキャラのスピード
        Vector2 deltaPosition;  // 座標変位(スクリーン座標系)
        bool up = playerStatus.command[PlayerStatus.CommandType.Up] > 0;
        bool down = playerStatus.command[PlayerStatus.CommandType.Down] > 0;
        bool left = playerStatus.command[PlayerStatus.CommandType.Left] > 0;
        bool right = playerStatus.command[PlayerStatus.CommandType.Right] > 0;

        if (left)
        {
            if (up)
            {
                deltaPosition = new Vector2(-speed / Sqrt2, -speed / Sqrt2);
            }
            else if (down)
            {
                deltaPosition = new Vector2(-speed / Sqrt2, speed / Sqrt2);
            }
            else
            {
                deltaPosition = new Vector2(-speed, 0.0f);
            }
        }
        else if (right)
        {
            if (up)
            {
                deltaPosition = new Vector2(speed / Sqrt2, -speed / Sqrt2);
            }
            else if (down)
            {
                deltaPosition = new Vector2(speed / Sqrt2, speed / Sqrt2);
            }
            else
            {
                deltaPosition = new Vector2(speed, 0.0f);
            }
        }
        else if (up)
        {
            deltaPosition = new Vector2(0.0f, -speed);
        }
        else if (down)
        {
            deltaPosition = new Vector2(0.0f, speed);
        }
        else
        {
            deltaPosition = new Vector2(0.0f, 0.0f);
        }

        drawingStatus.PositionScreen += deltaPosition;
    }


    /// <summary>ショット制御</summary>
    void Shot()
    {
        bool shot = playerStatus.command[PlayerStatus.CommandType.Shot] > 0;
        bool charge = playerStatus.command[PlayerStatus.CommandType.Charge] > 0;

        if (charge)
        {
            playerStatus.rapid = 0;
        }
        else
        {
            if (playerStatus.rapid <= 0)
            {
                if (shot)
                {
                    playerStatus.rapid = 15;
                }
            }
            if (playerStatus.rapid > 0)
            {
                if (playerStatus.rapid % 5 == 0)
                {
                    // ショットの生成
                    Vector2 pos = drawingStatus.PositionScreen;
                    Instantiate(MainShot).GetComponent<ShotController>().Initialize(pos + new Vector2(-6.0f, 0.0f), 10.0f, -90.0f, 1);
                    Instantiate(MainShot).GetComponent<ShotController>().Initialize(pos + new Vector2(6.0f, 0.0f), 10.0f, -90.0f, 1);
                }
                playerStatus.rapid -= 1;
            }
        }
    }


    /// <summary>レーザー制御</summary>
    void Laser()
    {
        bool charge = playerStatus.command[PlayerStatus.CommandType.Charge] > 0;

        if (charge)
        {
            // ロックオン範囲を広げていく
            float r = playerStatus.circleRadius;
            if (r < 100.0f)
            {
                playerStatus.circleRadius += 5.0f;
            }
            else if (r < 200.0f)
            {
                playerStatus.circleRadius += 1.0f;
            }
            else
            {
                playerStatus.circleRadius += 0.5f;
            }
        }
        else
        {
            // ロックオン範囲をゼロにして，ロックオンしている敵がいる場合はレーザーを出す
            playerStatus.circleRadius = 0;
            float angle = 150.0f;
            foreach (DrawingStatus ds in playerStatus.lockonDrawingStatus)
            {
                LockonStatus ls = ds.gameObject.GetComponent<LockonStatus>();
                Instantiate(LaserPrefab).GetComponent<LaserController>().Initialize(drawingStatus.PositionScreen, ls.enemyDrawingStatus, 30, angle);
                angle += 10.0f;
                Destroy(ds.gameObject);
            }
            foreach (EnemyController ec in playerStatus.enemyController)
            {
                ec.LockonReset();
            }
            playerStatus.lockonDrawingStatus = new List<DrawingStatus>();
        }

    }

    /// <summary>自機のアニメーション</summary>
    void Animation()
    {
        // 移動方向に合わせて自機を傾ける
        
        // 溜め撃ちに合わせて円を広げる
        Vector2 pos = drawingStatus.PositionScreen;
        Vector2 min = pos - new Vector2(playerStatus.circleRadius, playerStatus.circleRadius);
        Vector2 max = pos + new Vector2(playerStatus.circleRadius, playerStatus.circleRadius);
        Utility.ExtendScreenPosition(min, max, ref circleDrawingStatus);
    }


    /// <summary>各種パラメータを更新する</summary>
    void UpdateParameters()
    {
        // 敵機が消滅していたら敵機座標リストから消す
        List<EnemyController> newEnemyList = new List<EnemyController>();
        foreach (EnemyController ec in playerStatus.enemyController)
        {
            if (ec != null)
            {
                newEnemyList.Add(ec);
            }
        }
        playerStatus.enemyController = newEnemyList;

        // 照準が消滅していたら照準座標リストから消す
        List<DrawingStatus> newLockonList = new List<DrawingStatus>();
        foreach (DrawingStatus ds in playerStatus.lockonDrawingStatus)
        {
            if (ds != null)
            {
                newLockonList.Add(ds);
            }
        }
        playerStatus.lockonDrawingStatus = newLockonList;

        // 敵機をロックオンする
        if (playerStatus.circleRadius > 0)
        {
            foreach (EnemyController ec in playerStatus.enemyController)
            {
                bool flag = ec.Lockon(drawingStatus.PositionScreen, playerStatus.circleRadius);
                if (flag)
                {
                    GameObject obj = Instantiate(LockonPrefab);
                    obj.GetComponent<LockonController>().Initialize(drawingStatus, ec.gameObject.GetComponent<DrawingStatus>());
                    playerStatus.lockonDrawingStatus.Add(obj.GetComponent<DrawingStatus>());
                }
            }
        }
    }


    /// <summary>画面外に出た時の処理</summary>
    void OutOfScreen()
    {
        // オブジェクトのパラメータ
        float x = drawingStatus.PositionScreen.x;
        float y = drawingStatus.PositionScreen.y;
        float sizex = 20.0f;
        float sizey = 30.0f;

        // 領域のパラメータ
        Vector2 GameScreenMin = new Vector2(Define.GameScreenCenterX - Define.GameScreenSizeX / 2, Define.GameScreenCenterY - Define.GameScreenSizeY / 2);
        Vector2 GameScreenMax = new Vector2(Define.GameScreenCenterX + Define.GameScreenSizeX / 2, Define.GameScreenCenterY + Define.GameScreenSizeY / 2);

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
