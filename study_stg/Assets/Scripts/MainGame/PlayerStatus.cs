using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StudySTG;

/// <summary>
///   プレイヤーに関連するパラメータをまとめたクラス
/// </summary>
public class PlayerStatus : MonoBehaviour {
    /// <summary>プレイヤーの操作の種類</summary>
    public enum CommandType
    {
        Up = 0,
        Down,
        Left,
        Right,
        Shot,
        Charge
    }

    /// <summary>プレイヤーの状態の種類</summary>
    public enum StatusType
    {
        /// <summary>通常の状態</summary>
        Alive = 0,
        /// <summary>ミスからの復帰状態</summary>
        Miss
    }

    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    public DrawingStatus drawingStatus;

    /// <summary>敵機のEnemyControllerのリスト</summary>
    public List<EnemyController> enemyController;

    /// <summary>照準のDrawingStatusのリスト</summary>
    public List<DrawingStatus> lockonDrawingStatus;

    /// <summary>照準のlockonStatusのリスト</summary>
    public List<LockonStatus> lockonStatus;

    /// <summary>プレイヤーの状態</summary>
    public StatusType status;

    /// <summary>各ボタンを押し続けているフレーム数</summary>
    public Dictionary<CommandType, int> command;

    /// <summary>出現からの経過フレーム</summary>
    public int count;

    /// <summary>ミスからの経過フレーム</summary>
    public int missCount;

    /// <summary>1以上なら被弾してもミスにならない</summary>
    public int noDamageCount;

    /// <summary>スコア</summary>
    public int score;

    /// <summary>真値モデル用のタグに対する獲得点数</summary>
    public Dictionary<Define.EnemyTag, int> tagScore;

    /// <summary>ミスの要因となった敵</summary>
    public List<Define.EnemyTag> missFactor;

    /// <summary>ミスの時間</summary>
    public List<int> missTime;

    /// <summary>各敵との最小距離</summary>
    public Dictionary<Define.EnemyTag, Dictionary<int, float>> minDistance;

    /// <summary>各敵に対してロックオンしたか</summary>
    public Dictionary<Define.EnemyTag, Dictionary<int, int>> lockoned;

    /// <summary>各敵機の生存フレーム時間</summary>
    public Dictionary<Define.EnemyTag, Dictionary<int, int>> aliveTime;

    /// <summary>各敵機の残りHP / 最大HP</summary>
    public Dictionary<Define.EnemyTag, Dictionary<int, int[]>> enemyHP;

    /// <summary>ホーミングレーザーの威力</summary>
    public int laserPower;

    /// <summary>ロックオンの最大数</summary>
    public int lockonMaxNum;

    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>残機</summary>
    public int life;

    /// <summary>連射用カウント</summary>
    public int rapid;

    /// <summary>溜め撃ち用円形エフェクトの半径</summary>
    public float circleRadius;
}
