using UnityEngine;
using System.Collections;

///<summary>当たり判定用のステータス</summary>
public class CollisionStatus : MonoBehaviour {
    /// <summary>当たり判定のタイプ</summary>
    public enum CollisionType
    {
        /// <summary>何もしない</summary>
        Nothing,
        /// <summary>当たった対象に対して攻撃を加える</summary>
        Atack,
        /// <summary>消滅する</summary>
        Disappear
    }

    ///<summary>当たり判定の左上スクリーン座標(中心からの差分)</summary>
    public Vector2 colliderMin;

    ///<summary>当たり判定の右下スクリーン座標(中心からの差分)</summary>
    public Vector2 colliderMax;

    /// <summary>画面外に出た時の処理</summary>
    public CollisionType outOfScreen;
}
