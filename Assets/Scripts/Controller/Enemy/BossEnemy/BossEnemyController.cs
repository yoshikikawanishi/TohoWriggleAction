﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

//ボスの体力、被弾時の処理、クリア時の処理をするクラス
public class BossEnemyController : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;
    private AudioSource damage_Audio_Source;
    [SerializeField] AudioClip damage_Sound;
    [SerializeField] AudioMixerGroup audio_Group;
    //スクリプト
    private ObjectPool _pool;

    //体力
    public List<int> life = new List<int>();
    public List<int> LIFE;
    //フェーズの数
    private int phase_Num = 1;
    //現在のフェーズ
    private int now_Phase = 1;
    //落とすアイテム
    [SerializeField] private int power_Value;
    [SerializeField] private int score_Value;

    //クリア検知用
    private bool clear_Trigger = false;
    private bool is_Cleared = false;

    //フェーズ切り替え時のボム
    [SerializeField] private GameObject phase_Change_Bomb;
    //ボス撃破時のエフェクト
    [SerializeField] private GameObject clear_Effect;
    //ダメージエフェクト
    private GameObject damage_Effect;
    private ParticleSystem hit_Effect_Particle;



	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        //ダメージサウンドの設定
        damage_Audio_Source = gameObject.AddComponent<AudioSource>();
        damage_Audio_Source.clip = damage_Sound;
        damage_Audio_Source.volume = 0.001f;
        damage_Audio_Source.outputAudioMixerGroup = audio_Group;
        //パーティクル
        hit_Effect_Particle = transform.Find("BossHitEffect").GetComponent<ParticleSystem>();
        //ダメージエフェクトのオブジェクトプール
        _pool = gameObject.AddComponent<ObjectPool>();
        damage_Effect = Resources.Load("Effect/BossDamagedEffect") as GameObject;
        _pool.CreatePool(damage_Effect, 10);

        //初期値代入
        phase_Num = life.Count();
        life.Add(63);
        LIFE = new List<int>(life);
    }


    //被弾の検知
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾に当たった時
        if (collision.tag == "PlayerBulletTag") {
            Damaged(1);
            StartCoroutine("Damaged_Effect", collision.transform.position);
        }
        else if(collision.tag == "BeetleBulletTag") {
            Damaged(5);
        }
        //キックに当たった時
        else if (collision.tag == "PlayerAttackTag") {
            Damaged(10);
        }
    }


    //被弾時の処理
    public void Damaged(int damage) {
        life[now_Phase-1] -= damage;
        //エフェクト
        damage_Audio_Source.Play();
        hit_Effect_Particle.Play();
        StartCoroutine("Blink");
        //フェーズ切り替え
        if(life[now_Phase-1] <= 0) {
            Phase_Change(now_Phase + 1);
        }
        //体力0でクリア
        if(now_Phase > phase_Num && !is_Cleared) {
            is_Cleared = true;
            Clear();           
        }
        //被弾音の切り替え
        if(life[now_Phase-1] <= LIFE[now_Phase-1] / 5f) {
            damage_Audio_Source.volume = 0.05f;
        }
        else {
            damage_Audio_Source.volume = 0.01f;
        }
    }


    //ダメージエフェクトの生成
    private IEnumerator Damaged_Effect(Vector3 pos) {
        var effect = _pool.GetObject();
        effect.transform.position = pos;
        yield return new WaitForSeconds(0.25f);
        effect.SetActive(false);
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.02f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.02f);
    }


    //フェーズ切り替え時の処理
    public void Phase_Change(int next_Phase) {
        //アイテム出す
        Put_Out_Item(0, power_Value);
        //弾消し用のボム生成
        var bomb = Instantiate(phase_Change_Bomb) as GameObject;
        bomb.transform.position = transform.position;
        now_Phase = next_Phase;
    }


    //クリア時の処理
    private void Clear() {
        clear_Trigger = true;
        now_Phase = life.Count();
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        //エフェクト
        var effect = Instantiate(clear_Effect) as GameObject;
        effect.transform.position = transform.position;
        var bomb = Instantiate(phase_Change_Bomb) as GameObject;
        bomb.transform.position = transform.position;
        //画面を揺らす
        CameraShake _shake = gameObject.AddComponent<CameraShake>();
        _shake.Shake(0.25f, 4f, true);
        //アイテムを出す
        Put_Out_Item(score_Value, power_Value / 4);
    }


    //アイテムを出す
    private void Put_Out_Item(int score_Value, int power_Value) {
        //点
        int score_Num = score_Value / 100;
        for (int i = 0; i < score_Num; i++) {
            GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
            score.transform.position = transform.position;
            score.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-score_Num, score_Num) * 20, 500f + Random.Range(-100f, 100f));
        }
        //P
        int power_Num = power_Value;
        GameObject p = Resources.Load("Power") as GameObject;
        for (int i = 0; i < power_Num; i++) {
            GameObject power = Instantiate(p);
            power.transform.position = transform.position;
            power.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-power_Num, power_Num) * 5, 500f + Random.Range(-100f, 100f));
        }
    }


    //他からのクリア検知用
    public bool Clear_Trigger() {
        if (clear_Trigger) {
            clear_Trigger = false;
            return true;
        }
        return false;
    }


    //now_PhaseのGetter
    public int Get_Now_Phase() {
        return now_Phase;
    }
    //Setter
    public void Set_Now_Phase(int num) {
        now_Phase = num;
    }

}
