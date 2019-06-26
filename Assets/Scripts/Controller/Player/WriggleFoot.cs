using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleFoot : PlayerFoot {

    private WriggleController wriggle_Controller;


    private new void Start() {
        base.Start();
        wriggle_Controller = transform.GetComponentInParent<WriggleController>();
    }

    private new void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (collision.tag == "GroundTag") {
            if (!wriggle_Controller.is_Ground && !wriggle_Controller.is_Fly && player_Rigid.velocity.y < 30f) {
                wriggle_Controller.is_Ground = true;
                landing_Sound.Play();
                wriggle_Controller.StartCoroutine("Heal_Fly_Time");
            }
        }
        //すり抜け床
        if (collision.tag == "ThroughGroundTag" && !wriggle_Controller.is_Fly && player_Rigid.velocity.y < 30f) {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
                landing_Sound.Play();
                player_Controller.StartCoroutine("Heal_Fly_Time");
            }
        }
        //ダメージ床
        if(collision.tag == "DamageGroundTag") {
            if(!wriggle_Controller.is_Ground && !wriggle_Controller.is_Fly) {
                player_Controller.is_Ground = true;
                landing_Sound.Play();
                player_Controller.StartCoroutine("Heal_Fly_Time");
            }
        }
    }
}
