using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveBetweenTwoPoints))]
public class WolfRush : MonoBehaviour {

    //コンポーネント
    private KagerouController _controller;

    //目標地点
    private Vector2 aim_Pos;

    //突進終了
    private bool end_Rush = false;


    private void Awake() {
        //取得
        _controller = GetComponent<KagerouController>();
    }


    //突進開始
    public void Start_Rush(Vector2 aim_Pos) {
        end_Rush = false;
        StopCoroutine("Rush");
        this.aim_Pos = aim_Pos;
        StartCoroutine("Rush");
    }


    //突進
    private IEnumerator Rush() {
        //見た目変更
        _controller.Change_Parametar("RushBool", 1);
        //エフェクト
        Effect();
        //回転
        Rotate();
        //移動
        MoveBetweenTwoPoints _move = GetComponent<MoveBetweenTwoPoints>();
        _move.Start_Move(aim_Pos, 0, 0.050f);
        yield return new WaitUntil(_move.End_Move);
        //元に戻す
        _controller.Change_Parametar("IdleBool", 1);
        end_Rush = true;
    }


    //エフェクト
    private void Effect() {
        GameObject rush_Effect = transform.Find("RushEffect").gameObject;
        rush_Effect.GetComponent<ParticleSystem>().Play();
    }


    //進行方向を向く
    private void Rotate() {
        transform.LookAt2D(aim_Pos, Vector2.up);
    }


    //突進終了検知用
    public bool End_Rush() {
        if (end_Rush) {
            return true;
        }
        return false;
    }
    

}
