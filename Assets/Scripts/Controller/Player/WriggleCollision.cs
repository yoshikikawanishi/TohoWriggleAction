using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleCollision : PlayerCollision {

    //オーディオ
    private AudioSource get_Item_Sound;

    //スクリプト
    private PlayerManager _playerManager;

    //カメラ
    private GameObject main_Camera;
    //被弾時の電流エフェクト
    private GameObject bolt_Effect;


    // Use this for initialization
    new void Start () {
        base.Start();
        //オーディオの取得
        get_Item_Sound = GetComponent<AudioSource>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //カメラとダメージエフェクト
        main_Camera = GameObject.FindWithTag("MainCamera");
        bolt_Effect = Instantiate(Resources.Load("Effect/BoltEffect")) as GameObject;
        bolt_Effect.transform.SetParent(main_Camera.transform);
        bolt_Effect.transform.localPosition = new Vector3(0, 0, 10);
    }
	

    //OnTriggerEnter
    private new void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            Get_Hit();
        }
        //即死
        else if (collision.tag == "MissZoneTag") {
            Miss();
            Reduce_Power();
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
    private new void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag" || collision.gameObject.tag == "DamageGroundTag") {
            Get_Hit();
        }
    }

    
    //被弾
    protected new void Get_Hit() {
        if (!damaged_Trigger) {
            damaged_Trigger = true;
            Damaged(1);
        }
    }


    //被弾時の処理
    protected new void Damaged(int damage) {
        _playerManager.life -= damage;
        Reduce_Power();
        //体力が残るとき
        if (_playerManager.life > 0) {
            base.Damaged(damage);
            bolt_Effect.GetComponent<ParticleSystem>().Play();
        }
        //死亡時
        else {
            Miss();
        }
    }


    //死亡時の処理
    public override void Miss() {
        base.Miss();
        //死亡と復活
        _playerManager.stock--;
        if (_playerManager.stock > 0) {
            _playerManager.StartCoroutine("Revive");
        }
        //ゲームオーバー
        else {
            _playerManager.StartCoroutine("Game_Over");
        }
    }


    //powerを出す
    private void Reduce_Power() {
        int num = _playerManager.power / 4;
        float space = 800f / num;
        _playerManager.power /= 2;
        for (int i = 0; i < num; i++) {
            GameObject p = Instantiate(Resources.Load("Power")) as GameObject;
            p.transform.position = transform.position + new Vector3(0, 64f);
            p.GetComponent<Rigidbody2D>().velocity = new Vector2(-400f + space * i, Random.Range(350f, 450f));
        }
    }

}
