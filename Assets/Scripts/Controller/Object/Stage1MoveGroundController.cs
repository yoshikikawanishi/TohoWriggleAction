using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1MoveGroundController : MonoBehaviour {

    
    //位相
    private float phase = 0;
    //初期位置
    private Vector3 default_Pos;


	// Use this for initialization
	void Start () {
        default_Pos = transform.position;
    }


    //FixedUpdate
    private void FixedUpdate() {
        transform.position = default_Pos + new Vector3(Mathf.Sin(phase) * 64f, 0, 0);
        phase += 0.01f;

    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(transform);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        //1面動く床用
        if(collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(null);
        }
    }

}
