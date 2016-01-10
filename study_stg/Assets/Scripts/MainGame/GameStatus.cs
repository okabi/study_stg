using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>ゲームオーバーからのフレーム</summary>
    public int gameoverCount;

    /// <summary>結果発表開始からのフレーム</summary>
    public int resultCount;

    /// <summary>Resources 内部の弾の画像</summary>
    public Dictionary<Define.BulletImageType, Sprite> bulletSprites;

    /// <summary>次に生成する敵のID</summary>
    public Dictionary<Define.EnemyTag, int> nextEnemyID;
}
