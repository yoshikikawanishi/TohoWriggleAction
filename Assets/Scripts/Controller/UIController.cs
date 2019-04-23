using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    //スクリプト
    private PlayerManager _playerManager;

    //UI
    private GameObject[] life_UI = new GameObject[9];
    private Text stock_UI;
    private Text power_UI;
    private Text score_UI;

    //数値
    private int life_Num = 0;
    private int stock_Num = 0;
    private int power_Value = 0;
    private int score_Value = 0;


	// Use this for initialization
	void Start () {
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();

        //UIの取得
        for(int i = 0; i < 9; i++) {
            life_UI[i] = transform.Find("LifeUIs").GetChild(i).gameObject;
        }
        stock_UI = transform.Find("StockUI").GetComponent<Text>();
        power_UI = transform.Find("PowerUI").GetComponent<Text>();
        score_UI = transform.Find("ScoreUI").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        //UI表示
        Life_UI();
        Power_UI();
        Score_UI();
        Stock_UI();

	}


    //ライフUの表示I
    private void Life_UI() {
        if(life_Num != _playerManager.life) {
            life_Num = _playerManager.life;
            for(int i = 0; i < 9; i++) {
                life_UI[i].gameObject.SetActive(false);
            }
            for(int i = 0; i < life_Num; i++) {
                life_UI[i].gameObject.SetActive(true);
            }
        }
    }


    //ストックUIの表示
    private void Stock_UI() {
        if(stock_Num != _playerManager.stock) {
            stock_Num = _playerManager.stock;
            stock_UI.text = "×" + _playerManager.stock.ToString();
        }
    }


    //パワーUIの表示
    private void Power_UI() {
        if(power_Value != _playerManager.power) {
            power_Value = _playerManager.power;
            power_UI.text = "Power:" + power_Value.ToString("D3");
        }
    }


    //スコアUIの表示
    private void Score_UI() {
        if (score_Value != _playerManager.score) {
            score_Value = _playerManager.score;
            score_UI.text = "Score:" + score_Value.ToString("D10");
        }
    }

}
