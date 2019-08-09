using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallSerifPanel : MonoBehaviour {

    private SentenceDisplay _sentence;
    private Text text;

    // Use this for initialization
    void Awake() {
        _sentence = gameObject.AddComponent<SentenceDisplay>();
        text = GetComponentInChildren<Text>();
    }

    //商品を訪ねる
    public void Ask_Goods_Serif() {
        _sentence.Start_Display("YamameText", 4, text);
    }

    //商品を確認する
    public void Confirm_Goods_Serif() {
        _sentence.Start_Display("YamameText", 5, text);
    }

    //購入を感謝する
    public void Thank_Buying_Serif() {
        _sentence.Start_Display("YamameText", 6, text);
    }

    //パワーが足りない
    public void Run_Short_Power_Serif() {
        _sentence.Start_Display("YamameText", 7, text);
    }

    //会話選択
    public void Ask_Topic_Serif() {
        _sentence.Start_Display("YamameText", 8, text);
    }
}
