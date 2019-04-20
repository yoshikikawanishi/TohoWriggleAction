﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointController : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;


	// Use this for initialization
	void Start () {
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機と接触したときセーブ
        if (collision.tag == "PlayerBodyTag") {
            _gameManager.SaveData();
        }
    }
}