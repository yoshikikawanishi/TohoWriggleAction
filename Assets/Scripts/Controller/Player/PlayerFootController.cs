using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour {

    //オーディオ
    protected AudioSource landing_Sound;
    //自機
    protected PlayerController player_Controller;


	// Use this for initialization
	protected void Start () {
        //オーディオの取得
        landing_Sound = GetComponents<AudioSource>()[0];
        //自機の取得
        player_Controller = transform.GetComponentInParent<PlayerController>();
	}
	

    //OnTriggerStay
    protected void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (collision.tag == "GroundTag") {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
                landing_Sound.Play();
            }
        }
        if (collision.tag == "ThroughGroundTag") {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
            }
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        //接地判定
        if (collision.tag == "GroundTag" || collision.tag == "ThroughGroundTag") {
            if (player_Controller.is_Ground) {
                player_Controller.is_Ground = false;
            }
        }
    }


}
