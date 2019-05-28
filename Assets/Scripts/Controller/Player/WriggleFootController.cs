using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleFootController : PlayerFootController {

    private WriggleController wriggle_Controller;


    private new void Start() {
        base.Start();
        wriggle_Controller = transform.GetComponentInParent<WriggleController>();
    }

    private new void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        if (collision.tag == "GroundTag") {
            if (!wriggle_Controller.is_Ground && !wriggle_Controller.is_Fly) {
                wriggle_Controller.is_Ground = true;
                landing_Sound.Play();
                wriggle_Controller.StartCoroutine("Heal_Fly_Time");
            }
        }
        if (collision.tag == "ThroughGroundTag" && !wriggle_Controller.is_Fly) {
            if (!player_Controller.is_Ground) {
                player_Controller.is_Ground = true;
                player_Controller.StartCoroutine("Heal_Fly_Time");
            }
        }
    }
}
