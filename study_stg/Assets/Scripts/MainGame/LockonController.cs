using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>敵の照準の制御</summary>
public class LockonController : MonoBehaviour {
    /// <summary>アタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>アタッチされているLockonStatus</summary>
    private LockonStatus lockonStatus;

    /// <summary>プレイヤーのDrawingStatus</summary>
    private DrawingStatus playerDrawingStatus;

    /// <summary>照準の当たっている敵のDrawingStatus</summary>
    private DrawingStatus enemyDrawingStatus;

    /// <summary>アタッチされているLineRenderer</summary>
    private LineRenderer lineRenderer;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        lockonStatus = GetComponent<LockonStatus>();
        lineRenderer = GetComponent<LineRenderer>();
    
        // 親オブジェクトの設定
        transform.SetParent(GameObject.Find("Lockons").transform);
    }


    void Update()
    {
        // 敵が消えていれば消滅する
        if (enemyDrawingStatus == null)
        {
            Destroy(gameObject);
            return;
        }

        // 座標の調整とアニメーション
        if (lockonStatus.count < 20)
        {
            drawingStatus.Alpha = (int)(255 * lockonStatus.count / 20.0f);
            drawingStatus.Scale = 0.1f + 0.6f * (20 - lockonStatus.count) / 20.0f;
        }
        else
        {
            drawingStatus.Alpha = 255;
            drawingStatus.Scale = 0.1f;
        }
        drawingStatus.PositionScreen = enemyDrawingStatus.PositionScreen;
        lineRenderer.SetPosition(0, Utility.ScreenToWorld(playerDrawingStatus.PositionScreen));
        lineRenderer.SetPosition(1, Utility.ScreenToWorld(enemyDrawingStatus.PositionScreen));
        drawingStatus.Rotation -= 15.0f;
        lockonStatus.count += 1;
    }


    /// <summary>
    ///   初期化関数
    /// </summary>
    /// <param name="playerDrawingStatus">プレイヤーのDrawingStatus</param>
    /// <param name="enemyDrawingStatus">照準の当たっている敵のDrawingStatus</param>
    public void Initialize(DrawingStatus playerDrawingStatus, DrawingStatus enemyDrawingStatus)
    {
        drawingStatus.PositionScreen = enemyDrawingStatus.PositionScreen;
        this.playerDrawingStatus = playerDrawingStatus;
        this.enemyDrawingStatus = enemyDrawingStatus;
        lineRenderer.SetPosition(0, Utility.ScreenToWorld(playerDrawingStatus.PositionScreen));
        lineRenderer.SetPosition(1, Utility.ScreenToWorld(enemyDrawingStatus.PositionScreen));
        lineRenderer.SetColors(new Color(230 / 255.0f, 51 / 255.0f, 19 / 255.0f, 1), new Color(251 / 255.0f, 255 / 255.0f, 250 / 255.0f, 1));
    }
}
