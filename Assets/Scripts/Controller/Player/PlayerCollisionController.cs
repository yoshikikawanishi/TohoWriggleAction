﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;
    //オーディオ
    private AudioSource get_Item_Sound;

    //スクリプト
    private PlayerManager _playerManager;
    private PlayerController _playerController;
    private GameManager _gameManager;

    //自機
    private GameObject player;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 1.5f;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _renderer = GetComponentInParent<Renderer>();
        //オーディオの取得
        get_Item_Sound = GetComponent<AudioSource>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        _playerController = GetComponentInParent<PlayerController>();
        //自機
        player = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            Damaged(1);
        }
        //即死
        else if(collision.tag == "MissZoneTag") {
            Damaged(_playerManager.life + 2);
        }
        //点とPの獲得
        if (collision.tag == "ScoreTag") {
            _playerManager.Get_Score();
            get_Item_Sound.Play();
        }
        else if (collision.tag == "PowerTag") {
            _playerManager.Get_Power();
            get_Item_Sound.Play();
        }
    }
    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            Damaged(1);
        }
    }


    //被弾時の処理
    private void Damaged(int damage) {
        //ライフを減らす
        _playerManager.life -= damage;
        //powerを減らす
        Reduce_Power();
        //死亡時の処理
        if (_playerManager.life <= 0) {
            Miss();
        }
        else {
            //反動
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(-player.transform.localScale.x, 2) * 100f;
            //ボムエフェクト
            GameObject bomb = Instantiate(Resources.Load("Effect/PlayerDamagedBomb")) as GameObject;
            bomb.transform.position = transform.position;
            //点滅
            StartCoroutine("Blink");
        }
    }


    //powerを出す
    private void Reduce_Power() {
        int num = _playerManager.power / 4;
        float space = 800f / num;
        _playerManager.power /= 2;
        for(int i = 0; i < num; i++) {
            GameObject p = Instantiate(Resources.Load("Power")) as GameObject;
            p.transform.position = transform.position + new Vector3(0, 64f);
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(-400f + space * i, Random.Range(350f, 450f));
        }
    }


    //点滅、無敵化
    public IEnumerator Blink() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        for (int i = 0; i < 10; i++) {
            _renderer.enabled = false;
            yield return new WaitForSeconds(INVINCIBLE_TIME / 20f);
            _renderer.enabled = true;
            yield return new WaitForSeconds(INVINCIBLE_TIME / 20f);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = LayerMask.NameToLayer("PlayerLayer");

    }


    //ライフが0になった時の処理
    private void Miss() {
        //エフェクト
        GameObject effect = Instantiate(Resources.Load("Effect/PlayerMissEffect")) as GameObject;
        effect.transform.position = transform.position;
        //死亡と復活
        _playerManager.StartCoroutine("Revive");
        player.SetActive(false);        
    }


}
