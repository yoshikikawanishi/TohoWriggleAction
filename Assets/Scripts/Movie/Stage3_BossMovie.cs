﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3_BossMovie : MonoBehaviour {

    //スクリプト
    private PauseManager _pause;
    private MessageDisplay _message;
    private GameManager _gameManager;

    //ムービーの進行度
    private int movie_Progress = 1;

    //初回かどうか
    private bool is_First_Visit = false;


	// Use this for initialization
	void Awake () {
        //スクリプトの取得
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        _message = GetComponent<MessageDisplay>();
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
    }
	

    //ボス前ムービー
    public IEnumerator Before_Boss_Movie() {
        //初期設定
        movie_Progress = 1;
        _pause.Set_Pausable(false);
        is_First_Visit = _gameManager.Is_First_Visit();
        StartCoroutine(Player_Timeline());
        StartCoroutine(Mystia_Timeline());

        yield return new WaitForSeconds(1.5f);

        //ミスティア登場会話
        if (is_First_Visit) {
            _message.Start_Display("MystiaText", 1, 4);
            yield return new WaitUntil(_message.End_Message);
        }
        movie_Progress = 2;

        //終了設定
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Change_BGM_Index(6);
        _pause.Set_Pausable(true);
    }


    //自機
    private IEnumerator Player_Timeline() {
        //初期設定
        GameObject player = GameObject.FindWithTag("PlayerTag");
        WriggleController player_Controller = player.GetComponent<WriggleController>();
        player_Controller.Set_Playable(false);

        //移動
        player_Controller.Change_Parameter("DashBool");
        for(float t = 0; t < 1.5f; t += Time.deltaTime) {
            player.transform.position += new Vector3(1.5f, 0, 0);
            yield return null;
        }
        player_Controller.Change_Parameter("IdleBool");

        //ミスティア登場会話
        while(movie_Progress < 2) { yield return null; }

        //終了設定
        player_Controller.Set_Playable(true);
    }


    //ミスティア
    private IEnumerator Mystia_Timeline() {
        //初期設定
        GameObject mystia = GameObject.Find("Mystia");
        MystiaController mystia_Controller = mystia.GetComponent<MystiaController>();
        MoveBetweenTwoPoints mystia_Move = mystia.GetComponent<MoveBetweenTwoPoints>();

        //移動
        mystia_Move.Start_Move(new Vector3(150f, 0, 0), -16f, 0.016f);
        mystia_Controller.Change_Parameter("DashBool");
        yield return new WaitUntil(mystia_Move.End_Move);
        mystia_Controller.Change_Parameter("IdleBool");

        //ミスティア登場会話
        while (movie_Progress < 2) { yield return null; }

        //終了設定
        mystia_Controller.start_Battle = true;
    }
	

    //クリア後ムービー
    public IEnumerator Clear_Movie() {
        yield return new WaitForSeconds(5.0f);
        _message.Start_Display("MystiaText", 5, 9);
        yield return new WaitUntil(_message.End_Message);
        yield return new WaitForSeconds(1.5f);
        GetComponent<BorderFadeOut>().Start_Fade_Out(0.015f);
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Start_Fade_Out(0.01f, 1.6f);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Base_2Scene");
    }

}
