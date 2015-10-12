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
        /// <param name="z">世界座標のz座標</param>
        /// <returns>変換後の世界座標</returns>
        public static Vector3 ScreenToWorld(Vector2 screen, float z = 0.0f)
        {
            float x = 6.4f * screen.x / StudySTG.Define.ScreenSizeX - 3.2f;
            float y = 2.4f - 4.8f * screen.y / StudySTG.Define.ScreenSizeY;
            return new Vector3(x, y, z);
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


        /// <summary>指定した座標に引き伸ばした画像を表示するようなPosition及びScaleを設定する</summary>
        /// <param name="min">左上座標</param>
        /// <param name="max">右下座標</param>
        /// <param name="ds">対象となるDrawingStatus</param>
        public static void ExtendScreenPosition(Vector2 min, Vector2 max, ref DrawingStatus ds)
        {
            ds.PositionScreen = new Vector2((min.x + max.x) / 2.0f, (min.y + max.y) / 2.0f);
            float sizeX = max.x - min.x;
            float sizeY = max.y - min.y;
            ds.ScaleX = sizeX / ds.sprite.sprite.texture.width;
            ds.ScaleY = sizeY / ds.sprite.sprite.texture.height;
        }
    }
}
