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

    //ゲームを辞めるボタン押下時
    public void Quit_Button() {
        PlayerPrefs.Save();
        UnityEditor.EditorApplication.isPlaying = false;
        UnityEngine.Application.Quit();
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

    //キーコンフィグボタン押下時
    public void Go_Key_Config() {
        SceneManager.LoadScene("ConfigScene");
    }


    /*---------------------キーコンフィグ---------------------*/
    private bool wait_Input = false;

    private IEnumerator Wait_Input(string change_Key, GameObject button) {
        KeyConfig keyConfig = new KeyConfig();
        //色の変更
        button.GetComponent<Image>().color = new Color(1, 0.4f, 0.4f);
        //テキストの変更
        button.GetComponentInChildren<Text>().text = "";
        //入力待ち
        wait_Input = true;
        yield return null;
        while (true) {
            button.GetComponent<Button>().Select();
            if (Input.anyKeyDown) {
                //押されたキーコードの取得
                string put_Button = "";
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(code)) {
                        put_Button = code.ToString();
                        break;
                    }
                }
                put_Button = put_Button.ToLower();
                //ゲームパッド
                bool is_GamePad = false;
                for (int i = 0; i < 12; i++) {
                    if (Input.GetKeyDown("joystick button " + i.ToString())) {
                        keyConfig.Change_Button(change_Key, "joystick button " + i.ToString(), true);
                        button.GetComponentInChildren<Text>().text = "button " + i.ToString();
                        is_GamePad = true;
                        break;
                    }
                }
                //ゲームパッドじゃなかったとき
                if (!is_GamePad) {
                    if (put_Button == "leftshift") {
                        put_Button = "left shift";
                    }
                    keyConfig.Change_Button(change_Key, put_Button, false);
                    button.GetComponentInChildren<Text>().text = put_Button;
                }
                break;
            }
            yield return null;
        }
        wait_Input = false;
        //反映
        keyConfig.Create_InputManager();
        //ボタンのテキスト書き換え
        GetComponent<ConfigScene>().Button_Text_Change();
        //色を戻す
        button.GetComponent<Image>().color = new Color(1, 1, 1);
    }


    //ジャンプ、決定ボタンの変更
    public void Jump_Submit_Button() {
        if (!wait_Input) {
            GameObject button = GameObject.Find("Jump/Submit");
            StartCoroutine(Wait_Input("Jump/Submit", button));
        }
    }

    //ショット、戻るボタンの変更
    public void Shot_Cancel_Button() {
        if (!wait_Input) {
            GameObject button = GameObject.Find("Shot/Cancel");
            StartCoroutine(Wait_Input("Shot/Cancel", button));
        }
    }

    //飛行ボタンの変更
    public void Fly_Button() {
        if (!wait_Input) {
            GameObject button = GameObject.Find("Fly");
            StartCoroutine(Wait_Input("Fly", button));
        }
    }

    //ポーズボタンの変更
    public void Pause_Button() {
        if (!wait_Input) {
            GameObject button = GameObject.Find("Pause");
            StartCoroutine(Wait_Input("Pause", button));
        }
    }

}
