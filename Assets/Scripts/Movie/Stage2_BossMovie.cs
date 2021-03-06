﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2_BossMovie : MonoBehaviour {

    //スクリプト
    private MessageDisplay _message;
    private PauseManager _pause;
    private GameManager game_Manager;

    //ムービーの進行度
    private int movie_Progress = 0;

    //初回かどうか
    private bool is_First_Visit = true;

    // Use this for initialization
    void Start () {
        //スクリプトの取得
        _message = GetComponent<MessageDisplay>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
    }


    //ムービー
    public IEnumerator Previous_Movie() {
        //初期設定
        _pause.Set_Pausable(false);
        is_First_Visit = game_Manager.Is_First_Visit();
        StartCoroutine("Player_Timeline");
        StartCoroutine("Reimu_Timeline");
        StartCoroutine("Back_Ground_Timeline");

        movie_Progress = 1;

        yield return new WaitForSeconds(1.0f);
        if (is_First_Visit) {
            _message.Start_Display("ReimuText", 7, 9);      //霊夢、紫会話
            yield return new WaitUntil(_message.End_Message);
        }
        movie_Progress = 2;

        yield return new WaitForSeconds(2.0f);

        if (is_First_Visit) {
            _message.Start_Display("ReimuText", 10, 18);      //リグル、霊夢、紫会話
            yield return new WaitUntil(_message.End_Message);
        }
        movie_Progress = 3;

        //戦闘開始
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Change_BGM_Index(5);
        _pause.Set_Pausable(true);
    }


    //自機
    private IEnumerator Player_Timeline() {
        //初期設定
        GameObject player = GameObject.FindWithTag("PlayerTag");
        WriggleController player_Controller = player.GetComponent<WriggleController>();
        player_Controller.Set_Playable(false);
        player_Controller.Set_Is_Fly(true);
        player_Controller.Change_Parameter("FlyBool");
        player_Controller.Set_Gravity(0);

        while (movie_Progress < 2) { yield return null; }   //霊夢、紫会話

        //登場
        MoveBetweenTwoPoints player_Move = player.AddComponent<MoveBetweenTwoPoints>();
        player_Move.Start_Move(new Vector3(160f, 0), 64f, 0.02f);
        yield return new WaitUntil(player_Move.End_Move);
        player_Controller.Change_Parameter("FlyIdleBool");
        player.transform.localScale = new Vector3(-1, 1, 1);

        while (movie_Progress < 3) { yield return null; }   //リグル、霊夢、紫会話

        //戦闘開始、
        player_Controller.Set_Playable(true);
        player_Controller.Set_Gravity(50);
    }


    //霊夢
    private IEnumerator Reimu_Timeline() {
        GameObject reimu = GameObject.Find("Reimu");
        ReimuController reimu_Controller = reimu.GetComponent<ReimuController>();
        reimu_Controller.Change_Parameter("DashBool");

        while(movie_Progress < 3) { yield return null; }

        //戦闘開始
        reimu_Controller.start_Battle = true;
    }


    //背景
    private IEnumerator Back_Ground_Timeline() {
        GameObject[] scroll_Grounds = new GameObject[2];
        scroll_Grounds[0] = GameObject.Find("ScrollGrid").transform.GetChild(0).gameObject;
        scroll_Grounds[1] = GameObject.Find("ScrollGrid").transform.GetChild(1).gameObject;

        while (movie_Progress < 4) {
            for (int i = 0; i < 2; i++) {
                scroll_Grounds[i].transform.position += new Vector3(-2f, 0, 0) * Time.timeScale;
                if(scroll_Grounds[i].transform.position.x < -560f) {
                    scroll_Grounds[i].transform.position = new Vector3(560f, scroll_Grounds[i].transform.position.y);
                }
            }        
             yield return null;
        }
    }


    //ボスクリア後
    public IEnumerator Clear_Movie() {
        _pause.Set_Pausable(false);
        //霊夢止める
        GameObject.Find("Reimu").GetComponent<ReimuAttack>().StopAllCoroutines();
        GameObject.Find("Reimu").GetComponent<MoveBetweenTwoPoints>().StopAllCoroutines();
        //自機無敵
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
        //明転
        yield return new WaitForSeconds(3.0f);
        SpriteRenderer white_Out_Sprite = GameObject.Find("WhiteOut").GetComponent<SpriteRenderer>();
        while (white_Out_Sprite.color.a <= 1) {
            white_Out_Sprite.color += new Color(0, 0, 0, 0.005f);
            yield return null;
        }
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Start_Fade_Out(0.01f, 2.1f);
        yield return new WaitForSeconds(2.0f);
        //シーン遷移
        SceneManager.LoadScene("Base_1Scene");
        _pause.Set_Pausable(true);
    }
    
}
