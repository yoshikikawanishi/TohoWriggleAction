using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareBullet : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private AudioSource damage_Sound;

    //スクリプト
    private ObjectDestroyer _destroy_Effect;

    //オブジェクト
    private GameObject doremy;
    private GameObject player;

    //ステータス
    private int life = 120;
    private bool is_Dive_To_Doremy = false;
    private Vector2 aim;


	// Use this for initialization
	void Start () {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        damage_Sound = GetComponent<AudioSource>();
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
        if (collision.tag == "PlayerBulletTag") {
            Damaged(1);
        }
        else if(collision.tag == "PlayerAttackTag") {
            Damaged(8);
        }
        else if(collision.tag == "BeetleBulletTag") {
            Damaged(8);
        }
        //ドレミーと衝突してはじける
        if(collision.gameObject == doremy && is_Dive_To_Doremy) {
            Crush();
        }
    }

    //被弾
    private void Damaged(int damage) {
        if (life >= 2) {
            StartCoroutine("Blink");
            life -= damage;
            damage_Sound.Play();
        }
        else if (!is_Dive_To_Doremy) {
            Dive_To_Doremy();
            is_Dive_To_Doremy = true;
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
        UsualSoundManager.Familiar_Appear_Sound();
        //外れた時用
        Destroy(gameObject, 5.0f);
    }

    //ドレミーと衝突してはじける
    private void Crush() {
        _destroy_Effect.Destroy_Object();
        UsualSoundManager.Shot_Sound();
    }


    


    
}
