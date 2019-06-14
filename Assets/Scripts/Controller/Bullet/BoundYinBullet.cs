using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundYinBullet : MonoBehaviour {

    //コンポネント
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigid;
    [SerializeField] private bool is_Big = false;
    //無敵化
    private bool is_Invincible = false;

    //耐久
    private int life = 8;



    // Use this for initialization
    void Start() {
        //コンポーネント
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        //動き
        if (is_Big) {
            StartCoroutine("Big_Bullet_Move");
        }
        _rigid.angularVelocity = 200f;
    }
	

	// Update is called once per frame
	void Update () {
		if(life == 0) {
            life = -1;
            if (is_Big) {
                //分裂
                GameObject[] small_Bullet = new GameObject[2]; 
                for(int i = 0; i < 2; i++) {
                    small_Bullet[i] = Instantiate(Resources.Load("Bullet/SmallYinBallBullet")) as GameObject;
                    small_Bullet[i].transform.position = transform.position;
                    small_Bullet[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-100f + (i * 200f), 250f);
                }
                Destroy(gameObject);
            }
            else {
                //消滅
                Destroy(gameObject);
            }
        }
        if(transform.position.x > 260f) {
            Destroy(gameObject);
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾
        if (life > 0 && !is_Invincible) {
            if (collision.tag == "PlayerBulletTag") {
                life--;
                StartCoroutine("Blink");
            }
            if (collision.tag == "PlayerAttackTag" || collision.tag == "BombTag") {
                life = 0;
                StartCoroutine("Blink");
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (is_Invincible && collision.gameObject.tag == "PlayerTag") {
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
            _rigid.velocity = new Vector2(-150f, 100f);
            Debug.Log("AAA");
        }
    }


    //うごき
    private IEnumerator Big_Bullet_Move() {
        is_Invincible = true;
        _rigid.gravityScale = 0;
        _sprite.color = new Color(1, 0.4f, 0.4f);
        while(transform.position.x > -260f) {
            yield return null;
        }
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        GetComponent<Rigidbody2D>().velocity = new Vector2(90f, 300f);
        _sprite.color = new Color(1, 1, 1);
        _rigid.gravityScale = 32f;
        is_Invincible = false;
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
    }
}
