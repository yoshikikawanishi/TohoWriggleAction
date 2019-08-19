using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaController : MonoBehaviour {

    //コンポーネント
    private BossEnemyController boss_Controller;
    private MarisaAttack _attack;

    //戦闘開始
    public bool start_Battle = false;
    


    //Awake
    private void Awake() {
        //取得
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = GetComponent<MarisaAttack>();
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
                case 2: break;
                case 3: break;
            }
        }
        //クリア時
        if (boss_Controller.Clear_Trigger()) {

        }
	}

    
}
