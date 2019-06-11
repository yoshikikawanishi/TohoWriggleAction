using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenTwoPoints : MonoBehaviour {

    //移動終了検知用
    private bool end_Move = false;
    //曲線移動の最大の高さ
    private float slerp_Height = 0;
    //最高速
    private float max_Speed = 0.02f;


    //移動開始
    public void Start_Move(Vector3 next_Pos, float height, float speed) {
        end_Move = false;
        slerp_Height = height;
        max_Speed = speed;
        StopCoroutine("Move_Two_Points");
        StartCoroutine("Move_Two_Points", next_Pos);
    }


    //移動用のコルーチン
    private IEnumerator Move_Two_Points(Vector3 next_Pos) {
        float speed = 0;    //速度
        float now_Location = 0; //現在の移動距離割合
        Vector3 start_Pos = transform.position;

        while (now_Location <= 1) {
            now_Location += speed * Time.timeScale;
            //直線
            transform.position = Vector3.Lerp(start_Pos, next_Pos, now_Location);
            //上下
            transform.position += new Vector3(0, slerp_Height * Mathf.Sin(now_Location * Mathf.PI), 0);
            //加速
            if (speed <= max_Speed && Time.timeScale != 0) {
                speed += 0.001f;
            }
            //減速
            if (now_Location >= 0.8f && Time.timeScale != 0) {
                speed *= 0.9f;
            }
            yield return null;
        }

        end_Move = true;
    }


    //移動終了検知用
    public bool End_Move() {
        if (end_Move) {
            end_Move = false;
            return true;
        }
        return false;
    }
}
