using UnityEngine;
using System.Collections;

public class EnemyController0 : EnemyController {
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    /// <summary>ヘリのローター</summary>
    public GameObject rotorControl;

    /// <summary>敵モデルのy軸方向の回転角度</summary>
    public float modelAngle;


    /// <summary>
    ///   めっちゃ弱い敵
    /// </summary>
    protected override void Pattern()
    {
        int count = enemyStatus.count;

        // 移動に関するパターン
        if (count == 0)
        {
            modelAngle = 90.0f;
            enemyStatus.speed = 1.5f;
            enemyStatus.angle = 90.0f;
        }
        else if (count < 180) { }
        else if (count < 270)
        {
            enemyStatus.angle += 1.5f;
        }
        
        // モデル回転に関するパターン
        float deltaAngle = (float)System.Math.Atan2(playerStatus.PositionScreen.y - drawingStatus.PositionScreen.y, playerStatus.PositionScreen.x - drawingStatus.PositionScreen.x) * Mathf.Rad2Deg;
        transform.Rotate(0.0f, -(modelAngle - deltaAngle), 0.0f);
        modelAngle -= modelAngle - deltaAngle;
        rotorControl.transform.Rotate(0.0f, 0.0f, 15.0f);
        //transform.rotation = Quaternion.LookRotation(StudySTG.Utility.ScreenToWorld(playerStatus.PositionScreen) - transform.position); ;

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
