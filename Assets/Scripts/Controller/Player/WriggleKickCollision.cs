using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleKickCollision : MonoBehaviour {

    //自機
    private GameObject player;
    private WriggleController player_Controller;


	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
        player_Controller = GetComponentInParent<WriggleController>();
	}
	

    //FixedUpdate
    private void FixedUpdate() {
        player_Controller.is_Hit_Kick = false;
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyTag") {
            player.layer = LayerMask.NameToLayer("InvincibleLayer");
            player_Controller.is_Hit_Kick = true;
            Invoke("Change_Layer", 0.5f);
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "EnemyTag") {
            player.layer = LayerMask.NameToLayer("InvincibleLayer");
            player_Controller.is_Hit_Kick = true;
            Invoke("Change_Layer", 0.5f);
        }
    }

    //当たり判定を戻す
    private void Change_Layer() {
        player.layer = LayerMask.NameToLayer("PlayerLayer");
    }
}
