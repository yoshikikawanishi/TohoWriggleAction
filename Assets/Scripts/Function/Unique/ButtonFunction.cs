using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MBLDefine;
using UnityEngine.EventSystems;

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
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            TitleScene title_Scene_Controller = GameObject.Find("Scripts").GetComponent<TitleScene>();
            if (title_Scene_Controller != null) {
                title_Scene_Controller.Display_Confirm_Canvas();
            }
        }
    }

    //確認ボタン
    public void Confirm_Start_Button(bool is_Start_Game) {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            if (is_Start_Game) {
                _gameManager.DeleteData();
                StartCoroutine(Start_Game());
            }
            else {
                TitleScene title_Scene_Controller = GameObject.Find("Scripts").GetComponent<TitleScene>();
                if (title_Scene_Controller != null) {
                    title_Scene_Controller.Delete_Confirm_Canvas();
                }
            }
        }
    }

    //ゲーム開始
    private IEnumerator Start_Game() {
        FadeInOut f = GameObject.FindWithTag("ScriptsTag").GetComponent<FadeInOut>();
        f.Start_Fade_Out();
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Start_Fade_Out(0.01f, 1.0f);
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1.0f);
        _gameManager.StartCoroutine("LoadData");
    }


    //続きからボタン押下時
    public void Load_Data_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            StartCoroutine("Start_Game");
    }


    //コンティニューボタン押下時
    public void Continue_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            _playerManager.StartCoroutine("Continue");
    }

    //タイトルに戻るボタン押下時
    public void Title_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            SceneManager.LoadScene("TitleScene");
    }

    //ゲームを辞めるボタン押下時
    public void Quit_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            PlayerPrefs.Save();
            //UnityEditor.EditorApplication.isPlaying = false;
            UnityEngine.Application.Quit();
        }
    }

    //ゲーム再開ボタン押下時
    public void Resume_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Release_Pause_Game();
    }

    //ポーズ画面からタイトルに戻るボタン押下時
    public void Title_From_Pause_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>().Confirm_Back_Title();
    }

    //確認画面のボタン
    public void Confirm_Button(bool is_Back_Title) {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
            if (is_Back_Title) {
                _pause.Release_Pause_Game();
                SceneManager.LoadScene("TitleScene");
            }
            else {
                _pause.Delete_Confirm_Canvas();
            }
        }
    }

    //エクストラボタン押下時
    public void Go_Extra_Scene() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) 
            StartCoroutine("Start_Extra_Stage");
        
    }

    private IEnumerator Start_Extra_Stage() {
        FadeInOut f = GameObject.FindWithTag("ScriptsTag").GetComponent<FadeInOut>();
        f.Start_Fade_Out();
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Start_Fade_Out(0.01f, 1.0f);
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("ExtraFrontScene");
    }

    //キーコンフィグボタン押下時
    public void Go_Key_Config() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            SceneManager.LoadScene("ConfigScene");
    }

    //遊び方ボタン押下時
    public void Go_Guide_Scene() {
        if (InputManager.Instance.GetKeyDown(Key.Jump))
            SceneManager.LoadScene("PlayGuideScene");
    }

}
