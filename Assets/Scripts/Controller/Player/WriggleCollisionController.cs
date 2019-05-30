using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleCollisionController : PlayerCollisionController {

    //オーディオ
    private AudioSource get_Item_Sound;

    //カメラ
    private GameObject main_Camera;
    //被弾時の電流エフェクト
    private GameObject bolt_Effect;


    // Use this for initialization
    new void Start () {
        base.Start();
        //オーディオの取得
        get_Item_Sound = GetComponent<AudioSource>();
        //カメラとダメージエフェクト
        main_Camera = GameObject.Find("Main Camera");
        bolt_Effect = Instantiate(Resources.Load("Effect/BoltEffect")) as GameObject;
        bolt_Effect.transform.SetParent(main_Camera.transform);
        bolt_Effect.transform.localPosition = new Vector3(0, 0, 10);
    }
	

    //OnTriggerEnter
    private new void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            Damaged(1);
        }
        //即死
        else if (collision.tag == "MissZoneTag") {
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
    private new void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            Damaged(1);
        }
    }


    //被弾時の処理
    protected new void Damaged(int damage) {
        base.Damaged(damage);
        //powerを減らす
        Reduce_Power();
        if (!miss_Trigger) {
            //電流エフェクト
            bolt_Effect.GetComponent<ParticleSystem>().Play();
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
