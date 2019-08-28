using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFairyController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;

    //速度
    private float speed = -45f;


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }

	
	// Update is called once per frame
	void Update () {
        //画面内に入ったら動き出す
        if (_renderer.isVisible) {
            _rigid.velocity = new Vector2(speed * transform.localScale.x, _rigid.velocity.y);
        }
        //下に落ちたら消す
        if (transform.position.y < -240f) {
            Destroy(gameObject);
        }
        
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //反転用
        if (collision.tag == "InvisibleWallTag" || collision.tag == "EnemyTag") {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }
}
