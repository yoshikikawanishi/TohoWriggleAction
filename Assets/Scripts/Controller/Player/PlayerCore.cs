using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

    //スクリプト
    private WriggleCollision _collision;


	// Use this for initialization
	void Start () {
        _collision = transform.parent.GetChild(0).GetComponent<WriggleCollision>();	
	}


    //OnTriggerEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GroundTag") {
            _collision.Miss();
        }
    }
}
