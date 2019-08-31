using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigSerifPanel : MonoBehaviour {

    private UnderGroundShoppingManager shop_Manager;
    private SentenceDisplay _sentence;
    private Text text;

    // Use this for initialization
    void Awake() {
        shop_Manager = GameObject.Find("Scripts").GetComponent<UnderGroundShoppingManager>();
        _sentence = gameObject.AddComponent<SentenceDisplay>();
        text = GetComponentInChildren<Text>();
    }


    //売るボタン時
    public IEnumerator Sold_Serif() {
        _sentence.Start_Display("YamameText", 2, text);
        yield return new WaitUntil(_sentence.End_Display);
        shop_Manager.Open_Menu();
    }

    //メニュー
    public void Welcome_Serif() {
        _sentence.Start_Display("YamameText", 1, text);
    }

    //店を出るとき
    public void Exit_Shop_Serif() {
        _sentence.Start_Display("YamameText", 3, text);
    }

    //話す
    public IEnumerator Talk_Serif(int topic_Num) {
        int start_ID = 9;
        int end_ID = 9;
        switch (topic_Num) {
            case 1: start_ID = 9; end_ID = 10; break;
            case 2: start_ID = 11; end_ID = 12; break;
            case 3: start_ID = 13; end_ID = 14; break;
        }
        for(int i = start_ID; i <= end_ID; i++) {
            _sentence.Start_Display("YamameText", i, text);
            yield return new WaitUntil(_sentence.End_Display);
        }
        shop_Manager.Open_Talk_Table();
    }


}
