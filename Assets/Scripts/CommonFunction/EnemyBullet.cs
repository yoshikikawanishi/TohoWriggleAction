using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    //オブジェクトプールされる弾かどうか
    [SerializeField] private bool is_Pool = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //TriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //ボム、自機、壁に当たった時消す
        if(collision.tag == "BombTag" || collision.tag == "PlayerTag") {
            if (is_Pool) {
                gameObject.SetActive(false);
            }
            else {
                Destroy(gameObject);
            }
        }
    }


}
