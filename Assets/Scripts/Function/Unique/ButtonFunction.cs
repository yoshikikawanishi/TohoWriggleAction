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
        TitleScene title_Scene_Controller = GameObject.Find("Scripts").GetComponent<TitleScene>();
        if (title_Scene_Controller != null) {
            title_Scene_Controller.Display_Confirm_Canvas();
        }
    }

    //確認ボタン
    public void Confirm_Start_Button(bool is_Start_Game) {
        if (is_Start_Game) {
            _gameManager.DeleteData();
            StartCoroutine(Start_Game());
        }
        else {
            TitleScene title_Scene_Controller = GameObject.Find("Scripts").GetComponent<TitleScene>();
            if(title_Scene_Controller != null) {
                title_Scene_Controller.Delete_Confirm_Canvas();
            }
        }
    }

    //初めから開始
    private IEnumerator Start_Game() {
        FadeInOut f = GameObject.FindWithTag("ScriptsTag").GetComponent<FadeInOut>();
        f.Start_Fade_Out();
        yield return new WaitForSeconds(1.0f);
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
        //UnityEditor.EditorApplication.isPlaying = false;
        UnityEngine.Application.Quit();
    }

    //ゲーム再開ボタン押下時
    public void Resume_Button() {
        GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Release_Pause_Game();
    }

    //ポーズ画面からタイトルに戻るボタン押下時
    public void Title_From_Pause_Button() {
        GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Confirm_Back_Title();
    }

    //確認画面のボタン
    public void Confirm_Button(bool is_Back_Title) {
        PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        if (is_Back_Title) {
            _pause.Release_Pause_Game();
            SceneManager.LoadScene("TitleScene");
        }
        else {
            _pause.Delete_Confirm_Canvas();
        }
    }

    //エクストラボタン押下時
    public void Go_Extra_Scene() {
        SceneManager.LoadScene("ExtraScene");
    }

    //キーコンフィグボタン押下時
    public void Go_Key_Config() {
        SceneManager.LoadScene("ConfigScene");
    }

    //遊び方ボタン押下時
    public void Go_Guide_Scene() {
        SceneManager.LoadScene("PlayGuideScene");
    }

}
