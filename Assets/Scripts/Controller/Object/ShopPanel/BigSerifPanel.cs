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
        yield return new WaitForSeconds(0.4f);
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
        _sentence.Start_Display("YamameText", topic_Num + 8, text);
        yield return new WaitUntil(_sentence.End_Display);
        yield return new WaitForSeconds(0.4f);
        shop_Manager.Open_Talk_Table();
    }


}
