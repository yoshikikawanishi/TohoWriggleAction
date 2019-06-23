﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopClerk : TalkCharacter {

    
    //ショップキャンバス
    private GameObject shop_Canvas;


	// Use this for initialization
	new void Start () {
        base.Start();
        //ショップキャンバス
        shop_Canvas = GameObject.Find("ShopCanvas");
        shop_Canvas.SetActive(false);
    }


    //会話
    override protected IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        //自機を止める
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        player_Controller.Set_Playable(false);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player_Controller.Change_Parameter("IdleBool");
        //ポーズ禁止
        PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        _pause.Set_Pausable(false);
        //メッセージ開始
        _message.Start_Display(fileName, start_ID, end_ID);
        yield return new WaitUntil(_message.End_Message);
        //トレード開始
        Start_Trade();
    }

    //ショップ
    private void Start_Trade() {
        //ショップ画面表示
        shop_Canvas.SetActive(true);
        GameObject.Find("Item1Button").GetComponent<Button>().Select();
    }

    /*---------------ショップのボタン関数--------------*/
    public void Quit_Button() {
        //自機の移動とポーズの解除
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        player_Controller.Set_Playable(true);
        PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        _pause.Set_Pausable(true);
        //仮面を消す
        shop_Canvas.SetActive(false);
        //終了
        end_Talk = true;
        is_Talking = false;
    }


}
