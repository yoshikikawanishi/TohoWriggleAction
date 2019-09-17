using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareBullet : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;

    //スクリプト
    private ObjectDestroyer _destroy_Effect;

    //オブジェクト
    private GameObject doremy;
    private GameObject player;

    //ステータス
    private int life = 80;
    private bool is_Dive_To_Doremy = false;
    private Vector2 aim;


	// Use this for initialization
	void Start () {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _destroy_Effect = GetComponent<ObjectDestroyer>();
        doremy = GameObject.Find("Doremy");
        player = GameObject.FindWithTag("PlayerTag");   
	}
	
	// Update is called once per frame
	void Update () {
        //自機を追従
        if (!is_Dive_To_Doremy) {
            aim = player.transform.position - transform.position;
            _rigid.velocity = aim.normalized * 40f;
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾
        if (collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag" || collision.tag == "BeetleBulletTag") {
            if (life >= 2) {
                StartCoroutine("Blink");
                life--;
            }
            else if(!is_Dive_To_Doremy) {
                Dive_To_Doremy();
                is_Dive_To_Doremy = true;
            }
        }
        //ドレミーと衝突してはじける
        if(collision.gameObject == doremy && is_Dive_To_Doremy) {
            Crush();
        }
    }


    //点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.05f);
        _sprite.color = new Color(1, 1, 1);
    }


    //ドレミーのほうに飛んでいく
    private void Dive_To_Doremy() {
        GameObject effect = Instantiate(Resources.Load("Effect/PowerSpreadEffect") as GameObject);
        effect.transform.position = transform.position;
        Vector2 vector = (doremy.transform.position - transform.position).normalized;
        _rigid.velocity = vector * 400f;
        gameObject.tag = "PlayerAttackTag";
    }

    //ドレミーと衝突してはじける
    private void Crush() {
        _destroy_Effect.Destroy_Object();
    }


    


    
}
