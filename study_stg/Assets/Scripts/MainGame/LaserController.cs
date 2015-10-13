using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>ホーミングレーザーの制御</summary>
public class LaserController : MonoBehaviour {
    /// <summary>アタッチされているLaserStatus</summary>
    private LaserStatus laserStatus;

    /// <summary>レーザーの各パーツのVerticesStatus</summary>
    public VerticesStatus[] verticesStatus;

    public Vector2[] verticesPosition;

    public Vector2[] uv;


    void Awake()
    {
        laserStatus = GetComponent<LaserStatus>();
        verticesPosition = new Vector2[4];
        uv = new Vector2[4];
        verticesPosition[0] = new Vector2(-1, 1);
        verticesPosition[1] = new Vector2(1, 1);
        verticesPosition[2] = new Vector2(-1, -1);
        verticesPosition[3] = new Vector2(1, -1);
        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);
    }


    void Update()
    {
        // 以下，テスト
        verticesStatus[0].SetVerticesPosition(verticesPosition[0], verticesPosition[1], verticesPosition[2], verticesPosition[3]);
        verticesStatus[0].SetUV(uv[0], uv[1], uv[2], uv[3]);
        // 以上，テスト
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
        for (int i = 0; i < laserStatus.vertices.GetLength(0); i++)
        {
            for (int j = 0; j < laserStatus.vertices.GetLength(1); j++)
            {
                laserStatus.vertices[i, j] = Utility.ScreenToWorld(startPos);
            }
        }
        laserStatus.enemyDrawingStatus = enemyDrawingStatus;
        laserStatus.destinationPosition = enemyDrawingStatus.PositionScreen;
        laserStatus.count = 0;
        laserStatus.disappearCount = 0;
        laserStatus.power = power;
        laserStatus.isCollision = false;
        laserStatus.plusAngle = plusAng;
    }
}
