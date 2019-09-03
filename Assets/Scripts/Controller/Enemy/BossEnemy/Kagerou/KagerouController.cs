using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouController : MonoBehaviour {

    //コンポーネント
    private KagerouAttack _attack;
    private BossEnemyController boss_Controller;
    private Animator _anim;

    //戦闘開始
    private bool start_Battle = false;


	// Use this for initialization
	void Start () {
        //取得
        _attack = GetComponent<KagerouAttack>();
        boss_Controller = GetComponent<BossEnemyController>();
        _anim = GetComponent<Animator>();
	}


    // Update is called once per frame
    void Update() {
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
                case 3: _attack.Phase3(); break;
                case 4: _attack.Phase4(); break;
            }
        }
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;
    }


    //咆哮
    public void Roar() {
        GameObject roar_Effect = transform.Find("RoarEffect").gameObject;
        roar_Effect.GetComponent<ParticleSystem>().Play();
        roar_Effect.GetComponent<AudioSource>().Play();
    }

    //咆哮効果音
    public void Roar_Sound() {
        GameObject roar_Effect = transform.Find("RoarEffect").gameObject;
        roar_Effect.GetComponent<AudioSource>().Play();
    }


    //アニメーション変更
    public void Change_Parametar(string change_Bool, int scale_X) {

    }
}
