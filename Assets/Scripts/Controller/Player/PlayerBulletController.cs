using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour {

    private PlayerManager _playerManager;


	// Use this for initialization
	void Start () {
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
	}
	

    //OnEnable
    private void OnEnable() {
        if(_playerManager == null) {
            _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        }
        switch (_playerManager.option_Type) {
            case "Flies": StartCoroutine("Flies_Bullet"); break;
            case "ButterFly": StartCoroutine("ButterFly_Bullet"); break;
        }
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "EnemyTag" || collision.tag == "GroundTag") {
            gameObject.SetActive(false);
        }
    }

    //ハエ弾
    private IEnumerator Flies_Bullet() {
        yield return new WaitForSeconds(0.35f);
        gameObject.SetActive(false);
    }

    //蝶弾
    private IEnumerator ButterFly_Bullet() {
        yield return null;
        float center_Height = transform.position.y;
        for(float t = 0; t < 0.8f; t += Time.deltaTime) {
            transform.position = new Vector3(transform.position.x, center_Height + Mathf.Sin(t * 15) * 32f);
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
