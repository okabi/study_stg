using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace StudySTG
{
    /// <summary>縁取りされたテキスト</summary>
    public class UIOutlinedText : MonoBehaviour
    {
        /// <summary>アタッチされているRectTransform</summary>
        public RectTransform rectTransform;

        /// <summary>メインテキスト</summary>
        public Text mainText;

        /// <summary>縁取り</summary>
        public Outline outline;

        /// <summary>表示するテキスト</summary>
        public string Text
        {
            get
            {
                return mainText.text;
            }
            set
            {
                mainText.text = value;
            }
        }

        /// <summary>表示する座標(スクリーン座標系)</summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(rectTransform.anchoredPosition.x + Define.ScreenSizeX / 2.0f, -rectTransform.anchoredPosition.y + Define.ScreenSizeY / 2.0f);

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
                return (int)(255.0f * mainText.color.a);
            }
            set
            {
                mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, value / 255.0f);
                outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, value / 255.0f);
            }
        }

        /// <summary>メインテキストの色</summary>
        public Color MainColor
        {
            get
            {
                return mainText.color;
            }
            set
            {
                mainText.color = new Color(value.r, value.g, value.b, mainText.color.a);
            }
        }

        /// <summary>縁取りの色</summary>
        public Color OutlineColor
        {
            get
            {
                return outline.effectColor;
            }
            set
            {
                outline.effectColor = new Color(value.r, value.g, value.b, outline.effectColor.a);
            }
        }

        /// <summary>フォントサイズ</summary>
        public int FontSize
        {
            get
            {
                return (int)(mainText.fontSize * 640.0f / Screen.width);
            }
            set
            {
                mainText.fontSize = (int)(value * Screen.width / 640.0f);
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

        /// <summary>フォントスタイル(Bold, Italic)</summary>
        public FontStyle Style
        {
            get
            {
                return mainText.fontStyle;
            }
            set
            {
                mainText.fontStyle = value;
            }
        }

        /// <summary>テキストをどの位置に揃えるか</summary>
        public TextAnchor Alignment
        {
            get
            {
                return mainText.alignment;
            }
            set
            {
                mainText.alignment = value;
            }
        }


        /// <summary>インスタンス生成時に呼ぶ初期化関数</summary>
        /// <param name="message">表示するメッセージ</param>
        /// <param name="position">文字列を表示する座標(スクリーン座標系，ピボット左上)</param>
        /// <param name="mainColor">メイン部分の文字色</param>
        /// <param name="outlineColor">縁の文字色</param>
        /// <param name="fontSize">フォントサイズ</param>
        public void Init(string message, Vector2 position, Color mainColor, Color outlineColor, int fontSize)
        {
            Text = message;
            Position = position;
            MainColor = mainColor;
            OutlineColor = outlineColor;
            FontSize = fontSize;
            Alpha = 255;
        }
    }
}
