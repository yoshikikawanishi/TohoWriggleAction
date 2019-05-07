using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigid;

    //耐久
    private int life = 10;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();

        //初期設定
        _sprite.color = new Color(1, 1, 1, 0);
    }
	

	// Update is called once per frame
	void Update () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag") {
            life--;
            StartCoroutine("Blink");
            if(life == 9) {
                Appear_Chest();
            }
            if(life == 0) {
                Open_Chest();
            }
        }
    }


    //宝箱が現れる
    private void Appear_Chest() {
        _anim.SetTrigger("AppearTrigger");
        _sprite.color = new Color(1, 1, 1, 1);
        _rigid.velocity = new Vector2(0, 30f);
    }


    //宝箱を開く
    private void Open_Chest() {
        _anim.SetBool("OpenBool", true);
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
    }

}
