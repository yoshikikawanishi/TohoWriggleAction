using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    //スクリプト
    private PlayerManager _playerManager;

    //自機
    private GameObject player;
    private WriggleController _playerController;

    //UI
    private GameObject[] life_UI = new GameObject[9];
    private Text stock_UI;
    private Text power_UI;
    private Text score_UI;
    private Slider fly_Time_UI;
    private Image fly_Time_Image;

    //オーディオ
    private AudioSource alert_Sound;

    //数値
    private int life_Num = 0;
    private int stock_Num = 0;
    private int power_Value = 0;
    private int score_Value = 0;
    private float fly_Time_Value = 0;


	// Use this for initialization
	void Start () {
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            _playerController = player.GetComponent<WriggleController>();
        }
        //UIの取得
        for(int i = 0; i < 9; i++) {
            life_UI[i] = transform.Find("LifeUIs").GetChild(i).gameObject;
        }
        stock_UI = transform.Find("StockUI").GetComponent<Text>();
        power_UI = transform.Find("PowerUI").GetComponent<Text>();
        score_UI = transform.Find("ScoreUI").GetComponent<Text>();
        fly_Time_UI = transform.Find("FlyTimeUI").GetComponent<Slider>();
        fly_Time_Image = GameObject.Find("FlyTimeFill").GetComponent<Image>();
        //オーディオ
        alert_Sound = transform.Find("FlyTimeUI").GetComponent<AudioSource>(); 
	}
	
	// Update is called once per frame
	void Update () {

        //UI表示
        Life_UI();
        Power_UI();
        Score_UI();
        Stock_UI();
        Fly_Time_UI();
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


    //飛行時間のUIの表示
    private void Fly_Time_UI() {
        //PlayerControllerから数値を取得
        if (player != null) {
            if (fly_Time_Value != _playerController.Get_Fly_Time()) {
                fly_Time_Value = _playerController.Get_Fly_Time();
                fly_Time_UI.value = 5 - fly_Time_Value;
            }
        }
        //少なくなってきたとき赤色にして警告音
        if(fly_Time_UI.value < 1.5f && !Mathf.Approximately(1, fly_Time_Image.color.r)) {
            fly_Time_Image.color = new Color(1, 0.25f, 0.25f);
            alert_Sound.Play();
        }
        //元の色に戻す
        else if(fly_Time_UI.value >= 1.5f && !Mathf.Approximately(0.4f, fly_Time_Image.color.r)) {
            fly_Time_Image.color = new Color(0.4f, 1, 0.5f);
        }
    }

}
