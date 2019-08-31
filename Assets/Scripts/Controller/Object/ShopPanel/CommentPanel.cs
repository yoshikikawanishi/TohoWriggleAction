using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentPanel : MonoBehaviour {

    //選択中のボタン
    private GameObject now_Selected_Button;
    private GameObject old_Selected_Button;
    //商品ボタン
    private GameObject flies_Button;
    private GameObject butterfly_Button;
    private GameObject bee_Button;
    private GameObject beetle_Button;
    private GameObject back_Button;
    //テキスト
    private Text text;


    // Use this for initialization
    void Start() {
        //商品ボタン
        Transform goods_Panel = GameObject.Find("GoodsPanel").transform;
        flies_Button = goods_Panel.GetChild(0).gameObject;
        butterfly_Button = goods_Panel.GetChild(1).gameObject;
        bee_Button = goods_Panel.GetChild(2).gameObject;
        beetle_Button = goods_Panel.GetChild(3).gameObject;
        back_Button = goods_Panel.GetChild(4).gameObject;
        //テキスト
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update () {
        //選択中のボタンを確認
        now_Selected_Button = EventSystem.current.currentSelectedGameObject;
        if(now_Selected_Button != old_Selected_Button) {
            old_Selected_Button = now_Selected_Button;
            if (now_Selected_Button == flies_Button) {
                Flies_Comment();
            }
            else if (now_Selected_Button == butterfly_Button) {
                Butterfly_Comment();
            }
            else if (now_Selected_Button == bee_Button) {
                Bee_Comment();
            }
            else if (now_Selected_Button == beetle_Button) {
                Beetle_Comment();
            }
            else if(now_Selected_Button == back_Button) {
                Back_Comment();
            }
        }
        
	}

    //商品説明
    private void Flies_Comment() {
        text.text = "オプション：\nただのハエ";
    }

    private void Butterfly_Comment() {
        text.text = "オプション：\nたまがまがるよ";
    }

    private void Bee_Comment() {
        text.text = "オプション：\nはりがこわい";
    }

    private void Beetle_Comment() {
        text.text = "オプション：\nかっこいい！";
    }

    private void Back_Comment() {
        text.text = "メニューに戻ります";
    }
}
