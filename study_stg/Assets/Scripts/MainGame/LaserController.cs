using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>ホーミングレーザーの制御</summary>
public class LaserController : MonoBehaviour {
    /// <summary>アタッチされているLaserStatus</summary>
    private LaserStatus laserStatus;

    /// <summary>レーザーの各パーツのMeshFilter</summary>
    public MeshFilter[] meshFilter;


    void Awake()
    {
        laserStatus = GetComponent<LaserStatus>();
    }


    void Update()
    {
        int c = laserStatus.count;

        // 描画濃度の決定
        

        /*
        Vector3[] v = meshFilter.mesh.vertices;
        for (int i = 0; i < v.Length; i++)
        {
            v[i] = Utility.ScreenToWorld(vertices[i]);
        }
        meshFilter.mesh.vertices = v;
        meshFilter.mesh.RecalculateBounds();
        */
    }

    /*
        void Update()
        {
            int c = laserStatus.count;
        
            // 描画濃度の決定
            if (c < laserStatus.childrenDrawingStatus.Length)
            {
                laserStatus.childrenDrawingStatus[c].Alpha = 255;
            }
            if (laserStatus.isCollision)
            {
                laserStatus.childrenDrawingStatus[laserStatus.disappearCount].Alpha = 0;
                laserStatus.disappearCount += 1;
                if (laserStatus.disappearCount == laserStatus.childrenDrawingStatus.Length)
                {
                    Destroy(gameObject);
                }
            }

            // 各座標の更新
            for (int i = 0; i < laserStatus.childrenDrawingStatus.Length - 1; i++)
            {
                laserStatus.childrenDrawingStatus[i + 1].PositionScreen = laserStatus.childrenDrawingStatus[i].PositionScreen;
            }

            if (!laserStatus.isCollision)
            {
                // 目標地点の更新
                if (laserStatus.enemyDrawingStatus != null) laserStatus.destinationPosition = laserStatus.enemyDrawingStatus.PositionScreen;

                // 先頭座標の移動
                float sx = laserStatus.destinationPosition.x - laserStatus.childrenDrawingStatus[0].PositionScreen.x;
                float sy = laserStatus.destinationPosition.y - laserStatus.childrenDrawingStatus[0].PositionScreen.y;
                float angle = (float)System.Math.Atan2(sy, sx);
                if (c < 20) angle += (laserStatus.plusAngle * c / 20.0f) * (float)System.Math.PI / 180.0f;
                laserStatus.childrenDrawingStatus[0].PositionScreen += 10.0f * new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));

                // 当たり判定
                sx = laserStatus.destinationPosition.x - laserStatus.childrenDrawingStatus[0].PositionScreen.x;
                sy = laserStatus.destinationPosition.y - laserStatus.childrenDrawingStatus[0].PositionScreen.y;
                if (sx * sx + sy * sy < 10.0f * 10.0f)
                {
                    laserStatus.isCollision = true;
                    if (laserStatus.enemyDrawingStatus != null) laserStatus.enemyDrawingStatus.gameObject.SendMessage("Damage", laserStatus.power);
                }

                // 頂点座標の計算
                for (int i = 0; i < laserStatus.childrenDrawingStatus.Length - 1; i++)
                {
                    const float width = 4.0f;  // レーザー幅
                    Vector2 delta = laserStatus.childrenDrawingStatus[i].PositionScreen - laserStatus.childrenDrawingStatus[i + 1].PositionScreen;
                    angle = (float)System.Math.Atan2(delta.y, delta.x);
                    // laserStatus.childrenDrawingStatus[i].sprite.
                    // Vector2 min = new Vector2(laserStatus.childrenDrawingStatus[i].PositionScreen.x + width * System.Math.Cos(angle), 
                }
            }
        
            // カウントを進める
            laserStatus.count += 1;
        }
    */

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
