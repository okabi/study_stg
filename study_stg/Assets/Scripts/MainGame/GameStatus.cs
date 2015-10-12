using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ゲームの制御用ステータス
/// </summary>
public class GameStatus : MonoBehaviour {
    /// <summary>ゲームで利用する乱数</summary>
    public System.Random rand;

    /// <summary>現在のステージ</summary>
    public int stage;

    /// <summary>ゲーム内フレーム</summary>
    public int count;
}
