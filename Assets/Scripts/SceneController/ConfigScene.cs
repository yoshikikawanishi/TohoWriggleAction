using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigScene : MonoBehaviour {

    //ボタン
    Button jump_Button;
    Button shot_Button;
    Button fly_Button;
    Button pause_Button;


	// Use this for initialization
	void Start () {
        //ボタンの取得
        jump_Button = GameObject.Find("Jump/Submit").GetComponent<Button>();
        shot_Button = GameObject.Find("Shot/Cancel").GetComponent<Button>();
        fly_Button = GameObject.Find("Fly").GetComponent<Button>();
        pause_Button = GameObject.Find("Pause").GetComponent<Button>();
        //ボタンのテキスト書き換え
        Button_Text_Change();
        jump_Button.Select();  
	}


    //ボタンのテキスト書き換え
    public void Button_Text_Change() {
        string filePath = Application.dataPath + @"\KeyConfig.csv";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);
        jump_Button.GetComponentInChildren<Text>().text = text.textWords[1, 1] + " / " + text.textWords[1, 2];
        shot_Button.GetComponentInChildren<Text>().text = text.textWords[2, 1] + " / " + text.textWords[2, 2];
        fly_Button.GetComponentInChildren<Text>().text = text.textWords[3, 1] + " / " + text.textWords[3, 2];
        pause_Button.GetComponentInChildren<Text>().text = text.textWords[4, 1] + " / " + text.textWords[4, 2];

    }
}
