using UnityEngine;
using System.Collections;

namespace StudySTG
{
    /// <summary>
    ///   全シーンで残り続ける，セーブデータの中身
    /// </summary>
    public class SaveStatus : MonoBehaviour
    {
        /// <summary>上位10位までのハイスコア</summary>
        public int[] hiscore;

        /// <summary>クリアしたか</summary>
        public bool[] cleared;

        /// <summary>先ほどのプレイでのランキング(0ならランキング外)</summary>
        public int nowScoreRanking;

        /// <summary>リプレイ再生中か</summary>
        public bool replaying;
    }
}
