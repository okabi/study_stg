using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ボス登場時の影のエフェクト
/// </summary>
public class BossEffect : MonoBehaviour {
    /// <summary>アタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>生成されてからのカウント</summary>
    private int count;

    /// <summary>効果音再生用</summary>
    private AudioController audio;


    void Awake()
    {
        drawingStatus = GetComponent<DrawingStatus>();
        audio = GameObject.Find("AudioController").GetComponent<AudioController>();
        audio.PlaySoundEffect(Define.SoundID.BossApproach);
        count = 0;
        transform.SetParent(GameObject.Find("Effects").transform);
    }


    void Update()
    {
        if (count < 120)
        {
            drawingStatus.PositionScreen += new Vector2(0, -15);
        }
        else
        {
            Destroy(gameObject);
        }

        count += 1;
    }


    /// <summary>
    ///   初期化関数
    /// </summary>
    /// <param name="position">生成する座標</param>
    public void Initialize(Vector2 position)
    {
        drawingStatus.PositionScreen = position;
        drawingStatus.Alpha = 230;
    }
}
