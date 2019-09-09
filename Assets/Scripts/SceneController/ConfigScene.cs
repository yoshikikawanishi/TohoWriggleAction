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

        KeyConfig k = InputManager.Instance.keyConfig;

        jump_Button.GetComponentInChildren<Text>().text = k.GetKeyCode("Jump")[0].ToString() + "\t|\t" + k.GetKeyCode("Jump")[1].ToString();
        shot_Button.GetComponentInChildren<Text>().text = k.GetKeyCode("Shot")[0].ToString() + "\t|\t" + k.GetKeyCode("Shot")[1].ToString();
        fly_Button.GetComponentInChildren<Text>().text = k.GetKeyCode("Fly")[0].ToString() + "\t|\t" + k.GetKeyCode("Fly")[1].ToString();
        pause_Button.GetComponentInChildren<Text>().text = k.GetKeyCode("Pause")[0].ToString() + "\t|\t" + k.GetKeyCode("Pause")[1].ToString();

    }
}
