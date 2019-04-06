using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedController : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;

    //スクリプト
    private PlayerManager _playerManager;
    private PlayerController _playerController;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 1.5f;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _renderer = GetComponent<Renderer>();
        //スクリプトの取得
        _playerManager = GameObject.Find("CommonScripts").GetComponent<PlayerManager>();
        _playerController = GetComponent<PlayerController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            StartCoroutine("Damaged");
        }
    }
    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            StartCoroutine("Damaged");
        }
    }


    //被弾時の処理
    private IEnumerator Damaged() {
        //ライフを減らす
        _playerManager.life--;
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        //点滅
        for (int i = 0; i < 10; i++) {
            _renderer.enabled = false;
            yield return new WaitForSeconds(INVINCIBLE_TIME / 20f);
            _renderer.enabled = true;
            yield return new WaitForSeconds(INVINCIBLE_TIME / 20f);
        }
        yield return new WaitForSeconds(0.5f);
        //無敵解除
        gameObject.layer = LayerMask.NameToLayer("PlayerLayer");
    }


}
