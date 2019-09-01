using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleBulletController : MonoBehaviour {

    //弾の種類分け
    [SerializeField] private string option_Type;
	

    //OnEnable
    private void OnEnable() {     
        switch (option_Type) {
            case "Flies": StartCoroutine("Flies_Bullet"); break;
            case "ButterFly": StartCoroutine("ButterFly_Bullet"); break;
            case "Bee": StartCoroutine("Bee_Bullet"); break;
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //蜂弾は敵貫通
        if (option_Type == "Bee") {
            if (collision.tag == "GroundTag") {
                gameObject.SetActive(false);
            }
        }
        //カブトムシ弾、敵地面に当たったら破片を出す
        else if(option_Type == "Beetle") {
            if(collision.tag == "GroundTag" || collision.tag == "EnemyTag" || collision.tag == "SandbagTag") {
                if(this.transform.childCount == 0) {
                    return;
                }
                GameObject effect = transform.GetChild(0).gameObject;
                effect.SetActive(true);
                effect.transform.SetParent(null);
                Destroy(effect, 0.3f);
                Destroy(gameObject);
            }
        }
        //ハエ、蝶弾敵地面に当たったら消す
        else if (collision.tag == "EnemyTag" || collision.tag == "GroundTag" || collision.tag == "SandbagTag") {
            gameObject.SetActive(false);
        }
    }


    //ハエ弾
    private IEnumerator Flies_Bullet() {
        yield return new WaitForSeconds(0.35f);
        gameObject.SetActive(false);
    }


    //蜂弾
    private IEnumerator Bee_Bullet() {
        yield return new WaitForSeconds(0.40f);
        gameObject.SetActive(false);
    }


    //蝶弾、サインカーブする
    private IEnumerator ButterFly_Bullet() {
        yield return null;
        float center_Height = transform.position.y;
        for(float t = 0; t < 0.42f; t += Time.deltaTime) {
            transform.position = new Vector3(transform.position.x, center_Height + Mathf.Sin(t * 15) * 64f);
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
