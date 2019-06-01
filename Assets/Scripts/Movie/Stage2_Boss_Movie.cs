﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_Boss_Movie : MonoBehaviour {

    //自機、カメラ
    private GameObject player;
    private GameObject main_Camera;  

    //スクリプト
    private MessageDisplay _message;
    
    //ムービー終了検知用
    private bool is_End_Before_Movie = false;
    //メッセージ終了検知用
    private bool is_End_Message = false;

    
    // Use this for initialization
    void Awake () {
        //自機、カメラ
        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.Find("Main Camera");
        
        //スクリプト
        _message = GetComponent<MessageDisplay>();
    }
	

	// Update is called once per frame
	void Update () {
        //メッセージ表示終了
        if (_message.End_Message()) {
            is_End_Message = true;
        }
    }

    //LateUpdate
    private void LateUpdate() {
        //メッセージ表示終了の終了
        is_End_Message = false;
    }

    //メッセージ表示終了の検知用
    private bool Is_End_Message() {
        if (is_End_Message) {
            return true;
        }
        return false;
    }


    //ボス前ムービー
    public IEnumerator Before_Movie() {
        StartCoroutine("Wriggle_Timeline");
        StartCoroutine("Reimu_Timeline");
        StartCoroutine("Scroll_Ground");

        //カメラのスクロール止める
        main_Camera.GetComponent<CameraController>().Set_Can_Scroll(false);

        yield return new WaitForSeconds(1.0f);

        _message.Start_Display("ReimuText", 1, 1);  //霊夢発見セリフ
        yield return new WaitUntil(Is_End_Message);

        yield return new WaitForSeconds(2.0f);

        _message.Start_Display("ReimuText", 2, 3);  //キック前セリフ
        yield return new WaitUntil(Is_End_Message);

        yield return new WaitForSeconds(0.2f);

        _message.Start_Display("ReimuText", 4, 4);  //霊夢よけるセリフ
        yield return new WaitUntil(Is_End_Message);

        yield return new WaitForSeconds(0.5f);
        
        _message.Start_Display("ReimuText", 5, 5);  //霊夢無視セリフ
        yield return new WaitUntil(Is_End_Message);

        yield return new WaitForSeconds(1.0f);

        //カメラのスクロール始める
        main_Camera.GetComponent<CameraController>().Set_Can_Scroll(true);
        //ムービー終了
        is_End_Before_Movie = true;
    }


    //ボス前ムービーリグル
    private IEnumerator Wriggle_Timeline() {
        //初期設定
        WriggleController player_Controller = player.GetComponent<WriggleController>();
        Rigidbody2D player_Rigid = player.GetComponent<Rigidbody2D>();
        player_Controller.Set_Playable(false);
        player_Controller.Set_Gravity(0);
        player_Controller.is_Ground = false;
        player_Controller.Change_Parameter("FlyBool");

        //登場
        while (player.transform.position.x < -180f) {
            player.transform.position += new Vector3(1.5f, 0, 0);
            yield return null;
        }

        yield return new WaitUntil(Is_End_Message); //霊夢発見セリフ

        //キック開始位置まで移動
        Vector3 start_Pos = player.transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime) {
            player.transform.position = Vector3.Lerp(start_Pos, new Vector3(0, start_Pos.y + 32f, 0), t);
            yield return null;
        }
        //キックの溜め
        player.transform.localScale = new Vector3(-1, 1, 1);
        player_Controller.Change_Parameter("FlyIdleBool");
        for (float t = 0; t < 1; t += Time.deltaTime) {
            player.transform.position += new Vector3(0.2f, 0.2f);
            yield return null;
        }

        yield return new WaitUntil(Is_End_Message); //キック前セリフ

        //キック
        player_Controller.Change_Parameter("KickBool");
        player_Controller.Set_Gravity(50f);
        player_Rigid.velocity = new Vector2(-200f, -250f);
        yield return new WaitForSeconds(2.0f);

        //終了設定
        player_Controller.Set_Playable(true);
    }


    //ボス前ムービー霊夢
    private IEnumerator Reimu_Timeline() {
        //初期設定
        GameObject reimu = GameObject.Find("Reimu");
        ReimuWayController reimu_Controller = reimu.GetComponent<ReimuWayController>();
        reimu_Controller.Set_Is_Shot_Bullet(false);

        yield return new WaitUntil(Is_End_Message); //霊夢発見
        yield return null;
        yield return new WaitUntil(Is_End_Message); //キック前セリフ

        //キックよける
        yield return new WaitForSeconds(0.2f);
        reimu.transform.position += new Vector3(-10f, 0);

        yield return new WaitUntil(Is_End_Message); //霊夢よけるセリフ
        yield return null;
        yield return new WaitUntil(Is_End_Message); //霊夢無視セリフ

        //霊夢の移動
        reimu = GameObject.Find("Reimu");
        MoveBetweenTwoPoints reimu_Move = reimu.AddComponent<MoveBetweenTwoPoints>();
        reimu_Move.Set_Status(64f, 0.02f);
        reimu_Move.StartCoroutine("Move_Two_Points", new Vector3(128f, 16f));
        yield return new WaitUntil(reimu_Move.End_Move);
        //ショット撃ち始める
        reimu_Controller.Set_Is_Shot_Bullet(true);
    }


    //ボス前ムービー背景
    private IEnumerator Scroll_Ground() {
        //スクロールする地面
        GameObject[] scroll_Grounds = new GameObject[2];
        scroll_Grounds[0] = GameObject.Find("ScrollGround1");
        scroll_Grounds[1] = GameObject.Find("ScrollGround2");
        while (!Is_End_Message()) { //霊夢発見セリフ
            yield return null;
            for (int i = 0; i < 2; i++) {
                scroll_Grounds[i].transform.position += new Vector3(-2f, 0, 0);
                if (scroll_Grounds[i].transform.position.x < -570f) {
                    scroll_Grounds[i].transform.position = new Vector3(500f, 0, 0);
                }
            }
        }
        yield return null;
        while (!Is_End_Message()) { //キック前セリフ
            yield return null;
            for (int i = 0; i < 2; i++) {
                scroll_Grounds[i].transform.position += new Vector3(-2f, 0, 0);
                if (scroll_Grounds[i].transform.position.x < -570f) {
                    scroll_Grounds[i].transform.position = new Vector3(500f, 0, 0);
                }
            }
        }
    }
}