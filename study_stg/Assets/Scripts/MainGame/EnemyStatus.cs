using UnityEngine;
using System.Collections;

/// <summary>
///   敵に関連するパラメータをまとめたクラス
/// </summary>
public class EnemyStatus : MonoBehaviour {
    /// <summary>出現からの経過フレーム</summary>
    public int count;

    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>移動角度(度)</summary>
    public float angle;

    /// <summary>体力</summary>
    public int life;

    /// <summary>体力の初期値</summary>
    public int maxLife;

    /// <summary>画面外に出た時に消えるか</summary>
    public bool isDespawnable;

    /// <summary>プレイヤーにロックオンされてるか</summary>
    public bool isLockon;

    /// <summary>直前フレームでダメージを受けたか</summary>
    public bool isDamage;

    /// <summary>撃破時に入るスコア</summary>
    public int score;
}
