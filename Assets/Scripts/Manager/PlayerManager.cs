using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;

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


	// Use this for initialization
	void Start () {
        //スクリプト
        _gameManager = GetComponent<GameManager>();
	}


    //自機の復活の処理
    public IEnumerator Revive() {
        //パワーの保存
        PlayerPrefs.SetInt("Power", power);
        yield return new WaitForSeconds(1.0f);
        //セーブのロード
        _gameManager.StartCoroutine("LoadData");
        yield return null;
        life = 3;
        stock--;
        //点滅
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player.GetComponentInChildren<PlayerCollisionController>().StartCoroutine("Blink");
        }
    }


    //コンテニューの処理
    public IEnumerator Continue() {
        //セーブのロード
        _gameManager.StartCoroutine("LoadData");
        yield return null;
        life = 3;
        stock = 3;
    }


    //ライフアップ
    public void Life_Up() {
        if(life < 9) {
            life++;
        }     
    }


    //ストックの回復
    public void Get_Stock() {
        if(stock < 10) {
            stock++;
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
