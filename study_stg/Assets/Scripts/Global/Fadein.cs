using UnityEngine;
using System.Collections;

namespace StudySTG
{
    /// <summary>フェードイン等の処理</summary>
    public class Fadein : MonoBehaviour
    {
        /// <summary>UIImageのプレハブ</summary>
        public GameObject UIImagePrefab;

        /// <summary>フェードイン処理で利用する画像スプライト</summary>
        public Sprite fadeSprite;

        /// <summary>フェードインで利用しているインスタンスのUIImage</summary>
        private UIImage fadeImage;

        /// <summary>生成されてからのカウント</summary>
        private int count;

        /// <summary>最大輝度(0~255)</summary>
        private int maxAlpha;

        /// <summary>フェードアウト(画面を最大輝度で覆う)までのカウント</summary>
        private int fadeoutCount;

        /// <summary>フェードインが始まるカウント</summary>
        private int fadeinStartCount;

        /// <summary>フェードインが終了する(このオブジェクトが消滅する)カウント</summary>
        private int fadeinEndCount;


        void Update()
        {
            if (count < fadeoutCount)
            {
                fadeImage.Alpha = (int)((float)count * maxAlpha / fadeoutCount);
            }
            else if (count < fadeinStartCount)
            {
                fadeImage.Alpha = maxAlpha;
            }
            else if (count < fadeinEndCount)
            {
                int c = fadeinEndCount - count;
                float per = maxAlpha / (fadeinEndCount - fadeinStartCount);
                fadeImage.Alpha = (int)(c * per);
            }
            else
            {
                Destroy(fadeImage.gameObject);
                Destroy(this.gameObject);
            }
            count += 1;
        }


        /// <summary>インスタンス生成時に呼び出す初期化関数</summary>
        /// <param name="color">フェードの色</param>
        /// <param name="maxAlpha">フェードの最大輝度</param>
        /// <param name="fadeoutCount">フェードアウト(画面を最大輝度で覆う)までのカウント()</param>
        /// <param name="fadeinStartCount">フェードインが始まるカウント</param>
        /// <param name="fadeinEndCount">フェードインが終了する(このオブジェクトが消滅する)カウント</param>
        public void Init(Color color, int maxAlpha, int fadeoutCount, int fadeinStartCount, int fadeinEndCount)
        {
            GameObject obj = Instantiate(UIImagePrefab);
            obj.transform.SetParent(GameObject.Find("Fade Canvas").transform);
            fadeImage = obj.GetComponent<UIImage>();
            fadeImage.Init(fadeSprite, new Vector2(Define.ScreenSizeX / 2, Define.ScreenSizeY / 2), new Vector2(Define.ScreenSizeX + 5, Define.ScreenSizeY));
            count = 0;
            fadeImage.Blend = color;
            fadeImage.Alpha = 0;
            this.maxAlpha = maxAlpha;
            this.fadeoutCount = fadeoutCount;
            this.fadeinStartCount = fadeinStartCount;
            this.fadeinEndCount = fadeinEndCount;
        }
    }
}