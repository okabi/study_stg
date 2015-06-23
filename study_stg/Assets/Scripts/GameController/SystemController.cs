using UnityEngine;
using System.Collections;

/// <summary>
///   垂直同期設定など裏方設定を行う
/// </summary>
public class SystemController : MonoBehaviour {
    /// <summary>瞬間FPS</summary>
    private float fps;

    void Awake()
    {
        // 60FPS にしたい
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        fps = 1.0f / Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(567, 439, 43, 21), fps.ToString("f1") + "fps");
    }
}
