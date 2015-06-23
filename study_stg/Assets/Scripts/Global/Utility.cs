using UnityEngine;
using System.Collections;

namespace StudySTG
{
    /// <summary>
    ///   便利関数群
    /// </summary>
    public static class Utility
    {
        /// <summary>
        ///   スクリーン座標から世界座標に変換する
        /// </summary>
        /// <param name="screen">スクリーン座標</param>
        /// <returns>変換後の世界座標</returns>
        public static Vector3 ScreenToWorld(Vector2 screen)
        {
            float x = 6.4f * screen.x / StudySTG.Define.ScreenSizeX - 3.2f;
            float y = 2.4f - 4.8f * screen.y / StudySTG.Define.ScreenSizeY;
            return new Vector3(x, y, 0.0f);
        }


        /// <summary>
        ///   世界座標からスクリーン座標に変換する
        /// </summary>
        /// <param name="world">世界座標</param>
        /// <returns>変換後のスクリーン座標</returns>
        public static Vector2 WorldToScreen(Vector3 world)
        {
            float x = StudySTG.Define.ScreenSizeX * (world.x + 3.2f) / 6.4f;
            float y = StudySTG.Define.ScreenSizeY * (-world.y + 2.4f) / 4.8f;
            return new Vector2(x, y);
        }
    }
}
