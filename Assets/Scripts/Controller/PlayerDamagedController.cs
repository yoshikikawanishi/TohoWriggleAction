using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedController : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;

    //スクリプト
    private PlayerManager _playerManager;
    private PlayerController _playerController;

    //自機
    private GameObject player;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 1.5f;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _renderer = GetComponentInParent<Renderer>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
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
            StartCoroutine("Damaged", 1);
        }
        //即死
        else if(collision.tag == "MissZoneTag") {
            StartCoroutine("Damaged", _playerManager.life + 2);
        }
    }
    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            StartCoroutine("Damaged", 1);
        }
    }


    //被弾時の処理
    private IEnumerator Damaged(int damage) {
        //ライフを減らす
        _playerManager.life -= damage;
        //死亡時の処理
        if(_playerManager.life <= 0) {
            StartCoroutine("Miss");
        }
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        //反動
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(-player.transform.localScale.x, 2) * 100f;
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


    //ライフが0になった時の処理
    private IEnumerator Miss() {
        player.SetActive(false);
        yield return new WaitForSeconds(1.0f);
    }


}
