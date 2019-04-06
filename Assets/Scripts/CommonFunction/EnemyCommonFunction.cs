using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommonFunction : MonoBehaviour {

    //体力
    [SerializeField] private int life = 1;
    //消滅時のエフェクト
    [SerializeField] private GameObject vanish_Effect;

    //コンポーネント
    private SpriteRenderer _sprite;

    //デフォルトカラー
    private Color default_Color;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();

        //初期値代入
        default_Color = _sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //TriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾または自機に当たった時
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerTag") {
            Damaged(1);
        }
        //キックに当たった時
        else if(collision.tag == "PlayerAttackTag") {
            Damaged(10);
        }
    }


    //CollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision) {
        //自機の弾または自機に当たった時
        if (collision.gameObject.tag == "PlayerBulletTag" || collision.gameObject.tag == "PlayerTag") {
            Damaged(1);
        }
        //キックに当たった時
        else if(collision.gameObject.tag == "PlayerAttackTag") {
            Damaged(10);
        }
    }


    //被弾時の処理
    private void Damaged(int damage_Power) {
        life -= damage_Power;
        if(life <= 0) {
            GameObject effect = Instantiate(vanish_Effect);
            effect.transform.position = transform.position;
            Destroy(gameObject);
        }
        //点滅
        StartCoroutine("Blink");
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = default_Color * new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = default_Color;
        yield return new WaitForSeconds(0.1f);
    }



}
