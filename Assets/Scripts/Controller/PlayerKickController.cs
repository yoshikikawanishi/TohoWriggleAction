using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickController : MonoBehaviour {

    //自機
    private PlayerController player_Controller;


	// Use this for initialization
	void Start () {
        player_Controller = GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //FixedUpdate
    private void FixedUpdate() {
        player_Controller.is_Hit_Kick = false;
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyTag") {
            player_Controller.is_Hit_Kick = true;
        }
    }
}
