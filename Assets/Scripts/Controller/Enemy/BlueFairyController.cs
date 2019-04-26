using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFairyController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;


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
            _rigid.velocity = new Vector2(-37f, 0);
        }
        //左端に行ったら消す
        if (transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }
}
