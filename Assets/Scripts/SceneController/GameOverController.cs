using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
        //ボタンの初期位置
        Button continue_Button = GameObject.Find("ContinueButton").GetComponent<Button>();
        continue_Button.Select();

        //コンティニュー回数を増やす
        PlayerManager _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        _playerManager.continue_Count++;
        PlayerPrefs.SetInt("Continue", _playerManager.continue_Count);


    }
	
}
