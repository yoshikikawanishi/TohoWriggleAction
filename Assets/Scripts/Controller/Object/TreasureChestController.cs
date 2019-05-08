﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private SpriteRenderer _sprite;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigid;

    //耐久
    private int life = 7;


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
	

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag") {
            life--;
            StartCoroutine("Blink");
            //一回弾に当たると、宝箱を出現させる
            if(life == 6) {
                Appear_Chest();
            }
            //宝箱の耐久が0になったら開ける
            if(life == 0) {
                StartCoroutine("Open_Chest");
            }
        }
    }


    //宝箱が現れる
    private void Appear_Chest() {
        _anim.SetTrigger("AppearTrigger");
        _sprite.color = new Color(1, 1, 1, 1);
        _rigid.gravityScale = 7f;
        _rigid.velocity = new Vector2(0, 50f);
        _collider.isTrigger = false;
    }


    //宝箱を開く
    private IEnumerator Open_Chest() {
        _anim.SetBool("OpenBool", true);
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(0.5f);
        //出すアイテムの決定
        string item_Name = "Flies";
        switch (Random.Range(0, 4)) {
            case 0: item_Name = "Flies"; break;
            case 1: item_Name = "Bees"; break;
            case 2: item_Name = "Beetles"; break;
            case 3: item_Name = "ButterFlys"; break;
        }
        //アイテムの生成
        GameObject item = Instantiate(Resources.Load("Object/" + item_Name)) as GameObject;
        item.transform.position = transform.position + new Vector3(0, 16f, 0);
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
    }

}
