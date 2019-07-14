using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BossScene : MonoBehaviour {

    //スクリプト
    private Stage3_BossMovie _movie;

    //ミスティア
    private MystiaController mystia_Controller;
    private BossEnemyController boss_Controller;


	// Use this for initialization
	void Start () {
        //スクリプト
        _movie = GetComponent<Stage3_BossMovie>();
        //ミスティア
        GameObject mystia = GameObject.Find("Mystia");
        mystia_Controller = mystia.GetComponent<MystiaController>();
        boss_Controller = mystia.GetComponent<BossEnemyController>();
        //ムービー開始
        _movie.StartCoroutine("Before_Boss_Movie");
	}


    //Updata
    private void Update() {
        //クリア後
        if (boss_Controller.Clear_Trigger()) {
            mystia_Controller.Clear();        
            _movie.StartCoroutine("Clear_Movie");
        }
    }


}
