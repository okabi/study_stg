using UnityEngine;
using System.Collections;

/// <summary>
///   プレイヤー弾に関連するパラメータをまとめたクラス
/// </summary>
public class ShotStatus : MonoBehaviour {
    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>移動角度(度)</summary>
    public float angle;

    /// <summary>攻撃力</summary>
    public int power;
}
