using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStage : MonoBehaviour {

    //スクリプト
    private Extra_BossMovie _movie;
    private PlayerManager player_Manager;


	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Extra_BossMovie>();
        player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();

        //初期設定
        player_Manager.life = 9;
        player_Manager.power = 128;
        player_Manager.score = 3;
        //ムービー開始
        _movie.Start_Before_Movie();
	}
	
	
}
