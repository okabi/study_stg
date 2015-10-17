using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>ホーミングレーザーの制御</summary>
public class LaserController : MonoBehaviour {
    /// <summary>アタッチされているLaserStatus</summary>
    private LaserStatus laserStatus;

    /// <summary>レーザーパーツのプレハブ</summary>
    public GameObject partPrefab;

    /// <summary>レーザーパーツの数</summary>
    public int partNum;

    /// <summary>レーザーの各パーツのVerticesStatus</summary>
    private VerticesStatus[] verticesStatus;


    void Awake()
    {
        transform.SetParent(GameObject.Find("Lasers").transform);
        laserStatus = GetComponent<LaserStatus>();
        verticesStatus = new VerticesStatus[partNum];
        laserStatus.verticesPosition = new Vector2[partNum + 1];
        for (int i = 0; i < verticesStatus.Length; i++)
        {
            GameObject obj = Instantiate(partPrefab);
            obj.transform.SetParent(transform);
            verticesStatus[i] = obj.GetComponent<VerticesStatus>();
            Vector2[] uv = new Vector2[4];
            float min = (float)(verticesStatus.Length - i) / verticesStatus.Length;
            float max = (float)(verticesStatus.Length - i - 1) / verticesStatus.Length;
            uv[0] = new Vector2(0, max);
            uv[1] = new Vector2(1, max);
            uv[2] = new Vector2(0, min);
            uv[3] = new Vector2(1, min);
            verticesStatus[i].SetUV(uv[0], uv[1], uv[2], uv[3]);
        }
    }


    void Update()
    {
        // 各パーツの中心座標を更新
        Move();

        // 各パーツの頂点座標を更新
        Animation();

        // ロックオンした敵との当たり判定
        Collision();

        // 画面外に出ていて，役目を終えているなら破棄する
        OutOfScreen();

        laserStatus.count += 1;
    }


    /// <summary>各パーツの中心座標を動かす</summary>
    void Move()
    {
        int c = laserStatus.count;

        // 先頭以外のパーツの座標を更新
        for (int i = laserStatus.verticesPosition.Length - 2; i >= 0; i--)
        {
            laserStatus.verticesPosition[i + 1] = laserStatus.verticesPosition[i];
        }

        // 先頭パーツの座標を更新
        const int maxCount = 30;  // 移動角を曲げる最大フレーム数
        if (laserStatus.enemyDrawingStatus == null)
        {
            if (c >= maxCount) laserStatus.isCollision = true;
        }
        else
        {
            laserStatus.destinationPosition = laserStatus.enemyDrawingStatus.PositionScreen;
        }
        if (!laserStatus.isCollision)
        {
            Vector2 deltaPos = laserStatus.destinationPosition - laserStatus.verticesPosition[0];
            laserStatus.fixedAngle = (float)System.Math.Atan2(deltaPos.y, deltaPos.x);
            if (c < maxCount)
            {
                laserStatus.fixedAngle += (laserStatus.plusAngle * (float)System.Math.PI / 180) * (maxCount - c) / maxCount;
            }
        }
        laserStatus.verticesPosition[0] += 6.0f * new Vector2((float)System.Math.Cos(laserStatus.fixedAngle), (float)System.Math.Sin(laserStatus.fixedAngle));
    }


    /// <summary>各パーツの頂点座標を更新する</summary>
    void Animation()
    {
        int num = laserStatus.count;
        if (num >= verticesStatus.Length) num = verticesStatus.Length;
        Vector2 upperLeft = new Vector2();
        Vector2 upperRight = new Vector2();
        for (int i = 0; i < num; i++)
        {
            Vector2 deltaPos = laserStatus.verticesPosition[i] - laserStatus.verticesPosition[i + 1];
            float baseAngle = (float)System.Math.Atan2(deltaPos.y, deltaPos.x);
            float angle1 = baseAngle - (float)System.Math.PI / 2.0f;
            float angle2 = baseAngle + (float)System.Math.PI / 2.0f;
            float cos1 = (float)System.Math.Cos(angle1);
            float cos2 = (float)System.Math.Cos(angle2);
            float sin1 = (float)System.Math.Sin(angle1);
            float sin2 = (float)System.Math.Sin(angle2);
            const float width = 4.0f;
            if (i == 0)
            {
                upperLeft = laserStatus.verticesPosition[i] + width * new Vector2(cos2, sin2);
                upperRight = laserStatus.verticesPosition[i] + width * new Vector2(cos1, sin1);
            }
            Vector2 lowerLeft = laserStatus.verticesPosition[i + 1] + width * new Vector2(cos2, sin2);
            Vector2 lowerRight = laserStatus.verticesPosition[i + 1] + width * new Vector2(cos1, sin1);
            verticesStatus[i].SetVerticesPosition(upperLeft, upperRight, lowerLeft, lowerRight);
            upperLeft = lowerLeft;
            upperRight = lowerRight;
        }
    }


    /// <summary>ロックオンした敵に当たったかを判定する</summary>
    void Collision()
    {
        if (laserStatus.enemyDrawingStatus == null || laserStatus.isCollision) return;
        Vector2 laserPos = laserStatus.verticesPosition[0];
        Vector2 enemyPos = laserStatus.enemyDrawingStatus.PositionScreen;
        Vector2 deltaPos = new Vector2(laserPos.x - enemyPos.x, laserPos.y - enemyPos.y);
        float d = deltaPos.x * deltaPos.x + deltaPos.y * deltaPos.y;
        if (d < 10 * 10)
        {
            laserStatus.enemyDrawingStatus.gameObject.SendMessage("Damage", laserStatus.power);
            laserStatus.isCollision = true;
        }
    }


    /// <summary>全パーツが画面外に出ているかを見て，出ているかつ役目を終えているならこのオブジェクトを破棄する</summary>
    void OutOfScreen()
    {
        if (!laserStatus.isCollision) return;
        bool flag = true;
        foreach (Vector2 pos in laserStatus.verticesPosition)
        {
            // オブジェクトのパラメータ
            float x = pos.x;
            float y = pos.y;

            // 領域のパラメータ
            Vector2 GameScreenMin = new Vector2(StudySTG.Define.GameScreenCenterX - StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY - StudySTG.Define.GameScreenSizeY / 2);
            Vector2 GameScreenMax = new Vector2(StudySTG.Define.GameScreenCenterX + StudySTG.Define.GameScreenSizeX / 2, StudySTG.Define.GameScreenCenterY + StudySTG.Define.GameScreenSizeY / 2);

            // 画面外に出ていない
            if (GameScreenMin.x < x && x < GameScreenMax.x && GameScreenMin.y < y && y < GameScreenMax.y)
            {
                flag = false;
            }
        }
        if (flag)
        {
            if (laserStatus.isCollision)
            {
                Destroy(gameObject);
            }
        }
    }


    /// <summary>
    ///   初期化関数
    /// </summary>
    /// <param name="startPos">レーザー発射地点のスクリーン座標</param>
    /// <param name="enemyDrawingStatus">目標の敵機のDrawingStatus</param>
    /// <param name="power">攻撃力</param>
    /// <param name="plusAng">最大プラス角度(度)</param>
    public void Initialize(Vector2 startPos, DrawingStatus enemyDrawingStatus, int power, float plusAng)
    {
        laserStatus.startPosition = startPos;
        for (int i = 0; i < verticesStatus.Length; i++)
        {
            laserStatus.verticesPosition[i] = startPos;
            verticesStatus[i].SetVerticesPosition(startPos, startPos, startPos, startPos);
        }
        laserStatus.enemyDrawingStatus = enemyDrawingStatus;
        laserStatus.destinationPosition = enemyDrawingStatus.PositionScreen;
        laserStatus.count = 0;
        laserStatus.power = power;
        laserStatus.isCollision = false;
        laserStatus.plusAngle = plusAng;
    }
}
