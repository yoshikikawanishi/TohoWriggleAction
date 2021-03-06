﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyController : MonoBehaviour {

    //スクリプト
    private BossEnemyController boss_Controller;
    private DoremyAttack _attack;
    private MoveBetweenTwoPoints _move;
    private Extra_BossMovie stage_Movie;
    //コンポーネント
    private Animator _anim;
    //オブジェクト
    [SerializeField] private GameObject warp_In_Effect;
    [SerializeField] private GameObject warp_Out_Effect;
    [SerializeField] private GameObject back_Design;

    //戦闘開始
    public bool start_Battle = false;


    //Awake
    private void Awake() {
        //取得
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = GetComponent<DoremyAttack>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        stage_Movie = GameObject.FindWithTag("ScriptsTag").GetComponent<Extra_BossMovie>();
        _anim = GetComponent<Animator>();

    }


    // Use this for initialization
    void Start () {
        //テスト用
        Debug.Log("Boss Battle Test");
        boss_Controller.Set_Now_Phase(1);
	}
	

	// Update is called once per frame
	void Update () {
        //攻撃
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
                case 3: _attack.Phase3(); break;
                case 4: _attack.Phase4(); break;
                case 5: _attack.Phase5(); break;
                case 6: _attack.Phase6(); break;
            }
        }
        //クリア時
        if (boss_Controller.Clear_Trigger()) {
            _attack.Stop_Phase6();
            stage_Movie.Start_Clear_Movie();
        }
	}


    //アニメーション変更
    public void Change_Parameter(string next_Bool, int direction) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("TransformBool", false);

        _anim.SetBool(next_Bool, true);
        transform.localScale = new Vector3(direction, 1, 1);
    }


    //移動
    public void Move(Vector2 next_Pos, float speed) {
        _move.Start_Move(next_Pos, 0, speed);
    }
    public bool End_Move() {
        return _move.End_Move();
    }

    public void Move_Randome() {
        _move.Start_Random_Move(32f, 0.01f);
    }


    //瞬間移動
    public void Start_Warp(Vector2 next_Pos, int body_Direction) {
        StartCoroutine(Warp(next_Pos, body_Direction));
    }
    private IEnumerator Warp(Vector2 next_Pos, int body_Direction) {
        //無敵化
        Change_Layer("InvincibleLayer");
        //エフェクト
        warp_In_Effect.GetComponent<ParticleSystem>().Play();
        warp_In_Effect.GetComponent<AudioSource>().Play();
        //移動
        Change_Parameter("IdleBool", body_Direction);
        _anim.SetTrigger("WarpTrigger");
        yield return new WaitForSeconds(0.5f);
        transform.position = next_Pos;
        //エフェクト
        warp_Out_Effect.GetComponent<ParticleSystem>().Play();
        warp_Out_Effect.GetComponent<AudioSource>().Play();
        //戻す
        Change_Layer("EnemyLayer");
    }

    //ランダム移動時のワープ
    public void Do_Randome_Warp() {
        Vector2[] pos = { new Vector2(-160f, -50f), new Vector2(-160f, 50f), new Vector2(-40f, 0) };
        int num = Random.Range(0, 6);
        if(num >= 3) {
            Start_Warp(-pos[num % 3], 1);
        }
        else {
            Start_Warp(pos[num], -1);
        }
    }


    //フェーズ変更時の移動
    public void Warp_In_Phase_Change(Vector2 next_Pos, int direction) {
        StartCoroutine(Warp_Invincible(next_Pos, direction));
    }
    public IEnumerator Warp_Invincible(Vector2 next_Pos, int direction) {
        Change_Layer("InvincibleLayer");
        yield return new WaitForSeconds(2.5f);
        Start_Warp(next_Pos, direction);
        yield return new WaitForSeconds(1.5f);
        Change_Layer("EnemyLayer");
    }


    //レイヤー変更
    public void Change_Layer(string layer_Name) {
        gameObject.layer = LayerMask.NameToLayer(layer_Name);
    }


    //ためエフェクト
    public GameObject Play_Charge_Effect(float lifeTime) {
        GameObject effect = Instantiate(Resources.Load("Effect/PowerChargeEffectsRed") as GameObject);
        effect.transform.position = transform.position;
        effect.transform.SetParent(transform);
        if (lifeTime > 0) {
            Destroy(effect, lifeTime);
        }
        return effect;
    }

    //放出エフェクト
    public void Play_Spread_Effect() {
        GameObject effect = Instantiate(Resources.Load("Effect/PowerSpreadEffectRed") as GameObject);
        effect.transform.position = transform.position;
        Destroy(effect, 2.0f);
    }


    //バックデザイン出す
    public void Appear_Back_Design(Vector3 pos, Color color) {
        UsualSoundManager.Charge_Sound();
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        back_Design.transform.position = pos;
        for (int i = 0; i < back_Design.transform.childCount; i++) {
            back_Design.transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
        back_Design.SetActive(true);
    }

    //バックデザイン消す
    public void Delete_Back_Design() {
        back_Design.SetActive(false);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
    }
}
