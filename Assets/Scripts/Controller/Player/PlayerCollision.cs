using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;
    
    //自機
    protected GameObject player;
    //ダメージエフェクト
    [SerializeField] private GameObject damaged_Bomb;
    //死亡エフェクト
    [SerializeField] private GameObject miss_Effect;

    //被弾時の無敵時間
    private float INVINCIBLE_TIME = 2.0f;

    //被弾の判定
    protected bool damaged_Trigger = false;
    //ミスの判定
    protected bool miss_Trigger = false;


	// Use this for initialization
	protected void Start () {
        //コンポーネントの取得
        _renderer = GetComponentInParent<Renderer>();
        //自機
        player = transform.parent.gameObject;
    }


    //OnTriggerEnter
    protected void OnTriggerEnter2D(Collider2D collision) {
        //被弾時
        if (collision.tag == "EnemyTag" || collision.tag == "EnemyBulletTag") {
            Get_Hit();
        }
        //即死
        else if(collision.tag == "MissZoneTag") {
            Miss();
        }
        
    }


    //OnCollisionEnter
    protected void OnCollisionEnter2D(Collision2D collision) {
        //被弾時
        if (collision.gameObject.tag == "EnemyTag" || collision.gameObject.tag == "EnemyBulletTag") {
            Get_Hit();
        }
    }


    //被弾
    protected void Get_Hit() {
        if (!damaged_Trigger) {
            damaged_Trigger = true;
            Damaged(1);
        }
    }


    //被弾時の処理
    protected void Damaged(int damage) {
        //反動
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(-player.transform.localScale.x, 2) * 100f;
        //ボムエフェクト
        GameObject bomb = Instantiate(damaged_Bomb) as GameObject;
        bomb.transform.position = transform.position;
        //点滅、無敵化
        StartCoroutine("Blink");
    }


    //ライフが0になった時の処理
    protected void Miss() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        //エフェクト
        GameObject effect = Instantiate(miss_Effect) as GameObject;
        effect.transform.position = transform.position;
        player.SetActive(false);
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
}
