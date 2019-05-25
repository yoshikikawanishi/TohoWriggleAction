using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //体力
    [SerializeField] public int life = 1;
    //消滅時のエフェクト
    protected GameObject vanish_Effect;
    //点
    [SerializeField] private int score_Value = 200;
    //P
    [SerializeField] private int power_Value = 2;
    //回復アイテムの確立
    [SerializeField] private float probability_Recover_Item = 20f;
    //点、pを出す速度
    [SerializeField] private float item_Out_Speed = 500f;

    //コンポーネント
    protected SpriteRenderer _sprite;
    //オーディオ
    private AudioSource damage_Sound;

    //デフォルトカラー
    private Color default_Color;

    //消滅の処理に入ったかどうか
    protected bool is_Vanished = false;


	// Use this for initialization
	public void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        damage_Sound = GetComponents<AudioSource>()[0];
        //オブジェクト
        vanish_Effect = Resources.Load("Effect/EnemyVanishEffect") as GameObject;

        //初期値代入
        default_Color = _sprite.color;
	}
	

    //TriggerEnter2D
    public void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾または自機に当たった時
        if(collision.tag == "PlayerBulletTag" || collision.tag == "PlayerBodyTag") {
            Damaged(1);
        }
        //キック、ボムに当たった時
        else if(collision.tag == "PlayerAttackTag" || collision.tag == "BombTag") {
            Damaged(25);
        }
    }
    //CollisionEnter2D
    public void OnCollisionEnter2D(Collision2D collision) {
        //自機に当たった時
        if (collision.gameObject.tag == "PlayerTag") {
            Damaged(25);
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
    protected virtual void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPと回復アイテムの生成
        Put_Out_Item();
        Destroy(gameObject);
    }

    //点とPと回復アイテムの生成
    protected void Put_Out_Item() {
        //点
        int score_Num = score_Value / 100;
        for (int i = 0; i < score_Num; i++) {
            GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
            score.transform.position = transform.position;
            score.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-score_Num, score_Num) * 50, item_Out_Speed + Random.Range(-100f, 100f));
        }
        //P
        int power_Num = power_Value;
        for (int i = 0; i < power_Num; i++) {
            GameObject power = Instantiate(Resources.Load("Power")) as GameObject;
            power.transform.position = transform.position;
            power.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-power_Num, power_Num) * 50, item_Out_Speed + Random.Range(-100f, 100f));
        }
        //回復アイテム
        if(Random.Range(0, 100) < probability_Recover_Item) {
            GameObject life_Up_Item = Instantiate(Resources.Load("LifeUpItem")) as GameObject;
            life_Up_Item.transform.position = transform.position;
        }
    }


}
