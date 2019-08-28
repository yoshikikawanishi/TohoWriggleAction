using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashBlockController : MonoBehaviour {

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag") {
            if(transform.childCount != 0) {
                //エフェクト出す
                Effect();
                Destroy(gameObject);
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "DamageGroundTag") {
            if (transform.childCount != 0) {
                //エフェクト出す
                Effect();
                Destroy(gameObject);
            }
        }
    }


    //エフェクト
    private void Effect() {
        GameObject effect = transform.GetChild(0).gameObject;
        if (effect.GetComponent<AudioSource>() != null) {
            effect.transform.SetParent(null);
            AudioClip clip = effect.GetComponent<AudioSource>().clip;
            effect.GetComponent<AudioSource>().PlayOneShot(clip);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 1.0f);
        }
    }


}
