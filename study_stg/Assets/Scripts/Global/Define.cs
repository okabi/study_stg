using System.Collections;

namespace StudySTG
{
    /// <summary>
    ///   定数の定義用クラス
    /// </summary>
    public static class Define
    {
        /// <summary>画面の横サイズ(ピクセル)</summary>
        public const int ScreenSizeX = 640;

        /// <summary>画面の縦サイズ(ピクセル)</summary>
        public const int ScreenSizeY = 480;

        /// <summary>ゲーム画面の中央X座標(ピクセル)</summary>
        public const int GameScreenCenterX = 320;

        /// <summary>ゲーム画面の中央Y座標(ピクセル)</summary>
        public const int GameScreenCenterY = 240;

        /// <summary>ゲーム画面の横サイズ(ピクセル)</summary>
        public const int GameScreenSizeX = 640;

        /// <summary>ゲーム画面の縦サイズ(ピクセル)</summary>
        public const int GameScreenSizeY = 480;

        /// <summary>オブジェクトのタグを表す</summary>        
        public enum Tag
        {
            Player, Shot, Enemy, Bullet
        }

        /// <summary>タグ型から文字列に変換する</summary>        
        public static System.Collections.Generic.Dictionary<Tag, string> tagName;

        /// <summary>敵弾の画像タイプ</summary>
        public enum BulletImageType
        {
            MediumGreen,
            MediumPurple,
            BigRed,
            BigBlue
        }


        static void Awake()
        {
            tagName = new System.Collections.Generic.Dictionary<Tag, string>();
            tagName.Add(Tag.Player, "Player");
            tagName.Add(Tag.Shot, "Shot");
            tagName.Add(Tag.Enemy, "Enemy");
            tagName.Add(Tag.Bullet, "Bullet");
        }

    }
}