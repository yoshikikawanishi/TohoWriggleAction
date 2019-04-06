using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour {

    //自機
    private PlayerController player_Controller;


	// Use this for initialization
	void Start () {
        //自機の取得
        player_Controller = transform.GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (collision.tag == "GroundTag" && !player_Controller.is_Ground) {
            player_Controller.is_Ground = true;
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        //接地判定
        if(collision.tag == "GroundTag" && player_Controller.is_Ground) {
            player_Controller.is_Ground = false;
        }
    }


}
