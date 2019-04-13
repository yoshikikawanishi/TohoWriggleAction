using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {

    //種類番号
    [SerializeField] private int kind_Num = 0;

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;

    //自機
    //private GameObject player;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        //自機の取得
        //player = GameObject.FindWithTag("PlayerTag");

        switch (kind_Num) {
            case 1:RedFairy_Start(); break;
        }
    }
	

	// Update is called once per frame
	void Update () {
        
	}


    //FixedUpdate
    private void FixedUpdate() {

        switch (kind_Num) {
            case 1: RedFairy(); break;
            case 2: BlueFairy(); break;
        }

    }


    //RedFairy
    private void RedFairy_Start() {

    }
    private void RedFairy() {
        //画面内に入ったら動き出す
        if (_renderer.isVisible) {
            _rigid.velocity = new Vector2(-45f, 0);
        }
        //下に落ちたら消す
        if (transform.position.y < -160f) {
            Destroy(gameObject);
        }
    }


    //BlueFairy
    private void BlueFairy() {
        //画面内に入ったら動き出す
        if (_renderer.isVisible) {
            _rigid.velocity = new Vector2(-37f, 0);
        }
        //左端に行ったら消す
        if(transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }


}
