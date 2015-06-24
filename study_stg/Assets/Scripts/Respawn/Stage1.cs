using UnityEngine;
using System.Collections;

/// <summary>敵の出現等を制御する(ステージ1)</summary>
public class Stage1 : MonoBehaviour {
    /// <summary>ステージ開始からの経過フレーム</summary>
    public int count;

    /// <summary>一番弱い敵</summary>
    public GameObject enemy0;
    
    void FixedUpdate()
    {
        if (count % 120 == 0)
        {
            GameObject enemy = Instantiate(enemy0);
            enemy.GetComponent<EnemyController0>().Initialize(new Vector2(StudySTG.Define.GameScreenCenterX, -16.0f));
        }
        count += 1;
    }
}
