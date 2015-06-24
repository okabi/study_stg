using UnityEngine;
using System.Collections;

public class EnemyController0 : EnemyController {
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;


    /// <summary>
    ///   めっちゃ弱い敵
    /// </summary>
    protected override void Pattern()
    {
        int count = enemyStatus.count;

        // 移動に関するパターン
        if (count == 0)
        {
            enemyStatus.speed = 1.5f;
            enemyStatus.angle = 90.0f;
        }
        else if (count < 180) { }
        else if (count < 270)
        {
            enemyStatus.angle += 1.5f;
        }
        drawingStatus.Rotation = -(enemyStatus.angle + 90.0f);

        // ショットに関するパターン
        if (count == 120)
        {
            if (playerStatus != null)
            {
                // 自機狙い弾を撃つ
                float dx = playerStatus.PositionScreen.x - drawingStatus.PositionScreen.x;
                float dy = playerStatus.PositionScreen.y - drawingStatus.PositionScreen.y;
                float angle = (float)System.Math.Atan2((float)dy, (float)dx) * 180.0f / (float)System.Math.PI;
                GameObject bullet = Instantiate(bullet0);
                bullet.GetComponent<BulletController>().Initialize(drawingStatus.PositionScreen, 4.0f, angle);
            }
        }
    }
}
