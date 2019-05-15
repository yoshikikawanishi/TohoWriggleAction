using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpItemController : MonoBehaviour {

    //Start
    private void Start() {
        StartCoroutine("Blink");
    }

    //点滅
    private IEnumerator Blink() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        while (true) {
            yield return new WaitForSeconds(0.2f);
            _sprite.color = new Color(1, 0, 0);
            yield return new WaitForSeconds(0.5f);
            _sprite.color = new Color(1, 0.3f, 0.3f);
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().Life_Up();
            Destroy(gameObject);
        }
    }

}
