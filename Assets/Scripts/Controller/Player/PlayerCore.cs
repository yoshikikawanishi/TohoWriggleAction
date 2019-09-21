using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

    //スクリプト
    private WriggleCollision _collision;

    private float time = 0;


	// Use this for initialization
	void Start () {
        _collision = transform.parent.GetChild(0).GetComponent<WriggleCollision>();	
	}


    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "GroundTag" || collision.gameObject.tag == "ScreenWallTag") {
            if (time <= 0.1f) {
                time += Time.deltaTime;
            }
            else {
                _collision.Miss();
            }
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "GroundTag" || collision.gameObject.tag == "ScreenWallTag") {
            time = 0;
        }
    }
}
