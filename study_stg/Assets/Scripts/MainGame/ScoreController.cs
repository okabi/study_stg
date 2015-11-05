using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>敵機撃破後のスコア表示</summary>
public class ScoreController : MonoBehaviour {
    /// <summary>UITextのプレハブ</summary>
    public GameObject UITextPrefab;

    /// <summary>スコア表示UI</summary>
    private UIOutlinedText scoreText;

    /// <summary>生成されてからのカウント</summary>
    private int count;


    void Awake()
    {
        count = 0;
    }


    void Update()
    {
        if (count < 10)
        {
            scoreText.Position += new Vector2(0, -2);
        }
        else if (count < 50)
        {
            scoreText.Position += new Vector2(0, -0.5f);
        }
        else if (count < 80)
        {
            scoreText.Position += new Vector2(0, -0.5f);
            scoreText.Alpha = (int)(255.0f * (80 - count) / 30);
        }
        else
        {
            Destroy(scoreText.gameObject);
            Destroy(gameObject);
        }
        count += 1;
    }


    /// <summary>
    ///   初期化関数
    /// </summary>
    /// <param name="position">スコア表示を生成する座標(スクリーン座標系)</param>
    /// <param name="score">表示するスコア</param>
    public void Initialize(Vector2 position, int score)
    {
        scoreText = Instantiate(UITextPrefab).GetComponent<UIOutlinedText>();
        scoreText.gameObject.transform.SetParent(GameObject.Find("Game Canvas").transform);
        scoreText.Init(score.ToString(), position, new Color(1, 1, 1), new Color(0, 0, 0), 16);
    }
}
