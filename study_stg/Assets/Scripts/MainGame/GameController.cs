using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ゲームの制御
/// </summary>
public class GameController : MonoBehaviour {
    /// <summary>プレイヤーのステータス</summary>
    public PlayerStatus playerStatus;

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
        UIScore.Init("", new Vector2(0, 0), new Color(1, 1, 1), new Color(0, 0, 0), 18);
    }


    void Update()
    {
        UIScore.Text = "Score " + playerStatus.score.ToString();
        gameStatus.count += 1;
    }
}
