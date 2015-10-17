using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>敵の照準のステータス</summary>
public class LockonStatus : MonoBehaviour {
    /// <summary>照準の当たっている敵のDrawingStatus</summary>
    public DrawingStatus enemyDrawingStatus;

    /// <summary>生成されてからのカウント</summary>
    public int count;
}
