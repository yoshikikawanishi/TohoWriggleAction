using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaController : MonoBehaviour {

    //コンポーネント
    private BossEnemyController boss_Controller;
    private MarisaAttack _attack;
    private Animator _anim;

    //戦闘開始
    public bool start_Battle = false;

    //背景デザイン
    [SerializeField] private GameObject back_Design;
    

    //Awake
    private void Awake() {
        //取得
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = GetComponent<MarisaAttack>();
        _anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //ボス戦
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
                case 3: _attack.Phase3(); break;
                case 4: _attack.Phase4(); break;
            }
        }
        //クリア時
        if (boss_Controller.Clear_Trigger()) {
            _attack.StopAllCoroutines();
            GameObject.Find("Scripts").GetComponent<Stage4_BossMovie>().Start_Clear_Movie();
        }
	}


    //アニメーション変更
    public void Change_Parameter(string change_Bool, int scale_X) {
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("DashBool1", false);
        _anim.SetBool("DashBool2", false);
        _anim.SetBool("AttackBool", false);

        _anim.SetBool(change_Bool, true);
        transform.localScale = new Vector3(scale_X, 1, 1);
    }


    //背景デザイン出す
    public void Appear_Back_Design(Vector3 pos) {
        back_Design.transform.position = pos;
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        back_Design.SetActive(true);
    }

    //背景デザイン消す
    public void Disappear_Back_Design() {
        back_Design.SetActive(false);
    }

    
}
