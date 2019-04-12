using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;

	// Use this for initialization
	void Start () {
        //スクリプト
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //コンティニューボタン押下時
    public void Continue_Button() {
        _gameManager.StartCoroutine("LoadData");
    }

}
