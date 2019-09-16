using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpItemController : MonoBehaviour {

    private bool is_Before_Delete = false;

    //Start
    private void Start() {
        StartCoroutine("Blink");
    }

    //点滅
    private IEnumerator Blink() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        while (!is_Before_Delete) {
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


    //点滅して消す
    public void Delete(float lifeTime) {
        StartCoroutine("Delete_Routine", lifeTime);
    }

    private IEnumerator Delete_Routine(float lifeTime) {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(lifeTime);
        is_Before_Delete = true;
        for (float t = 0.2f; t > 0.05f; t *= 0.9f) {
            _sprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(t);
            _sprite.color = new Color(1, 0, 0, 0);
            yield return new WaitForSeconds(t);
        }
    }

    

    

}
