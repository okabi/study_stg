using UnityEngine;
using System.Collections;

/// <summary>
///   2Dオブジェクトの描画に関連する情報を纏めたクラス．
///   ゲーム上のオブジェクト全てにこのスクリプトをアタッチすること．
/// </summary>
namespace StudySTG
{
    public class DrawingStatus : MonoBehaviour
    {
        /// <summary>アタッチされているSpriteRenderer</summary>
        public SpriteRenderer sprite;

        /// <summary>世界座標のZ座標</summary>
        public float worldZ;

        /// <summary>3Dモデル利用時に使う．マテリアルが存在する子オブジェクト</summary>
        public GameObject[] models;

        /// <summary>3Dモデル利用時に使うRenderer</summary>
        private Renderer[] modelRenderers;

        /// <summary>左上(0.0f, 0.0f)，右下(640.0f, 480.0f)としたピクセル座標</summary>
        public Vector2 PositionScreen
        {
            get
            {
                Vector3 v = StudySTG.Utility.WorldToScreen(transform.position);
                return new Vector2(v.x, v.y);
            }
            set
            {
                transform.position = StudySTG.Utility.ScreenToWorld(value, worldZ);
            }
        }

        /// <summary>回転角度(度)</summary>
        public float Rotation
        {
            get
            {
                return transform.eulerAngles.z;
            }
            set
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, value);
            }
        }

        /// <summary>拡縮率．1.0fがオリジナルサイズ</summary>
        public float Scale
        {
            get
            {
                return (transform.localScale.x + transform.localScale.y) / 2.0f;
            }
            set
            {
                transform.localScale = new Vector3(value, value);
            }
        }

        /// <summary>X軸方向の拡縮率．1.0fがオリジナルサイズ</summary>
        public float ScaleX
        {
            get
            {
                return transform.localScale.x;
            }
            set
            {
                transform.localScale = new Vector3(value, transform.localScale.y);
            }
        }

        /// <summary>Y軸方向の拡縮率．1.0fがオリジナルサイズ</summary>
        public float ScaleY
        {
            get
            {
                return transform.localScale.y;
            }
            set
            {
                transform.localScale = new Vector3(transform.localScale.x, value);
            }
        }

        /// <summary>αブレンド値(0～255)</summary>
        public int Alpha
        {
            get
            {
                if (sprite != null)
                {
                    return (int)(sprite.color.a * 255);
                }
                else
                {
                    return (int)(modelRenderers[0].material.GetColor("_Color").a);
                }
            }
            set
            {
                if (sprite != null)
                {
                    Color color = sprite.color;
                    color.a = value / 255.0f;
                    sprite.color = color;
                }
                else
                {
                    foreach (Renderer r in modelRenderers)
                    {
                        Color color = r.material.GetColor("_Color");
                        color.a = value / 255.0f;
                        r.material.SetColor("_Color", color);
                    }
                }
            }
        }

        /// <summary>ブレンドカラー(RGB)</summary>
        public Color Blend
        {
            get
            {
                if (sprite != null)
                {
                    return sprite.color;
                }
                else
                {
                    return modelRenderers[0].material.GetColor("_Color");
                }
            }
            set
            {
                if (sprite != null)
                {
                    sprite.color = new Color(value.r, value.g, value.b, Alpha);
                }
                else
                {
                    foreach (Renderer r in modelRenderers)
                    {
                        r.material.SetColor("_Color", new Color(value.r, value.g, value.b, Alpha));
                    }
                }
            }
        }


        void Awake()
        {
            modelRenderers = new Renderer[models.Length];
            for (int i = 0; i < models.Length; i++ )
            {
                modelRenderers[i] = models[i].GetComponent<Renderer>();
                modelRenderers[i].material.EnableKeyword("_Color");
            }
        }
    }
}