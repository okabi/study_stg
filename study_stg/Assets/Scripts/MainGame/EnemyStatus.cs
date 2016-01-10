using UnityEngine;
using System.Collections;
using StudySTG;

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

    /// <summary>プレイヤーにロックオンされている時，そのレーザーで受ける予定のダメージ</summary>
    public int lockonDamage;

    /// <summary>レーザーで倒した時の予定倍率(ロックオン順．高いものが優先)</summary>
    public int lockonMultiply;

    /// <summary>過去のロックオン数で倒される予定か(倍率リセットバグ対策)</summary>
    public bool isToBeDestroyedByLaser;

    /// <summary>直前フレームでダメージを受けたか</summary>
    public bool isDamage;

    /// <summary>撃破時に入るスコア</summary>
    public int score;

    /// <summary>マテリアルの元々のblend</summary>
    public Color originalBlend;

    /// <summary>ロックオンの対象となる座標(スクリーン座標系)</summary>
    public Vector2[] lockonTargetPosition;

    /// <summary>ロックオンの結果照準が表示される座標(スクリーン座標系)</summary>
    public Vector2 lockonEffectPosition;

    /// <summary>ボスの部位か？</summary>
    public bool isBossPart;

    /// <summary>真値・推定モデル用のタグ</summary>
    public Define.EnemyTag tag;

    /// <summary>プレイヤーとの最小距離(推定モデル用)</summary>
    public float minPlayerDistance;

    /// <summary>プレイヤーにロックオンされた倍率(推定モデル用)</summary>
    public int lockoned;

    /// <summary>敵機ID(タグごと)</summary>
    public int ID;
}
