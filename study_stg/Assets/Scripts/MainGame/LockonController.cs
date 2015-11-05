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
        if (lockonStatus.enemyStatus == null)
        {
            Destroy(gameObject);
            return;
        }

        // 座標の調整とアニメーション
        lockonStatus.enemyStatus.lockonMultiply = lockonStatus.multiply;
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
        drawingStatus.PositionScreen = lockonStatus.enemyStatus.lockonEffectPosition;
        lineRenderer.SetPosition(0, Utility.ScreenToWorld(playerDrawingStatus.PositionScreen));
        lineRenderer.SetPosition(1, Utility.ScreenToWorld(lockonStatus.enemyStatus.lockonEffectPosition));
        drawingStatus.Rotation -= 15.0f;
        lockonStatus.count += 1;
    }


    /// <summary>
    ///   初期化関数
    /// </summary>
    /// <param name="playerDrawingStatus">プレイヤーのDrawingStatus</param>
    /// <param name="enemyDrawingStatus">照準の当たっている敵のDrawingStatus</param>
    /// <param name="multiply">照準の倍率</param>
    public void Initialize(DrawingStatus playerDrawingStatus, DrawingStatus enemyDrawingStatus, int multiply)
    {
        drawingStatus.PositionScreen = enemyDrawingStatus.PositionScreen;
        this.playerDrawingStatus = playerDrawingStatus;
        lockonStatus.multiply = multiply;
        lockonStatus.enemyStatus = enemyDrawingStatus.gameObject.GetComponent<EnemyStatus>();
        lineRenderer.SetPosition(0, Utility.ScreenToWorld(playerDrawingStatus.PositionScreen));
        lineRenderer.SetPosition(1, Utility.ScreenToWorld(lockonStatus.enemyStatus.lockonEffectPosition));
        lineRenderer.SetColors(new Color(230 / 255.0f, 51 / 255.0f, 19 / 255.0f, 1), new Color(251 / 255.0f, 255 / 255.0f, 250 / 255.0f, 1));
    }
}
