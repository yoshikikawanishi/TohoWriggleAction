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


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        
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
        else if(collision.tag == "PlayerFootTag") {
            Damaged(5);
        }
    }


    //CollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision) {
        //自機の弾または自機に当たった時
        if (collision.gameObject.tag == "PlayerBulletTag" || collision.gameObject.tag == "PlayerTag") {
            Damaged(1);
        }
        //キックに当たった時
        else if(collision.gameObject.tag == "PlayerFootTag") {
            Damaged(5);
        }
    }


    //被弾時の処理
    private void Damaged(int damage_Power) {
        life--;
        if(life <= 0) {
            GameObject effect = Instantiate(vanish_Effect);
            Destroy(gameObject);
        }
        //点滅
        StartCoroutine("Blink");
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.5f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.5f);
    }



}
