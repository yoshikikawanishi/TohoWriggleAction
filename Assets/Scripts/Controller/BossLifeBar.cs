using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLifeBar : MonoBehaviour {

    //ボス
    [SerializeField] private GameObject Boss_Enemy;
    private BossEnemyController boss_Controller;

    //ライフバー
    private Slider life_Bar;

    private int now_Phase = 1;

	// Use this for initialization
	void Start () {
        //取得
        boss_Controller = Boss_Enemy.GetComponent<BossEnemyController>();
        life_Bar = transform.GetComponentInChildren<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        Show_Life_Bar();
	}


    //ライフバーの表示
    private void Show_Life_Bar() {
        //フェーズの取得
        if(now_Phase != boss_Controller.Get_Now_Phase()) {
            now_Phase = boss_Controller.Get_Now_Phase();
        }
        //最大値の変更
        if(life_Bar.maxValue != boss_Controller.LIFE[now_Phase - 1]) {
            life_Bar.maxValue = boss_Controller.LIFE[now_Phase - 1];
        }
        //値の変更
        if(life_Bar.value != boss_Controller.life[now_Phase - 1]) {
            life_Bar.value = boss_Controller.life[now_Phase - 1];
        }
    }
}
