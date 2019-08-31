using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderGroundShoppingManager : MonoBehaviour {


    //パネル
    [SerializeField] private GameObject menu_Panel;
    [SerializeField] private GameObject goods_Panel;
    [SerializeField] private GameObject talk_Table_Panel;
    [SerializeField] private GameObject big_Serif_Panel;
    [SerializeField] private GameObject small_Serif_Panel;
    [SerializeField] private GameObject comment_Panel;
    //キャンバス
    [SerializeField] private GameObject shop_Canvas;
    [SerializeField] private GameObject confirm_Canvas;

    //今のパワーを表示
    private PlayerManager player_Manager;
    private Text now_Power_Text;
    private int power;

    //選択した商品保存用
    private string selecte_Option;

    //コンポーネント
    private CanvasGroup canvas_Group;

    //スクリプト
    private UnderGroundBackGround _background;


	// Use this for initialization
	void Start () {
        //コンポーネント
        canvas_Group = shop_Canvas.GetComponent<CanvasGroup>();
        //スクリプト
        _background = GetComponent<UnderGroundBackGround>();
        //パワー表示
        player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        now_Power_Text = GameObject.Find("PowerText").GetComponent<Text>();
        now_Power_Text.text = power.ToString() + " P";
        //入店
        StartCoroutine("Enter_Shop");

	}

    //Update
    private void Update() {
        //パワー表示
        if(power != player_Manager.power) {
            power = player_Manager.power;
            now_Power_Text.text = power.ToString() + " P";
        }
    }


    //パネル切り替え
    /*
        left_Frame == 0:なし/1:セリフパネル/2:商品パネル/3:話題表パネル
        right_Frame == 0:なし/1:メニューパネル/2:セリフパネル
        up_Frame == 0:なし/1:コメントパネル
    */
    public void Change_Panel(int left_Frame, int right_Frame, int up_Frame) {
        menu_Panel.SetActive(false);
        goods_Panel.SetActive(false);
        talk_Table_Panel.SetActive(false);
        big_Serif_Panel.SetActive(false);
        small_Serif_Panel.SetActive(false);
        comment_Panel.SetActive(false);
        
        switch (left_Frame) {
            case 0: goods_Panel.SetActive(false); talk_Table_Panel.SetActive(false); big_Serif_Panel.SetActive(false); break;
            case 1: goods_Panel.SetActive(false); talk_Table_Panel.SetActive(false); big_Serif_Panel.SetActive(true); break;
            case 2: goods_Panel.SetActive(true); talk_Table_Panel.SetActive(false); big_Serif_Panel.SetActive(false); break;
            case 3: goods_Panel.SetActive(false); talk_Table_Panel.SetActive(true); big_Serif_Panel.SetActive(false); break;
        }
        switch (right_Frame) {
            case 0: menu_Panel.SetActive(false); small_Serif_Panel.SetActive(false); break;
            case 1: menu_Panel.SetActive(true); small_Serif_Panel.SetActive(false); break;
            case 2: menu_Panel.SetActive(false); small_Serif_Panel.SetActive(true); break;
        }
        switch (up_Frame) {
            case 0: comment_Panel.SetActive(false); break;
            case 1: comment_Panel.SetActive(true); break;
        }
    }

    //入店時
    private IEnumerator Enter_Shop() {
        Change_Panel(0, 0, 0);
        GetComponent<FadeInOut>().Start_Fade_In();
        yield return new WaitForSeconds(1.0f);
        Open_Menu();
    }

    //メニュー
    public void Open_Menu() {
        Change_Panel(1, 1, 0);
        _background.Restore_Back_Ground();
        big_Serif_Panel.GetComponent<BigSerifPanel>().Welcome_Serif();
    }

    //商品欄表示
    public void Open_Goods_Table() {
        Change_Panel(2, 2, 1);
        small_Serif_Panel.GetComponent<SmallSerifPanel>().Ask_Goods_Serif();
    }

    //売る
    public void Sold() {
        Change_Panel(1, 0, 0);
        big_Serif_Panel.GetComponent<BigSerifPanel>().StartCoroutine("Sold_Serif");
    }

    //話す欄表示
    public void Open_Talk_Table() {
        Change_Panel(3, 2, 0);
        small_Serif_Panel.GetComponent<SmallSerifPanel>().Ask_Topic_Serif();
    }

    //話す
    public void Talk(int topic_Num) {
        Change_Panel(1, 0, 0);
        big_Serif_Panel.GetComponent<BigSerifPanel>().StartCoroutine("Talk_Serif", topic_Num);
    }

    //店を出る
    public void Exit_Shop() {
        Change_Panel(1, 0, 0);
        big_Serif_Panel.GetComponent<BigSerifPanel>().Exit_Shop_Serif();
        _background.Change_Back_Ground();
        StartCoroutine("Exit");
    }
    private IEnumerator Exit() {
        yield return new WaitForSeconds(0.5f);
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(1.5f);
        GameManager game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        game_Manager.StartCoroutine(game_Manager.Load_Scene("Stage3_2Scene", new Vector2(10456, -76f)));
    }

    //商品の確認
    private void Confirm_Goods() {
        Change_Panel(2, 2, 1);
        _background.Restore_Back_Ground();
        if (power >= 10) {
            small_Serif_Panel.GetComponent<SmallSerifPanel>().Confirm_Goods_Serif();
            canvas_Group.interactable = false;
            confirm_Canvas.SetActive(true);
        }
        else {
            small_Serif_Panel.GetComponent<SmallSerifPanel>().Run_Short_Power_Serif();
        }
    }

    //商品購入後
    private void Bought_Goods() {
        confirm_Canvas.SetActive(false);
        small_Serif_Panel.GetComponent<SmallSerifPanel>().Thank_Buying_Serif();
        canvas_Group.interactable = true;
        GameObject.Find("GoodsPanel").transform.GetChild(4).GetComponent<Button>().Select();
        _background.Change_Back_Ground();
    }
    

    /*-------ボタン--------*/
    //メニュー
    public void Buy_Button() {
        Open_Goods_Table();
    }

    public void Sold_Button() {
        Sold();
    }

    public void Talk_Button() {
        Open_Talk_Table();
    }

    public void Exit_Shop_Button() {
        Exit_Shop();
    }

    //商品
    public void Flies_Button() {
        selecte_Option = "Flies";
        Confirm_Goods();
    }

    public void Butterfly_Button() {
        selecte_Option = "ButterFly";
        Confirm_Goods();
    }

    public void Bee_Button() {
        selecte_Option = "Bee";
        Confirm_Goods();
    }

    public void Beetle_Button() {
        selecte_Option = "Beetle";
        Confirm_Goods();
    }

    public void Yes_Button() {
        player_Manager.Set_Option_Type(selecte_Option);
        player_Manager.Set_Power(power - 5);
        Bought_Goods();
    }

    public void No_Button() {
        confirm_Canvas.SetActive(false);
        canvas_Group.interactable = true;
        Open_Goods_Table();
    }

    public void Back_Menu_Button() {
        Open_Menu();
    }

    //話題
    public void First_Topic_Button() {
        Talk(1);
    }

    public void Second_Topic_Button() {
        Talk(2);
    }

    public void Third_Topic_Button() {
        Talk(3);
    }
}
