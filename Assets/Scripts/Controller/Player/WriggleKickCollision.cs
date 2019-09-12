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
	

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyTag" || collision.tag == "SandbagTag") {
            //当たり判定
            player.layer = LayerMask.NameToLayer("InvincibleLayer");
            Invoke("Change_Layer", Time.deltaTime * 3f);
            //通知
            player_Controller.Set_Is_Hit_Kick(true);
            //エフェクト
            Play_Effect();
        }
    }


    //当たり判定を戻す
    private void Change_Layer() {
        player.layer = LayerMask.NameToLayer("PlayerLayer");
    }


    //エフェクト
    private void Play_Effect() {
        GetComponent<ParticleSystem>().Play();

    }
}
