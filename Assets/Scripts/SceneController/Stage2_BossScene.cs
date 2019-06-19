using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossScene : MonoBehaviour {

    //スクリプト
    private Stage2_BossMovie _movie;
    private GameManager game_Manager;

    //霊夢
    private BossEnemyController boss_Controller;


	// Use this for initialization
	void Start () {
        //スクリプトの取得
        _movie = GetComponent<Stage2_BossMovie>();
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        boss_Controller = GameObject.Find("Reimu").GetComponent<BossEnemyController>();
        //ムービー開始
        _movie.StartCoroutine("Previous_Movie");

        //セーブ
        //game_Manager.SaveData();
	}

    private void Update() {
        //クリア後
        if (boss_Controller.Clear_Trigger()) {
            _movie.StartCoroutine("Clear_Movie");
        }
    }
}
