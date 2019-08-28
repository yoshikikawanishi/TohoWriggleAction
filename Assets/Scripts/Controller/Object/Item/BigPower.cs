using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPower : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            PlayerManager pm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
            pm.Set_Power(pm.power + 50);
            GameObject.FindWithTag("PlayerTag").transform.GetChild(7).GetComponents<AudioSource>()[0].Play();
            Destroy(gameObject);
        }
    }
}
