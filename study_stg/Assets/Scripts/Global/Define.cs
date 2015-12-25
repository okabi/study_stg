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

        /// <summary>真値・推定モデル用の敵機タグ</summary>
        public enum EnemyTag
        {
            /// <summary>最初に出てくる青ヘリと赤ヘリ</summary>
            Scene0 = 0,
            /// <summary>回転する青ヘリ</summary>
            Scene1,
            /// <summary>飛行機と同時に出てくる青ヘリ</summary>
            Scene2,
            /// <summary>中ボス前の飛行機</summary>
            Scene3,
            /// <summary>中ボス前の赤ヘリ</summary>
            Scene4,
            /// <summary>中ボス後の青ヘリと赤ヘリ</summary>
            Scene5,
            /// <summary>ボス前の飛行機</summary>
            Scene6,
            /// <summary>中ボス及び中ボスの早回しで出てくる赤ヘリ</summary>
            MidBoss,
            /// <summary>ボス戦(第一段階，自機狙い)</summary>
            Boss0,
            /// <summary>ボス戦(第二段階，まっすぐ)</summary>
            Boss1,
            /// <summary>ボス戦(第三段階，黄色ヘリが出てくる攻撃)</summary>
            Boss2,
            /// <summary>ボス戦(第四段階，横移動～突っ込む～元の位置に戻る)</summary>
            Boss3,
            /// <summary>ボス戦(第五段階，2ループ目)</summary>
            Boss4,
            /// <summary>ボス戦(発狂)</summary>
            Boss5,
            /// <summary>ボス戦(黄色ヘリ自体)</summary>
            BossYellowHeli
        }

        /// <summary>効果音のID</summary>
        public enum SoundID
        {
            /// <summary>ボス撃破時の効果音</summary>
            BossDie = 0,
            /// <summary>敵機(ボス除く)撃破時の効果音</summary>
            EnemyDie,
            /// <summary>自機被弾時の効果音</summary>
            PlayerDie,
            /// <summary>ボス登場演出時の効果音</summary>
            BossApproach,
            /// <summary>レーザー発射時の効果音</summary>
            Laser,
            /// <summary>ロックオン時の効果音</summary>
            Lockon,
            /// <summary>ショット命中時の効果音</summary>
            ShotAttach,
            /// <summary>ショット発射時の効果音</summary>
            Shot,
            /// <summary>着火の効果音</summary>
            Fire
        }

        /// <summary>BGMのID</summary>
        public enum BGMID
        {
            Stage1 = 0,
            Boss1
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