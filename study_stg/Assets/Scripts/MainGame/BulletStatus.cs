using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   敵弾に関連するパラメータをまとめたクラス
/// </summary>
public class BulletStatus : MonoBehaviour {
    /// <summary>出現からの経過フレーム</summary>
    public int count;

    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>移動角度(度)</summary>
    public float angle;

    /// <summary>画面外に出た時に消えるか</summary>
    public bool isDespawnable;

    /// <summary>どの敵機から発射された弾か(推定モデル用)</summary>
    public Define.EnemyTag tag;
}
