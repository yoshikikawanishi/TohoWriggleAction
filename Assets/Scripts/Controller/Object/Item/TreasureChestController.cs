using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private SpriteRenderer _sprite;
    //オーディオ
    private AudioSource appear_Sound;
    private AudioSource open_Sound;
    //スクリプト
    private PlayerManager _playerManager;

    //状態
    private enum STATE {
        wait,
        appear,
        open,
    }
    private STATE state = STATE.wait;

    //耐久
    private int life = 6;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //オーディオの取得
        appear_Sound = GetComponents<AudioSource>()[0];
        open_Sound = GetComponents<AudioSource>()[1];
        //初期設定
        _sprite.color = new Color(1, 1, 1, 0);
    }
	

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(state != STATE.open) {
           if(collision.tag == "PlayerBulletTag") {
                Damaged(1);
           } 
           else if(collision.tag == "PlayerAttackTag" || collision.tag == "BeetleBulletTag") {
                Damaged(5);
           }
        }
        
    }


    //被弾
    private void Damaged(int damage) {
        life -= damage;
        StartCoroutine("Blink");
        //一回弾に当たると、宝箱を出現させる
        if (state == STATE.wait) {
            Appear_Chest();
            state = STATE.appear;
        }
        //宝箱の耐久が0になったら開ける
        if (life <= 0 && state == STATE.appear) {
            StartCoroutine("Open_Chest");
            state = STATE.open;
        }
    }


    //宝箱が現れる
    private void Appear_Chest() {
        _anim.SetTrigger("AppearTrigger");
        appear_Sound.Play();
        _sprite.color = new Color(1, 1, 1, 1);
    }


    //宝箱を開く
    private IEnumerator Open_Chest() {
        _anim.SetBool("OpenBool", true);
        open_Sound.Play();
        yield return new WaitForSeconds(0.5f);
        //出すアイテムの決定
        string item_Name = "Flies";
        do {
            switch (Random.Range(0, 4)) {
                case 0: item_Name = "Flies"; break;
                case 1: item_Name = "Bee"; break;
                case 2: item_Name = "Beetle"; break;
                case 3: item_Name = "ButterFly"; break;
            }
        } while (item_Name == _playerManager.option_Type);
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
