using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderGroundShoppingManager : MonoBehaviour {

    //状況
    public enum STATE {
        welcome,
        menu,
        shopping,
        solding,
        talking,
        exit,
    }

    //パネルの場所
    public enum PANEL {
        left,
        right_down,
        right_up,
    }

    //現在の状況
    public STATE now_State;

    //パネル
    [SerializeField] private GameObject menu_Panel;
    [SerializeField] private GameObject goods_Panel;
    [SerializeField] private GameObject big_Serif_Panel;
    [SerializeField] private GameObject small_Serif_Panel;
    [SerializeField] private GameObject comment_Panel;
    private MenuPanel menu;
    private GoodsPanel goods;
    private BigSerifPanel big_Serif;
    private SmallSerifPanel small_Serif;
    private CommentPanel comment;


	// Use this for initialization
	void Start () {
        //入店
        now_State = STATE.welcome;
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (now_State) {
            case STATE.welcome: break;
            case STATE.menu: break;
            case STATE.shopping: break;
            case STATE.solding: break;
            case STATE.talking: break;
            case STATE.exit: break;
        }
	}

    //パネル切り替え
    public void Change_Panel(PANEL pos) {
        switch (pos) {
            case PANEL.left:
                goods_Panel.SetActive(!goods_Panel.activeSelf);
                big_Serif_Panel.SetActive(!big_Serif_Panel.activeSelf);
                break;
            case PANEL.right_down:
                menu_Panel.SetActive(!menu_Panel.activeSelf);
                small_Serif_Panel.SetActive(!small_Serif_Panel.activeSelf);
                break;
        }
    }


    /*-------ボタン--------*/
    //メニュー
    public void Buy_Button() {
        now_State = STATE.shopping;
    }

    public void Sold_Button() {
        now_State = STATE.solding;
    }

    public void Talk_Button() {
        now_State = STATE.talking;
    }

    public void Exit_Shop_Button() {
        now_State = STATE.exit;
    }

    //商品
    public void Flies_Button() {

    }

    public void Butterfly_Button() {

    }

    public void Bee_Button() {

    }

    public void Beetle_Button() {

    }

    public void God_Insect_Button() {

    }

    public void back_Menu_Button() {

    }

    //話題


}
