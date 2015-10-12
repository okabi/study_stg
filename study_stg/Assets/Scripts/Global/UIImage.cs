using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace StudySTG
{
    /// <summary>UI画像(ピボットは中心)</summary>
    public class UIImage : MonoBehaviour
    {
        /// <summary>アタッチされているRectTransform</summary>
        public RectTransform rectTransform;

        /// <summary>アタッチされているUIImage</summary>
        public Image image;

        /// <summary>表示する画像スプライト</summary>
        public Sprite ImageSprite
        {
            get
            {
                return image.sprite;
            }
            set
            {
                image.sprite = value;
            }
        }

        /// <summary>表示する座標(スクリーン座標系)</summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(rectTransform.anchoredPosition.x + Define.ScreenSizeX / 2.0f, -rectTransform.anchoredPosition.y - Define.ScreenSizeY / 2.0f);

            }
            set
            {
                rectTransform.anchoredPosition = new Vector2(value.x - Define.ScreenSizeX / 2.0f, -value.y + Define.ScreenSizeY / 2.0f);
            }
        }

        /// <summary>αブレンド値(0~255)</summary>
        public int Alpha
        {
            get
            {
                return (int)(255.0f * image.color.a);
            }
            set
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, value / 255.0f);
            }
        }

        /// <summary>RGBブレンド値</summary>
        public Color Blend
        {
            get
            {
                return image.color;
            }
            set
            {
                image.color = new Color(value.r, value.g, value.b);
            }
        }

        /// <summary>画像サイズ(スクリーン座標系)</summary>
        public Vector2 Size
        {
            get
            {
                return new Vector2(rectTransform.sizeDelta.x * 640.0f / Screen.width, rectTransform.sizeDelta.y) * 480.0f / Screen.height;
            }
            set
            {
                rectTransform.sizeDelta = new Vector2(Screen.width / 640.0f * value.x, Screen.height / 480.0f *  value.y);
            }
        }

        /// <summary>画像拡大率</summary>
        public float Scale
        {
            get
            {
                return rectTransform.localScale.x;
            }
            set
            {
                rectTransform.localScale = new Vector3(value, value, value);
            }
        }


        /// <summary>インスタンス生成時に呼ぶ初期化関数</summary>
        /// <param name="sprite">表示する画像スプライト</param>
        /// <param name="position">画像を表示する座標(スクリーン座標系，ピボット中心)</param>
        /// <param name="size">画像の大きさ(スクリーン座標系)</param>
        public void Init(Sprite sprite, Vector2 position, Vector2 size)
        {
            Position = position;
            ImageSprite = sprite;
            Size = size;
            Alpha = 255;
            Blend = new Color(1.0f, 1.0f, 1.0f);
        }

    }
}
