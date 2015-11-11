﻿using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   ボス
/// </summary>
public class BossPattern0 : MonoBehaviour
{
    /// <summary>真っ直ぐな弾</summary>
    public GameObject bullet0;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;

    /// <summary>目的位置</summary>
    public Vector2 destination;

    /// <summary>初期位置</summary>
    private Vector2 start;


    void Awake()
    {
        // コンポーネントやオブジェクトの読み込み
        drawingStatus = GetComponent<DrawingStatus>();
        enemyStatus = GetComponent<EnemyStatus>();
        playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
    }


    void Update()
    {
        int count = enemyStatus.count;

        // 移動
        if (count == 0)
        {
            start = drawingStatus.PositionScreen;
        }
        else if (count < 90)
        {
            Vector2 deltaPos = start - destination;
            Vector2 a = deltaPos / (90 * 90);
            drawingStatus.PositionScreen = destination + a * (float)System.Math.Pow((90 - count), 2);
        }
        else if (count == 90)
        {
            enemyStatus.angle = 90;
        }
        else if (count < 120)
        {
            enemyStatus.speed += 0.03f;
        }

        // ショット
        if (count == 90)
        {
            // 自機狙い弾を撃つ
            Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
            float dx = playerPos.x - drawingStatus.PositionScreen.x;
            float dy = playerPos.y - drawingStatus.PositionScreen.y;
            float angle = (float)System.Math.Atan2((float)dy, (float)dx) * 180.0f / (float)System.Math.PI;
            GameObject bullet = Instantiate(bullet0);
            bullet.GetComponent<BulletController>().Initialize(Define.BulletImageType.MediumGreen, drawingStatus.PositionScreen, 4.0f, angle);
        }
    }
}
