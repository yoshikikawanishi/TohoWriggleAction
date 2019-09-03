using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveBetweenTwoPoints))]
public class WolfRush : MonoBehaviour {

    //目標地点
    private Vector2 aim_Pos;

    //突進終了
    private bool end_Rush = false;


	// Use this for initialization
	void Start () {

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

        //エフェクト
        Effect();
        //移動
        MoveBetweenTwoPoints _move = GetComponent<MoveBetweenTwoPoints>();
        _move.Start_Move(aim_Pos, 0, 0.050f);
        yield return new WaitUntil(_move.End_Move);

        //元に戻す
        end_Rush = true;
    }


    //エフェクト
    private void Effect() {
        AngleTwoPoints _angle = new AngleTwoPoints();
        float angle = _angle.Cal_Angle_Two_Points(transform.position, aim_Pos);
        GameObject rush_Effect = transform.Find("RushEffect").gameObject;
        rush_Effect.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        rush_Effect.GetComponent<ParticleSystem>().Play();
    }


    //突進終了検知用
    public bool End_Rush() {
        if (end_Rush) {
            return true;
        }
        return false;
    }
    

}
