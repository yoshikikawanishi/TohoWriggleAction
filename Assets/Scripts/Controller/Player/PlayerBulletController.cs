using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //OnEnable
    private void OnEnable() {
        StartCoroutine("Delete");
    }
    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "EnemyTag" || collision.tag == "GroundTag") {
            gameObject.SetActive(false);
        }
    }

    //弾を消す
    private IEnumerator Delete() {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }

}
