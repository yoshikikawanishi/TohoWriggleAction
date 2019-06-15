﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    //ライフ
    public int life = 3;
    //ストック
    public int stock = 3;
    //スコア
    public int score = 0;
    //パワー
    public int power = 0;
    //コンテニュー回数
    public int continue_Count = 0;

    //オプションタイプ
    public string option_Type = "Flies";


    //自機の復活の処理
    public IEnumerator Revive() {
        //パワーとオプションの保存
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetString("Option", option_Type);
        yield return new WaitForSeconds(1.0f);
        //セーブのロード
        GetComponent<GameManager>().StartCoroutine("LoadData");
        yield return null;
        life = 3;
        stock--;
        //点滅
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player.GetComponentInChildren<PlayerCollision>().StartCoroutine("Blink");
        }
    }


    //ゲームオーバーの処理
    public IEnumerator Game_Over() {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOverScene");
        life = 3;
        stock = 3;
        continue_Count++;
        PlayerPrefs.SetInt("Life", life);
        PlayerPrefs.SetInt("Stock", stock);
        PlayerPrefs.SetInt("Continue", continue_Count);
    }


    //コンテニューの処理
    public IEnumerator Continue() {
        //セーブのロード
        GetComponent<GameManager>().StartCoroutine("LoadData");
        yield return null;
        life = 3;
        stock = 3;
    }


    //ライフアップ
    public void Life_Up() {
        if(life < 9) {
            life++;
            GameObject.FindWithTag("PlayerTag").transform.GetChild(7).GetComponents<AudioSource>()[0].Play();
        }     
    }


    //ストックの回復
    public void Get_Stock() {
        if(stock < 10) {
            stock++;
            GameObject.FindWithTag("PlayerTag").transform.GetChild(7).GetComponents<AudioSource>()[1].Play();
        }
    }


    //点の獲得
    public void Get_Score() {
        score += 100;
        if(score % 30000 == 0) {
            stock++;
        }
    }


    //Pの獲得
    public void Get_Power() {
        if (power <= 128) {
            power++;
        }
    }


    //オプションタイプの変更
    public void Set_Option_Type(string next_Type) {
        switch (next_Type) {
            case "Flies": option_Type = next_Type; break;
            case "ButterFly": option_Type = next_Type; break;
            case "Beetle": option_Type = next_Type; break;
            case "Bee": option_Type = next_Type; break;
        }
    }
}
