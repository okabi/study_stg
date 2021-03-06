﻿using UnityEngine;
using System.Collections;
using StudySTG;

/// <summary>
///   めっちゃ弱い敵(青色ヘリ)
///   動き: 左(右)から登場した後，円弧を描いて上方へ退散する
///   ショット: なし
/// </summary>
public class EnemyPattern6 : MonoBehaviour
{
    /// <summary>ヘリのローター</summary>
    public GameObject rotorControl;

    /// <summary>敵モデルのy軸方向の回転角度</summary>
    public float modelAngle;

    /// <summary>0なら左から，1なら右から登場</summary>
    public int movePattern;

    ///<summary>アタッチされているDrawingStatusスクリプト</summary>
    private DrawingStatus drawingStatus;

    /// <summary>敵のステータス</summary>
    private EnemyStatus enemyStatus;

    ///<summary>プレイヤーの情報を持つPlayerStatusスクリプト</summary>
    private PlayerStatus playerStatus;


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
            modelAngle = 90.0f;
            enemyStatus.speed = 2.0f;
            enemyStatus.angle = 180 * movePattern;
        }
        else if (count < 120) { }
        else if (count < 180)
        {
            if (movePattern == 0)
            {
                enemyStatus.angle -= 2.0f;
            }
            else
            {
                enemyStatus.angle += 2.0f;
            }
        }

        // モデル回転
        Vector2 playerPos = playerStatus.drawingStatus.PositionScreen;
        float deltaAngle = (float)System.Math.Atan2(playerPos.y - drawingStatus.PositionScreen.y, playerPos.x - drawingStatus.PositionScreen.x) * Mathf.Rad2Deg;
        transform.Rotate(0.0f, -(modelAngle - deltaAngle), 0.0f);
        modelAngle -= modelAngle - deltaAngle;
        rotorControl.transform.Rotate(0.0f, 0.0f, 15.0f);
    }
}
