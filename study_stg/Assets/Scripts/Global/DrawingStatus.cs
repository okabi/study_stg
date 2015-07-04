using UnityEngine;
using System.Collections;

/// <summary>
///   2Dオブジェクトの描画に関連する情報を纏めたクラス．
///   ゲーム上のオブジェクト全てにこのスクリプトをアタッチすること．
/// </summary>
public class DrawingStatus : MonoBehaviour
{
    /// <summary>アタッチされているSpriteRenderer</summary>
    public SpriteRenderer sprite;

    /// <summary>世界座標のZ座標</summary>
    public float worldZ;

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
            return transform.localScale.x;
        }
        set
        {
            transform.localScale = new Vector3(value, value);
        }
    }

    /// <summary>αブレンド値(0～255)</summary>
    public int Alpha
    {
        get
        {
            return (int)sprite.color.a;
        }
        set
        {
            Color color = sprite.color;
            color.a = value;
            sprite.color = color;
        }
    }

    /// <summary>ブレンドカラー(RGB)</summary>
    public Color Blend
    {
        get
        {
            return sprite.color;
        }
        set
        {
            sprite.color = value;
        }

    }

    /// <summary>アタッチされているSprite(画像)の大きさ</summary>
    public Vector2 SpriteSize { get; private set; }


    void Awake()
    {
        if (sprite != null)
        {
            float pixelsPerUnit = sprite.sprite.rect.width / sprite.bounds.size.x;
            SpriteSize = new Vector2(pixelsPerUnit * sprite.bounds.size.x, pixelsPerUnit * sprite.bounds.size.y);
        }
    }


    /// <summary>
    ///   スクリーン座標で座標を変化させる
    /// </summary>
    /// <param name="delta">変化量(スクリーン座標)</param>
    public void TranslateScreen(Vector2 delta)
    {
        PositionScreen += delta;
    }
}
