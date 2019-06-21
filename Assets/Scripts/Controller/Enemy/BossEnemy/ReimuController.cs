using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//霊夢
public class ReimuController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    //スクリプト
    private BossEnemyController boss_Controller;
    private ReimuAttack _attack;
    
    //戦闘開始
    public bool start_Battle = false;


    // Use this for initialization
    void Awake () {
        //コンポーネント
        _anim = GetComponent<Animator>();
        //スクリプト
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = gameObject.AddComponent<ReimuAttack>();
    }
	

	// Update is called once per frame
	void Update () {
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
            }
        }
        
	}


    //アニメーション
    public void Change_Parameter(string change_Bool) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("AvoidBool", false);
        _anim.SetBool("AttackBool", false);

        _anim.SetBool(change_Bool, true);
    }
}
