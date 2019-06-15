using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossScene : MonoBehaviour {

    //スクリプト
    private Stage2_BossMovie _movie;
    private GameManager game_Manager;

	// Use this for initialization
	void Start () {
        //スクリプトの取得
        _movie = GetComponent<Stage2_BossMovie>();
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //ムービー開始
        _movie.StartCoroutine("Previous_Movie");

        //セーブ
        game_Manager.SaveData();
	}
}
