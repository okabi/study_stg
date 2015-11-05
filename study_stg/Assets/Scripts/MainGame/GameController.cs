using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ゲームの制御
/// </summary>
public class GameController : MonoBehaviour {
    /// <summary>プレイヤーのステータス</summary>
    public PlayerStatus playerStatus;

    /// <summary>ゲーム時間表示用UI(デバッグ用)</summary>
    public UIOutlinedText TimeCount;
    
    /// <summary>スコア表示用UI</summary>
    public UIOutlinedText UIScore;

    /// <summary>アタッチされているGameStatus</summary>
    private GameStatus gameStatus;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        gameStatus = GetComponent<GameStatus>();
        gameStatus.rand = new System.Random();
        gameStatus.count = 0;
    }


    void Update()
    {
        UIScore.Text = "Score " + playerStatus.score.ToString();
        gameStatus.count += 1;
        TimeCount.Text = string.Format("Time {0} (frame: {1})", gameStatus.count / 60, gameStatus.count);
    }
}
