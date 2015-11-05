using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>ホーミングレーザーのステータス</summary>
public class LaserStatus : MonoBehaviour {
    /// <summary>各パーツの基準座標(スクリーン座標系)</summary>
    public Vector2[] verticesPosition;

    /// <summary>発射地点の座標</summary>
    public Vector2 startPosition;

    /// <summary>着弾地点の座標</summary>
    public Vector2 destinationPosition;

    /// <summary>ターゲットとなる敵のEnemyStatus</summary>
    public EnemyStatus enemyStatus;

    /// <summary>生成されてからのカウント</summary>
    public int count;

    /// <summary>攻撃力</summary>
    public int power;

    /// <summary>ホーミングレーザーの最大プラス角度(度)</summary>
    public float plusAngle;

    /// <summary>敵機に当たったか</summary>
    public bool isCollision;

    /// <summary>敵のサーチを止めた後の固定角度(度)</summary>
    public float fixedAngle;
}
