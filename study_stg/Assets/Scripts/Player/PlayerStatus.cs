using UnityEngine;
using System.Collections;

/// <summary>
///   プレイヤーに関連するパラメータをまとめたクラス
/// </summary>
public class PlayerStatus : MonoBehaviour {
    /// <summary>プレイヤー弾(メイン)のプレハブ</summary>
    public GameObject MainShot;

    /// <summary>出現からの経過フレーム</summary>
    public int count;

    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>残機</summary>
    public int life;

    /// <summary>連射用カウント</summary>
    public int rapid;
}
