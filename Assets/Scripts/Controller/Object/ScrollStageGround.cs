using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//強制スクロールの面の地面
public class ScrollStageGround : MonoBehaviour {

    //自機、カメラ
    private GameObject player;
    private GameObject main_Camera;

    //Start
    private void Start() {
        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.Find("Main Camera");
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            player.transform.SetParent(null);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            player.transform.SetParent(main_Camera.transform);
        }
    }

}
