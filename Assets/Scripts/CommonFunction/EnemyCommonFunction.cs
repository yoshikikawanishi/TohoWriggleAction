using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommonFunction : MonoBehaviour {

    //体力
    [SerializeField] private int life = 1;
    //消滅時のエフェクト
    [SerializeField] private GameObject vanish_Effect;
    //点
    [SerializeField] private int score_Value = 200;
    //P
    [SerializeField] private int power_Value = 2;

    //コンポーネント
    private SpriteRenderer _sprite;
    //オーディオ
    private AudioSource damage_Sound;

    //デフォルトカラー
    private Color default_Color;

    //消滅の処理に入ったかどうか
    private bool is_Vanished = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        damage_Sound = GetComponents<AudioSource>()[0];

        //初期値代入
        default_Color = _sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //TriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾または自機に当たった時
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerBodyTag") {
            Damaged(1);
        }
        //キックに当たった時
        else if(collision.tag == "PlayerAttackTag") {
            Damaged(10);
        }
    }
    //CollisionEnter2D
    private void OnCollisionEnter2D(Collision2D collision) {
        //自機に当たった時
        if (collision.gameObject.tag == "PlayerTag") {
            Damaged(life / 2);
        }
    }


    //被弾時の処理
    private void Damaged(int damage_Power) {
        life -= damage_Power;
        //消滅
        if (life <= 0 && !is_Vanished) {
            Vanish();
            is_Vanished = true;
        }
        else if(!is_Vanished){
            damage_Sound.Play();
            //点滅
            StartCoroutine("Blink");
        }
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = default_Color * new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = default_Color;
        yield return new WaitForSeconds(0.1f);
    }


    //消滅時の処理
    private void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPの生成
        Put_Out_Item();
        Destroy(gameObject);
    }

    //点とPの生成
    private void Put_Out_Item() {
        int score_Num = score_Value / 100;
        for (int i = 0; i < score_Num; i++) {
            GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
            score.transform.position = transform.position;
            score.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-score_Num, score_Num) * 50, Random.Range(300f, 500f));
        }
        int power_Num = power_Value;
        for (int i = 0; i < power_Num; i++) {
            GameObject power = Instantiate(Resources.Load("Power")) as GameObject;
            power.transform.position = transform.position;
            power.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-power_Num, power_Num) * 50, Random.Range(300f, 500f));
        }
    }


}
