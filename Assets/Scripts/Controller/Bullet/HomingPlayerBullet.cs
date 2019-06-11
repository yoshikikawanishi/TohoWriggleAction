using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPlayerBullet : MonoBehaviour {

    //自機
    private GameObject player;
    //コンポーネント
    private Rigidbody2D _rigid;


	// Use this for initialization
	void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		if(player != null) {
            transform.LookAt2D(player.transform, Vector2.right);
            _rigid.velocity = transform.right * 400f;
        }
	}
}
