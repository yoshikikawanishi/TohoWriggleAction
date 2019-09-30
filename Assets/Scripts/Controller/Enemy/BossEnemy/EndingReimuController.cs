using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingReimuController : MonoBehaviour {

    //スクリプト
    private BossEnemyController boss_Controller;
    private EndingReimuAttaack _attack;
    //コンポーネント
    private Animator _anim;

    public bool start_Battle_Trigger = false;


    //Awake
    private void Awake() {
        //取得
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = GetComponent<EndingReimuAttaack>();
        _anim = GetComponent<Animator>();
    }


	
	// Update is called once per frame
	void Update () {
        //攻撃
        if (start_Battle_Trigger) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Do_Reimu_Attack(); break;
            }
        }
	}


    //アニメーション変更
    public void Change_Animation(string change_Bool, int scale_X) {
        _anim.SetBool("AvoidBool", false);
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("AttackBool", false);
        _anim.SetBool("DashBool", false);

        _anim.SetBool(change_Bool, true);
        transform.localScale = new Vector3(scale_X, 1, 1);
    }
}
