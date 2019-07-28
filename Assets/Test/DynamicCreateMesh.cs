//メッシュ生成テスト用
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//必要なコンポーネント
[RequireComponent(typeof(MeshRenderer))]    //必須
[RequireComponent(typeof(MeshFilter))]      //うれしい、安定化に必須
public class DynamicCreateMesh : MonoBehaviour {

    //メッシュにつけるマテリアル
    [SerializeField]
    private Material _mat;

    //中心座標
    public List<Vector2> center;
    //幅
    public float width = 32f;
    //長さ
    private float length = 500f;


    private void Start() {
        //メッシュ生成
        var mesh = new Mesh();

        int count = center.Count;
        Vector3[] vertices = new Vector3[count * 4];
        int[] triangles = new int[count * 6];
        
        //頂点座標
        for(int i = 0; i < count; i++) {
            vertices[4 * i] = center[i] + new Vector2(length / 2, width / 2);
            vertices[4 * i + 1] = center[i] + new Vector2(length / 2, -width / 2);
            vertices[4 * i + 2] = center[i] + new Vector2(-length / 2, -width / 2);
            vertices[4 * i + 3] = center[i] + new Vector2(-length / 2, width / 2);
        }

        //頂点インデックスの割り当て
        int positionIndex = 0;
        for (int i = 0; i < count; i++) {
            triangles[positionIndex++] = (i * 4) + 1;
            triangles[positionIndex++] = (i * 4) + 2;
            triangles[positionIndex++] = (i * 4) + 3;

            triangles[positionIndex++] = (i * 4) + 3;
            triangles[positionIndex++] = (i * 4) + 0;
            triangles[positionIndex++] = (i * 4) + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();      //法線ベクトルの決定（おまじないとみなしても良さそう）
        var filter = GetComponent<MeshFilter>();    //MeshFilterコンポーネント用
        filter.sharedMesh = mesh;

        //マテリアルの付与
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = _mat;

    }
}
