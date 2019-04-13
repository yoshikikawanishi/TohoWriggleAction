using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    //自機
    private GameObject player;
    //自機との距離
    private float distance;
    //自機との角度
    private Vector2 angle;

    //コンポーネント
    private Rigidbody2D _rigid;


    // Use this for initialization
    void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _rigid = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        //自機が近いと吸い付く
        if (player != null) {
            angle = player.transform.position - transform.position;
            distance = Mathf.Sqrt(angle.x * angle.x + angle.y * angle.y);
            if (distance < 64f) {
                _rigid.velocity = angle.normalized * 320f;
            }

            if (transform.position.y < -200f) {
                Destroy(gameObject);
            }
        }
    }


    //自機と当たったら消す
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            Destroy(gameObject);
        }
    }

}
