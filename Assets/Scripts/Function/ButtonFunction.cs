using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;
    private PlayerManager _playerManager;


	// Use this for initialization
	void Start () {
        //スクリプト
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
    }


    //初めからボタン押下時
    public void Start_Game_Button() {
        _gameManager.DeleteData();
        _gameManager.StartCoroutine("LoadData");
    }


    //続きからボタン押下時
    public void Load_Data_Button() {
        _gameManager.StartCoroutine("LoadData");
    }

    //コンティニューボタン押下時
    public void Continue_Button() {
        _playerManager.StartCoroutine("Continue");
    }

    //タイトルに戻るボタン押下時
    public void Title_Button() {
        SceneManager.LoadScene("TitleScene");
    }

    //ゲーム再開ボタン押下時
    public void Resume_Button() {
        GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Release_Pause_Game();
    }

    //ポーズ画面からタイトルに戻るボタン押下時
    public void Title_From_Pause_Button() {
        GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Release_Pause_Game();
        SceneManager.LoadScene("TitleScene");
    }


    /*---------------------キーコンフィグ---------------------*/
    //入力待ち
    private bool wait_Input = false;

    private IEnumerator Wait_Input(string name, GameObject button) {
        wait_Input = true;
        KeyConfig keyConfig = new KeyConfig();
        //色の変更
        button.GetComponent<Image>().color = new Color(1, 0.4f, 0.4f);
        //テキストの変更
        button.GetComponentInChildren<Text>().text = "";
        //入力待ち
        yield return null;
        while (true) {
            if (Input.GetButtonDown("joystick button 3")) {
                keyConfig.Change_Button(name, "joystick button 3", true);
                keyConfig.Create_InputManager();
                //テキスト変更
                button.GetComponentInChildren<Text>().text = "button 3";
                break;
            }
            yield return null;
        }
        //色を戻す
        button.GetComponent<Image>().color = new Color(1, 1, 1);
        wait_Input = false;
    }


    //ジャンプ、決定ボタンの変更
    public void Jump_Submit_Button() {
        if (!wait_Input) {
            GameObject button = GameObject.Find("Jump/Submit");
            StartCoroutine(Wait_Input("Jump/Submit", button));
        }
    }
 

}
