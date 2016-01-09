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

    /// <summary>被弾時のエフェクトのプレハブ</summary>
    public GameObject DestroyEffectPrefab;

    /// <summary>プレイヤーの残機表示用UIのプレハブ</summary>
    public GameObject PlayerLifeUIPrefab;
    
    /// <summary>アタッチされているPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>サブショット用円形エフェクトのDrawingStatus</summary>
    private DrawingStatus circleDrawingStatus;

    /// <summary>総合的なゲーム情報</summary>
    private GameStatus gameStatus;

    /// <summary>プレイヤーの残機表示用UI</summary>
    private List<GameObject> playerLifeUIs;

    /// <summary>保存データ</summary>
    private SaveStatus saveStatus;

    /// <summary>リプレイ制御用</summary>
    private ReplayController replayController;

    /// <summary>効果音再生用</summary>
    private AudioController audio;


    void Awake () {
        // コンポーネントやオブジェクトの読み込み
        playerStatus = GetComponent<PlayerStatus>();
        playerStatus.drawingStatus = GetComponent<DrawingStatus>();
        playerStatus.enemyController = new List<EnemyController>();
        playerStatus.lockonDrawingStatus = new List<DrawingStatus>();
        playerStatus.lockonStatus = new List<LockonStatus>();
        playerStatus.status = PlayerStatus.StatusType.Alive;
        playerStatus.command = new Dictionary<PlayerStatus.CommandType, int>();
        foreach (PlayerStatus.CommandType type in System.Enum.GetValues(typeof(PlayerStatus.CommandType)))
        {
            playerStatus.command[type] = 0;
        }
        drawingStatus = playerStatus.drawingStatus;
        circleDrawingStatus = GameObject.Find("Circle").GetComponent<DrawingStatus>();
        gameStatus = GameObject.Find("GameController").GetComponent<GameStatus>();
        audio = GameObject.Find("AudioController").GetComponent<AudioController>();
        playerLifeUIs = new List<GameObject>();
        playerStatus.score = 0;
        playerStatus.tagScore = new Dictionary<Define.EnemyTag, int>();
        playerStatus.missFactor = new List<Define.EnemyTag>();
        playerStatus.missTime = new List<int>();
        playerStatus.minDistance = new Dictionary<Define.EnemyTag, List<float>>();
        playerStatus.lockoned = new Dictionary<Define.EnemyTag, List<int>>();
        foreach (Define.EnemyTag tag in System.Enum.GetValues(typeof(Define.EnemyTag)))
        {
            playerStatus.tagScore[tag] = 0;
            playerStatus.minDistance[tag] = new List<float>();
            playerStatus.lockoned[tag] = new List<int>();
        }
        playerStatus.noDamageCount = 120;
        AddLifeUI(playerStatus.life - 1);
        if (GameObject.Find("SaveController") != null)
        {
            saveStatus = GameObject.Find("SaveController").GetComponent<SaveStatus>();
        }
        replayController = GameObject.Find("ReplayController").GetComponent<ReplayController>();
    }
	
	
	void Update () {
        InputManager();
        switch (playerStatus.status)
        {
            case PlayerStatus.StatusType.Alive:
                Move();
                OutOfScreen();
                Shot();
                Laser();
                break;
            case PlayerStatus.StatusType.Miss:
                Miss();
                break;
        }
        Animation();
        UpdateParameters();
        playerStatus.count += 1;
    }


    /// <summary>
    ///   残機表示を増やしたり減らしたりする
    /// </summary>
    /// <param name="addNum">増やす数(減らす場合は負の数を指定する)</param>
    void AddLifeUI(int addNum)
    {
        if (addNum > 0)
        {
            GameObject UIParent = GameObject.Find("UI Canvas");
            for (int i = 0; i < addNum; i++)
            {
                GameObject obj = Instantiate(PlayerLifeUIPrefab);
                obj.transform.SetParent(UIParent.transform);
                UIImage uii = obj.GetComponent<UIImage>();
                uii.Position = new Vector2(16 * playerLifeUIs.Count, 18);
                uii.Scale = 1.0f;
                playerLifeUIs.Add(obj);
            }
        }
        else
        {
            int maxIndex = playerLifeUIs.Count - 1;
            addNum = System.Math.Abs(addNum);
            for (int i = 0; i < addNum; i++)
            {
                if (maxIndex - i >= 0)
                {
                    Destroy(playerLifeUIs[maxIndex - i]);
                    playerLifeUIs.RemoveAt(maxIndex - i);
                }
            }
        }
    }


    /// <summary>プレイヤーの入力を受け取る</summary>
    void InputManager()
    {
        if (saveStatus != null && saveStatus.replaying)
        {
            var command = replayController.LoadInput();
            foreach (var c in command)
            {
                playerStatus.command[c.Key] = c.Value ? playerStatus.command[c.Key] + 1 : 0;
            }
        }
        else
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
            replayController.SaveInput(playerStatus.command);
        }
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
                    audio.PlaySoundEffect(Define.SoundID.Shot);
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
            if (r < 80.0f)
            {
                playerStatus.circleRadius += 5.0f;
            }
            else if (r < 120.0f)
            {
                playerStatus.circleRadius += 1.0f;
            }
            else if (r < 150.0f)
            {
                playerStatus.circleRadius += 0.5f;
            }
        }
        else
        {
            // ロックオン範囲をゼロにして，ロックオンしている敵がいる場合はレーザーを出す
            playerStatus.circleRadius = 0;
            int num = 0;
            bool soundPlayed = false;
            foreach (DrawingStatus ds in playerStatus.lockonDrawingStatus)
            {
                if (ds != null)
                {
                    LockonStatus ls = ds.gameObject.GetComponent<LockonStatus>();
                    float angle = 0;
                    if (num % 2 == 0)
                    {
                        angle = 90 + 40 * (num / 2);
                    }
                    else
                    {
                        angle = -90 - 40 * (num / 2);
                    }
                    if (!soundPlayed)
                    {
                        audio.PlaySoundEffect(Define.SoundID.Laser);
                        soundPlayed = true;
                    }
                    Instantiate(LaserPrefab).GetComponent<LaserController>().Initialize(drawingStatus.PositionScreen, ls.enemyStatus, playerStatus.laserPower, angle);
                    Destroy(ds.gameObject);
                    num += 1;
                }
            }
            foreach (EnemyController ec in playerStatus.enemyController)
            {
                if (ec != null)
                {
                    ec.LockonReset();
                }
            }
            playerStatus.lockonDrawingStatus = new List<DrawingStatus>();
            playerStatus.lockonStatus = new List<LockonStatus>();
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
        if (playerStatus.lockonDrawingStatus.Count < playerStatus.lockonMaxNum)
        {
            circleDrawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            circleDrawingStatus.Blend = new Color(1.0f, 0.1f, 0.1f);
        }

        // 無敵時間中なら青色に点滅させる
        if (playerStatus.noDamageCount > 0 && playerStatus.noDamageCount % 10 < 5)
        {
            playerStatus.drawingStatus.Blend = new Color(0.1f, 0.1f, 1.0f);
        }
        else
        {
            playerStatus.drawingStatus.Blend = new Color(1.0f, 1.0f, 1.0f);
        }
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
        foreach (LockonStatus ls in playerStatus.lockonStatus)
        {
            if (ls.enemyStatus != null)
            {
                ls.enemyStatus.lockonMultiply = 1;
            }
        }
        List<LockonStatus> newLockonStatusList = new List<LockonStatus>();
        for (int i = 0; i < playerStatus.lockonStatus.Count; i++)
        {
            if (playerStatus.lockonStatus[i] != null)
            {
                if (newLockonStatusList.Count + 1 >= playerStatus.lockonStatus[i].enemyStatus.lockonMultiply)
                {
                    playerStatus.lockonStatus[i].multiply = newLockonStatusList.Count + 1;
                }
                newLockonStatusList.Add(playerStatus.lockonStatus[i]);
            }
        }
        playerStatus.lockonStatus = newLockonStatusList;

        // 敵機をロックオンする
        if (playerStatus.circleRadius > 0 && playerStatus.lockonDrawingStatus.Count < playerStatus.lockonMaxNum)
        {
            foreach (EnemyController ec in playerStatus.enemyController)
            {
                bool flag = ec.Lockon(drawingStatus.PositionScreen, playerStatus.circleRadius, playerStatus.laserPower, playerStatus.lockonDrawingStatus.Count + 1);
                if (flag)
                {
                    audio.PlaySoundEffect(Define.SoundID.Lockon);
                    GameObject obj = Instantiate(LockonPrefab);
                    obj.GetComponent<LockonController>().Initialize(drawingStatus, ec.gameObject.GetComponent<DrawingStatus>(), playerStatus.lockonDrawingStatus.Count + 1);
                    playerStatus.lockonDrawingStatus.Add(obj.GetComponent<DrawingStatus>());
                    playerStatus.lockonStatus.Add(obj.GetComponent<LockonStatus>());
                    playerStatus.circleRadius = 0;
                }
            }
        }

        // 無敵時間を進める
        if (playerStatus.noDamageCount > 0)
        {
            playerStatus.noDamageCount -= 1;
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
    ///   被弾時の自機制御
    /// </summary>
    void Miss()
    {
        int c = playerStatus.missCount;

        if (gameStatus.gameoverCount > 0) { }
        else if (c == 0)
        {
            // 敵機のロックオンを消去する
            foreach (EnemyController ec in playerStatus.enemyController)
            {
                if (ec != null)
                {
                    ec.LockonReset();
                    ec.gameObject.GetComponent<EnemyStatus>().isToBeDestroyedByLaser = false;
                }
            }
            foreach (DrawingStatus ds in playerStatus.lockonDrawingStatus)
            {
                if (ds != null)
                {
                    Destroy(ds.gameObject);
                }
            }
            playerStatus.lockonDrawingStatus = new List<DrawingStatus>();
            playerStatus.lockonStatus = new List<LockonStatus>();
            foreach (PlayerStatus.CommandType type in System.Enum.GetValues(typeof(PlayerStatus.CommandType)))
            {
                playerStatus.command[type] = 0;
            }
            playerStatus.circleRadius = 0;

            // 座標を移動する
            drawingStatus.PositionScreen = new Vector2(StudySTG.Define.GameScreenCenterX, StudySTG.Define.GameScreenSizeY + 100);

            if (playerStatus.life == 1)
            {
                // ゲームオーバー処理に移行
                gameStatus.gameoverCount = 1;
            }
        }
        else if (c < 50) { }
        else if (c == 50)
        {
            playerStatus.noDamageCount = 170;
            playerStatus.life -= 1;
            AddLifeUI(-1);
        }
        else if (c < 100)
        {
            playerStatus.drawingStatus.PositionScreen += new Vector2(0, -3);
        }
        else
        {
            playerStatus.status = PlayerStatus.StatusType.Alive;
        }
        playerStatus.missCount += 1;
    }


    /// <summary>
    ///   被弾して残機が1つ減る
    /// </summary>
    /// <param name="other">相手の情報</param>
    void Damage(GameObject other)
    {
        if (playerStatus.status == PlayerStatus.StatusType.Alive && playerStatus.noDamageCount <= 0)
        {
            // ミス原因について記録する
            switch (other.layer)
            {
                case 10:
                    playerStatus.missFactor.Add(other.GetComponent<EnemyStatus>().tag);
                    playerStatus.missTime.Add(gameStatus.count);
                    break;
                case 11:
                    playerStatus.missFactor.Add(other.GetComponent<BulletStatus>().tag);
                    playerStatus.missTime.Add(gameStatus.count);
                    break;
            }

            // 被弾エフェクトを出す
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos = playerStatus.drawingStatus.PositionScreen;
                float angle = 0.1f * gameStatus.rand.Next(3600);
                Instantiate(DestroyEffectPrefab).GetComponent<DestroyEffect>().Initialize(
                    pos,
                    angle,
                    1000,
                    new Color(0.2f, 0.2f, 1.0f));
            }

            // プレイヤー状態をミスに変える
            audio.PlaySoundEffect(Define.SoundID.PlayerDie);
            playerStatus.status = PlayerStatus.StatusType.Miss;
            playerStatus.missCount = 0;
        }
    }
}
