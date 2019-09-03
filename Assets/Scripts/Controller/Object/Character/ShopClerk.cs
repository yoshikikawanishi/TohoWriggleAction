using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopClerk : TalkCharacter {

    
    //ショップキャンバス
    private GameObject shop_Canvas;

    private int life_Up_Price = 8;


	// Use this for initialization
	new void Start () {
        base.Start();
        //ショップキャンバス
        shop_Canvas = GameObject.Find("ShopCanvas");
        shop_Canvas.SetActive(false);
    }


    //会話
    override protected IEnumerator Talk() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        if (player_Controller.Get_Playable()) {
            is_Talking = true;
            end_Talk = false;
            //自機を止める
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
    }

    //ショップ
    private void Start_Trade() {
        //ショップ画面表示
        Set_Up_Item_Price();
        shop_Canvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        GameObject.Find("Item1Button").GetComponent<Button>().Select();
    }


    //値段設定
    private void Set_Up_Item_Price() {
        PlayerManager player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        switch (player_Manager.life) {
            case 1: life_Up_Price = 4; break;
            case 2: life_Up_Price = 8; break;
            case 3: life_Up_Price = 16; break;
            case 4: life_Up_Price = 32; break;
            default: life_Up_Price = 64; break;
        }
        //テキスト変更
        shop_Canvas.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = life_Up_Price.ToString();
    }


    /*---------------ショップのボタン関数--------------*/
    public void Quit_Button() {
        StartCoroutine("Quit_Trade");
    }

    private IEnumerator Quit_Trade() {
        yield return new WaitForSeconds(0.1f);
        //自機の移動とポーズの解除
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        player_Controller.Set_Playable(true);
        PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        _pause.Set_Pausable(true);
        //画面を消す
        shop_Canvas.SetActive(false);
        //終了
        end_Talk = true;
        is_Talking = false;
    }


    //ライフ回復
    public void Life_Up_Button() {
        PlayerManager player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        if (player_Manager.power >= life_Up_Price) {
            player_Manager.Life_Up();
            player_Manager.Set_Power(player_Manager.power - life_Up_Price);
            Quit_Button();
        }
    }


    //1UP
    public void Get_Stock_Button() {
        PlayerManager player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        if (player_Manager.power >= 64) {
            player_Manager.Get_Stock();
            player_Manager.Set_Power(player_Manager.power - 64);
            Quit_Button();
        }
    }


}
