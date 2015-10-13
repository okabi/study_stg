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

    /// <summary>プレイヤー弾(メイン)のプレハブ</summary>
    public GameObject MainShot;

    /// <summary>アタッチされているDrawingStatusスクリプト</summary>
    public DrawingStatus drawingStatus;

    /// <summary>敵機のEnemyControllerのリスト</summary>
    public List<EnemyController> enemyController;

    /// <summary>照準のDrawingStatusのリスト</summary>
    public List<DrawingStatus> lockonDrawingStatus;

    /// <summary>各ボタンを押し続けているフレーム数</summary>
    public Dictionary<CommandType, int> command;

    /// <summary>出現からの経過フレーム</summary>
    public int count;

    /// <summary>スコア</summary>
    public int score;

    /// <summary>移動スピード(フレームごと，スクリーン座標系)</summary>
    public float speed;

    /// <summary>残機</summary>
    public int life;

    /// <summary>連射用カウント</summary>
    public int rapid;

    /// <summary>溜め撃ち用円形エフェクトの半径</summary>
    public float circleRadius;
}
