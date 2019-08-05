using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepRedFairy : MonoBehaviour {

    //コンポーネント
    private BulletFunctions _bullet;
    private Rigidbody2D _rigid;
    private Renderer _renderer;

    //初期位置
    private float default_Y;

    //ショット
    private bool start_Action = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _bullet = GetComponent<BulletFunctions>();
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();

        //初期位置代入
        default_Y = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        //画面内に入ったら動き出す
        if (_renderer.isVisible && !start_Action) {
            start_Action = true;
            _rigid.velocity = new Vector2(-37f, 0);
            StartCoroutine("Shot");
        }
        //左端に行ったら消す
        if (transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }


    //FixedUpdate
    private void FixedUpdate() {
        if (_renderer.isVisible) {
            Swing();
        }
    }


    //振動
    private void Swing() {
        transform.position = new Vector2(transform.position.x, default_Y + Mathf.Sin(Time.time*2) * 16f);
    }


    //ショット
    private IEnumerator Shot() {
        yield return new WaitForSeconds(3.0f);
        _bullet.Set_Bullet(Resources.Load("Bullet/PurpleBullet") as GameObject);
        _bullet.Odd_Num_Bullet(3, 24f, 80f, 5.0f);
    }
}
