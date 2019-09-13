using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleGrazeCollision : MonoBehaviour {

    private PlayerManager player_Manager;
    private AudioSource graze_Sound;

    private float time = 0;
    private float span = 0.05f;


	// Use this for initialization
	void Start () {
        player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        graze_Sound = GetComponent<AudioSource>();
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "EnemyBulletTag") {
            if(time < span) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                player_Manager.Get_Score();
                graze_Sound.Play();
            }
        }
    }
}
