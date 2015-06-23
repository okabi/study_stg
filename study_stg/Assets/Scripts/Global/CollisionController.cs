using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///<summary>当たり判定総合クラス．CollisionStatusと共に，当たり判定を持つオブジェクトにアタッチする</summary>
public class CollisionController : MonoBehaviour {
    /// <summary>このオブジェクトにアタッチされているDrawingStatus</summary>
    private DrawingStatus drawingStatus;

    /// <summary>このオブジェクトにアタッチされているCollisionStatus</summary>
    private CollisionStatus collisionStatus;


    void Awake()
    {
        drawingStatus = GetComponent<DrawingStatus>();
        collisionStatus = GetComponent<CollisionStatus>();
    }


    void Update()
    {
        OutOfScreen();
    }


    /// <summary>オブジェクトが画面外に出た時の処理</summary>
    void OutOfScreen()
    {
        float x = drawingStatus.PositionScreen.x;
        float y = drawingStatus.PositionScreen.y;
        float scale = drawingStatus.Scale;
        float sizex = scale * drawingStatus.SpriteSize.x;
        float sizey = scale * drawingStatus.SpriteSize.y;

        if (x + sizex < 0.0f || x - sizex > StudySTG.Define.ScreenSizeX || y + sizey < 0.0f || y - sizey > StudySTG.Define.ScreenSizeY)
        {
            if (collisionStatus.outOfScreen == CollisionStatus.CollisionType.Disappear)
            {
                Destroy(gameObject);
            }
        }
           
    }

}
