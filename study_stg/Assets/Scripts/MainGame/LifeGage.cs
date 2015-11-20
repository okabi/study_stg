using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   敵機のライフゲージ
/// </summary>
public class LifeGage : MonoBehaviour
{
    /// <summary>HPゲージのスプライト</summary>
    public Sprite gageSprite;

    /// <summary>UIImageのプレハブ</summary>
    public GameObject UIImagePrefab;

    /// <summary>ライフゲージ(下地，本体)</summary>
    private UIImage[] gage;


    void Awake()
    {
        gage = new UIImage[2];
        GameObject parent = GameObject.Find("Game Canvas");
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(UIImagePrefab);
            obj.transform.SetParent(parent.transform);
            gage[i] = obj.GetComponent<UIImage>();
        }
    }


    void OnDestroy()
    {
        foreach (var g in gage)
        {
            if (g != null)
            {
                Destroy(g.gameObject);
            }
        }
    }


    /// <summary>
    ///   毎フレーム，Controllerから呼ぶ関数．ライフゲージを更新する．
    /// </summary>
    /// <param name="pos">ゲージを表示する座標(左上)</param>
    /// <param name="life">ライフ残量</param>
    /// <param name="maxLife">最大ライフ</param>
    public void UpdateParameter(Vector2 pos, int life, int maxLife)
    {
        if (life < maxLife)
        {
            Vector2 gageBaseSize = new Vector2(50.0f, 4.0f);
            Vector2 gageSize = new Vector2(gageBaseSize.x * life / maxLife, gageBaseSize.y);
            gage[0].Init(gageSprite, pos + gageBaseSize / 2, gageBaseSize);
            gage[0].Blend = new Color(0.4f, 0, 0);
            gage[1].Init(gageSprite, pos + gageSize / 2, gageSize);
            gage[1].Blend = new Color(0.8f, 0.8f, 0);
        }
    }
}
