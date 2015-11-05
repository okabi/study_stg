using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>敵の照準のステータス</summary>
public class LockonStatus : MonoBehaviour {
    /// <summary>照準の当たっている敵のEnemyStatus</summary>
    public EnemyStatus enemyStatus;

    /// <summary>生成されてからのカウント</summary>
    public int count;

    /// <summary>照準の倍率</summary>
    public int multiply;
}
