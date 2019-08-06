using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAreaEntrance : MonoBehaviour {

    //入口か出口か
    [SerializeField] private bool is_Entrance;


    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        //入口
        if (is_Entrance) {
            if (collision.tag == "PlayerBodyTag" && Input.GetKeyDown(KeyCode.DownArrow)) {
                GameObject.FindWithTag("PlayerTag").transform.position = new Vector3(-800f, -80f);
            }
        }
        //出口
        else {
            if (collision.tag == "PlayerBodyTag" && Input.GetKeyDown(KeyCode.DownArrow)) {
                GameObject.FindWithTag("PlayerTag").transform.position = new Vector3(800f, -80f);
            }
        }
    }

}
