using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootController : MonoBehaviour {

    //オーディオ
    protected AudioSource landing_Sound;
    //自機
    protected PlayerController player_Controller;
    //コンポーネント
    protected Rigidbody2D player_Rigid;


	// Use this for initialization
	protected void Start () {
        //オーディオの取得
        landing_Sound = GetComponents<AudioSource>()[0];
        //自機の取得
        player_Controller = transform.GetComponentInParent<PlayerController>();
        //コンポーネントの取得
        player_Rigid = transform.GetComponentInParent<Rigidbody2D>();
    }
	

    //OnTriggerStay
    protected void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (collision.tag == "GroundTag" && player_Rigid.velocity.y <= 5f) {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
                landing_Sound.Play();
            }
        }
        if (collision.tag == "ThroughGroundTag" && player_Rigid.velocity.y <= 5f) {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
                landing_Sound.Play();
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
