using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuTalismanBullet : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;
    //霊夢
    private GameObject reimu;

    //一番近くの敵
    private GameObject target;


	// Use this for initialization
	void Awake () {
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        //霊夢
        reimu = GameObject.Find("Reimu");
        //一番近くの敵を探す
        Find_Nearest_Enemy();
    }


    //OnEnable
    private void OnEnable() {
        Find_Nearest_Enemy();
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }


    // Update is called once per frame
    void Update () {
        //敵をホーミング
		if(target != null && target.transform.position.x > reimu.transform.position.x + 32f) {
            To_Homing();
        }
        else {
            _rigid.velocity = transform.right * 500f;
        }
        
        
        //画面外で消す
        if (!_renderer.isVisible) {
            gameObject.SetActive(false);
        }
	}


    //霊夢より右にいる一番近くの敵を探す
    private void Find_Nearest_Enemy() {
        target = null;
        float min_Distance = 10000;
        float distance = 0;
        GameObject[] enemy_List = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (GameObject enemy in enemy_List) {
            distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < min_Distance && enemy.transform.position.x > reimu.transform.position.x) {
                min_Distance = distance;
                target = enemy;
            }
        }
    }


    //ホーミング
    private void To_Homing() {
        transform.LookAt2D(target.transform, Vector2.right);
        _rigid.velocity = transform.right * 500f;
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "GroundTag" || collision.tag == "EnemyTag") {
            gameObject.SetActive(false);
        }
    }

}
