using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;

    //ライフ
    private int life = 2;

    //やられモーション開始
    private bool start_Motion = false;

    private float time = 0;
    private float move_Speed = 0.3f;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
	}

	
	//FixedUpdate
	void FixedUpdate () {
        //待機時
        if (!start_Motion) {
            Transition();
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾の判定
        if (collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag" || collision.tag == "BeetleBulletTag") {
            life--;
            //体力がなくなった時落下開始
            if(life <= 0 && !start_Motion) {
                start_Motion = true;
                Fall();
            }
        }
        //地面との衝突判定
        else if(collision.tag == "GroundTag") {
            //地面との衝突と点の発射
            StartCoroutine("Collide_Ground");
        }
    }


    //待機時の移動
    private void Transition() {
        if(time < 2.0f) {
            time += Time.deltaTime;
            transform.position += new Vector3(-move_Speed, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(time < 4.0f) {
            time += Time.deltaTime;
            transform.position += new Vector3(move_Speed, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else {
            time = 0;
        }
    }



    //落下
    private void Fall() {
        _rigid.gravityScale = 50f;
        _anim.SetBool("FallBool", true);
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }

    //地面との衝突と点の発射
    private IEnumerator Collide_Ground() {
        _rigid.gravityScale = 0;
        _rigid.velocity = new Vector2(0, 0);
        //点の発射
        for(int i = 0; i < 20; i++) {
            var score = Instantiate(Resources.Load("Score")) as GameObject;
            score.transform.position = transform.position;
            score.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-150f, 150f), Random.Range(400f, 600f));
        }
        yield return new WaitForSeconds(1.0f);
        //逃げる
        Escape();
    }

    //逃亡
    private void Escape() {
        _anim.SetBool("EscapeBool", true);
        _rigid.velocity = new Vector2(300f, 0);
        Destroy(gameObject, 1.5f);
    }
    


}
