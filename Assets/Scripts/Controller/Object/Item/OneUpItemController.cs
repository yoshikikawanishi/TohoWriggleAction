using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUpItemController : MonoBehaviour {


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            PlayerManager _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
            _playerManager.Get_Stock();
            Destroy(gameObject);
        }
    }
}
