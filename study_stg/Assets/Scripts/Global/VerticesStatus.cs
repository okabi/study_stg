using UnityEngine;
using System.Collections;

namespace StudySTG
{
    /// <summary>
    ///   2Dグラフィックにおいて，ポリゴンの頂点情報を保持する必要のあるオブジェクトにアタッチする．
    ///   すなわち，頂点情報を用いた変形を行う場合にアタッチする．
    /// </summary>
    public class VerticesStatus : MonoBehaviour
    {
        /// <summary>アタッチされているMeshFilter(Quad)</summary>
        public MeshFilter meshFilter;

        
        /// <summary>頂点座標(スクリーン座標系)を取得する．順に左下・右下・左上・右上</summary>
        /// <returns>頂点座標(スクリーン座標系)．順に左下・右下・左上・右上</returns>
        public Vector3[] GetVerticesPosition()
        {
            return meshFilter.mesh.vertices;
        }


        /// <summary>頂点座標(スクリーン座標系)を設定する</summary>
        /// <param name="UpperLeft">左上座標(スクリーン座標系)</param>
        /// <param name="UpperRight">右上座標(スクリーン座標系)</param>
        /// <param name="LowerLeft">左下座標(スクリーン座標系)</param>
        /// <param name="LowerRight">右下座標(スクリーン座標系)</param>
        public void SetVerticesPosition(Vector2 UpperLeft, Vector2 UpperRight, Vector2 LowerLeft, Vector2 LowerRight)
        {
            Vector3[] newVertices = new Vector3[4];
            newVertices[0] = (Vector3)Utility.ScreenToWorld(LowerLeft);
            newVertices[1] = (Vector3)Utility.ScreenToWorld(UpperRight);
            newVertices[2] = (Vector3)Utility.ScreenToWorld(LowerRight);
            newVertices[3] = (Vector3)Utility.ScreenToWorld(UpperLeft);
            meshFilter.mesh.vertices = newVertices;
            meshFilter.mesh.RecalculateBounds();
        }


        /// <summary>メッシュのUV情報(スプライトの描画範囲)を取得する</summary>
        /// <returns>メッシュのUV(順に左下・右下・左上・右上)</returns>
        public Vector2[] GetUV()
        {
            return meshFilter.mesh.uv;
        }


        /// <summary>メッシュのUV情報(スプライトの描画範囲)を設定する</summary>
        /// <param name="UpperLeft">左上</param>
        /// <param name="UpperRight">右上</param>
        /// <param name="LowerLeft">左下</param>
        /// <param name="LowerRight">右下</param>
        public void SetUV(Vector2 UpperLeft, Vector2 UpperRight, Vector2 LowerLeft, Vector2 LowerRight)
        {
            Vector2[] newUV = new Vector2[4];
            newUV[0] = LowerLeft;
            newUV[1] = UpperRight;
            newUV[2] = LowerRight;
            newUV[3] = UpperLeft;
            meshFilter.mesh.uv = newUV;
            meshFilter.mesh.RecalculateBounds();
        }
    }
}