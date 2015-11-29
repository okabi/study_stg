using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///   リプレイ情報を扱うクラス
/// </summary>
public class ReplayStatus : MonoBehaviour
{
    /// <summary>プレイ開始日時(DateTimeに変換可能)</summary>
    public long dateTime;

    /// <summary>そのプレイでの乱数の種</summary>
    public int seed;

    /// <summary>プレイランク(0:ゲームオーバー，1~4:C~S)</summary>
    public int rank;

    /// <summary>プレイヤーの入力</summary>
    public List<byte> playerInput;

    /// <summary>次に読み込む入力のインデックス</summary>
    public int inputIndex;
}
