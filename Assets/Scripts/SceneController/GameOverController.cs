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
        //ゲームオーバー回数の表示
        Text count_Text = GameObject.Find("GameOverCount").GetComponent<Text>();
        PlayerManager pm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        count_Text.text = "x" + pm.continue_Count.ToString();
    }
	
}
