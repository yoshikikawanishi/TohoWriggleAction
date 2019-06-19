using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundYinBullet : Enemy {

    //コンポネント
    private Rigidbody2D _rigid;
    [SerializeField] private bool is_Big = false;
    //無敵化
    private bool is_Invincible = true;

    //耐久
    private int life = 2;



    // Use this for initialization
    new void Start() {
        base.Start();
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
        //初期値
        default_Color = new Color(1, 1, 1);
        //回転
        _rigid.angularVelocity = 500f;
    }


    //OnTriggerEnter
    private new void OnTriggerEnter2D(Collider2D collision) {
        if (!is_Invincible) {
            base.OnTriggerEnter2D(collision);
        }
        
    }

    //OnCollisionEnter
    private new void OnCollisionEnter2D(Collision2D collision) {
        if (is_Invincible && collision.gameObject.tag == "PlayerTag") {
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
            _rigid.velocity = new Vector2(-150f, 100f);
        }
        //地面に初めて当たった時
        if(is_Invincible && collision.gameObject.tag == "GroundTag") {
            is_Invincible = false;
            _sprite.color = new Color(1, 1, 1);
            _rigid.gravityScale = 24f;
            _rigid.velocity = new Vector2(_rigid.velocity.x / 2, 300f);
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        }
    }


    //消滅時
    override protected void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPと回復アイテムの生成
        Put_Out_Item();
        Destroy(gameObject);
        //分裂
        if (is_Big) {
            //分裂
            GameObject[] small_Bullet = new GameObject[2];
            for (int i = 0; i < 2; i++) {
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

}
