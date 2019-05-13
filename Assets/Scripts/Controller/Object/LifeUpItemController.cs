using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpItemController : MonoBehaviour {


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().Life_Up();
            Destroy(gameObject);
        }
    }

}
