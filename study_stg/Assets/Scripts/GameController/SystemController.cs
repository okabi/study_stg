using UnityEngine;
using System.Collections;

/// <summary>
///   垂直同期設定など裏方設定を行う
/// </summary>
public class SystemController : MonoBehaviour {
    /// <summary>表示FPS</summary>
    private float fps;

    /// <summary>Intervalフレームの瞬間FPS合計値</summary>
    private float fpsSum;

    /// <summary>表示FPS更新用カウント</summary>
    private int count;

    /// <summary>FPS表示を更新するフレーム間隔</summary>
    private const int Interval = 50;


    void Awake()
    {
        count = Interval;
    }


    void FixedUpdate()
    {
        fpsSum += 1.0f / Time.deltaTime;
        count -= 1;
        if (count == 0)
        {
            fps = fpsSum / (float)Interval;
            fpsSum = 0.0f;
            count = Interval;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), fps.ToString("f1") + "fps");
    }
}
