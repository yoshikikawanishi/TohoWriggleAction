using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yukkuri : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;

    //絵
    [SerializeField] private Sprite normal_Sprite;
    [SerializeField] private Sprite damage_Sprite;


	// Use this for initialization
	void Start () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerAttackTag" || collision.tag == "PlayerBulletTag") {
            StartCoroutine("Damaged");
        }
    }


    //被弾
    private IEnumerator Damaged() {
        _sprite.sprite = damage_Sprite;
        yield return new WaitForSeconds(0.5f);
        _sprite.sprite = normal_Sprite;
    }
}
