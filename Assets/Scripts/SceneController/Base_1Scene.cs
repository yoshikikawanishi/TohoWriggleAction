﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base_1Scene : MonoBehaviour {

    //自機
    private GameObject player;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            GetComponent<FadeInOut>().Start_Fade_In();
            Debug.Log("AA");
        }
	}
	
	// Update is called once per frame
	void Update () {
		//右端に行ったらシーン遷移
        if(player.transform.position.x > 740f) {
            SceneManager.LoadScene("Stage3_1Scene");
        }
	}
}
