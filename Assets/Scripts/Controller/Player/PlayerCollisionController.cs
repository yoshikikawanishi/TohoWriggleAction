using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;
    
    //スクリプト
    protected PlayerManager _playerManager;

    //自機
    private GameObject player;
    //ダメージエフェクト
    [SerializeField] private GameObject damaged_Bomb;
    //死亡エフェクト
    [SerializeField] private GameObject miss_Effect;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 1.5f;

    //被弾の判定
    private bool damaged_Trigger = false;
    //ミスの判定
    protected bool miss_Trigger = false;


	// Use this for initialization
	protected void Start () {
        //コンポーネントの取得
        _renderer = GetComponentInParent<Renderer>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //自機
        player = transform.parent.gameObject;

    }


    //OnTriggerEnter
    protected void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            Damaged(1);
        }
        //即死
        else if(collision.tag == "MissZoneTag") {
            Damaged(_playerManager.life + 2);
        }
        
    }


    //OnCollisionEnter
    protected void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            Damaged(1);
        }
    }


    //被弾時の処理
    protected void Damaged(int damage) {
        if (!damaged_Trigger) {
            damaged_Trigger = true; 
            //ライフを減らす
            _playerManager.life -= damage;
            //死亡時の処理
            if (_playerManager.life <= 0 && !miss_Trigger) {
                gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
                Miss();
                miss_Trigger = true;
            }
            else {
                //反動
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(-player.transform.localScale.x, 2) * 100f;
                //ボムエフェクト
                GameObject bomb = Instantiate(damaged_Bomb) as GameObject;
                bomb.transform.position = transform.position;
                //点滅、無敵化
                StartCoroutine("Blink");
            }
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
        damaged_Trigger = false;
    }


    //ライフが0になった時の処理
    private void Miss() {
        //エフェクト
        GameObject effect = Instantiate(miss_Effect) as GameObject;
        effect.transform.position = transform.position;
        //死亡と復活
        _playerManager.stock--;
        if (_playerManager.stock > 0) {
            _playerManager.StartCoroutine("Revive");
        }
        //ゲームオーバー
        else {
            Invoke("Game_Over", 1.0f);
        }
        player.SetActive(false);
    }


    //ゲームオーバー
    private void Game_Over() {
        SceneManager.LoadScene("GameOverScene");
    }


}
