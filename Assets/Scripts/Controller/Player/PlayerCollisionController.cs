using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;
    //オーディオ
    private AudioSource get_Item_Sound;

    //スクリプト
    private PlayerManager _playerManager;

    //自機
    private GameObject player;
    //カメラ
    private GameObject main_Camera;
    //ダメージエフェクト
    private GameObject bolt_Effect;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 1.5f;

    //被弾の判定
    private bool damaged_Trigger = false;
    //ミスの判定
    private bool miss_Trigger = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _renderer = GetComponentInParent<Renderer>();
        //オーディオの取得
        get_Item_Sound = GetComponent<AudioSource>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //自機
        player = transform.parent.gameObject;
        //カメラとダメージエフェクト
        main_Camera = GameObject.Find("Main Camera");
        bolt_Effect = Instantiate(Resources.Load("Effect/BoltEffect")) as GameObject;
        bolt_Effect.transform.SetParent(main_Camera.transform);
        bolt_Effect.transform.localPosition = new Vector3(0, 0, 10);
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
        if (!damaged_Trigger) {
            damaged_Trigger = true; 
            //ライフを減らす
            _playerManager.life -= damage;
            //powerを減らす
            Reduce_Power();
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
                GameObject bomb = Instantiate(Resources.Load("Effect/PlayerDamagedBomb")) as GameObject;
                bomb.transform.position = transform.position;
                bolt_Effect.GetComponent<ParticleSystem>().Play();
                //点滅、無敵化
                StartCoroutine("Blink");
            }
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
        damaged_Trigger = false;
    }


    //ライフが0になった時の処理
    private void Miss() {
        Debug.Log("Miss");
        //エフェクト
        GameObject effect = Instantiate(Resources.Load("Effect/PlayerMissEffect")) as GameObject;
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
        Debug.Log("GameOver");
        SceneManager.LoadScene("GameOverScene");
    }


}
