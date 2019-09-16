using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour {

    //対象
    private GameObject target;
    private Rigidbody2D target_Rigid;

    private bool is_Vacuuming = false;

    private float power = 0;

    private Vector2 direction;

	
	// LateUpdate
	void LateUpdate () {
		if(is_Vacuuming && target != null) {
            direction = -(target.transform.position - transform.position).normalized;
            target_Rigid.AddForce(direction * power);
        }
	}


    //吸い寄せ開始
    public void Start_Vacuum(GameObject target, float power) {
        this.target = target;
        target_Rigid = target.GetComponent<Rigidbody2D>();
        this.power = power;
        if (target_Rigid == null) {
            Debug.Log("Vacuum target don't has Rigidbody2D");
            return;
        }
        is_Vacuuming = true;
    }


    //吸い寄せ終了
    public void Stop_Vacuum() {
        power = 0;
        is_Vacuuming = false;
    }
}
