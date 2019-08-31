using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScene : MonoBehaviour {

    //キャンバス
    private GameObject canvas;
    private CanvasGroup canvas_Group;
    private GameObject confirm_Canvas;


	// Use this for initialization
	void Start () {
        //取得
        canvas = GameObject.Find("Canvas");
        canvas_Group = canvas.GetComponent<CanvasGroup>();
        confirm_Canvas = GameObject.Find("ConfirmCanvas");
        confirm_Canvas.SetActive(false);

        //ボタンの初期位置
        Button continue_Button = GameObject.Find("ContinueButton").GetComponent<Button>();
        continue_Button.Select();

        //ドレミーハットの表示
        Display_Doremy_Hat();

        //エクストラステージの開放
        Open_Extra_Stage();
	}


    //ドレミーハットの表示
    private void Display_Doremy_Hat() {
        //取得
        GameObject[] doremy_Hat_Images = new GameObject[3];
        GameObject parent = GameObject.Find("DoremyHats");
        for(int i = 0; i < 3; i++) {
            doremy_Hat_Images[i] = parent.transform.GetChild(i).gameObject;
            doremy_Hat_Images[i].SetActive(false);
        }
        //ファイル読み込み
        string filePath = Application.dataPath + @"\DoremyHat.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);
        //反映
        for(int i = 1; i < text.rowLength; i++) {
            if(text.textWords[i, 1] == "true") {
                doremy_Hat_Images[i-1].SetActive(true);
            }
        }
    }


    //エクストラステージの開放
    private void Open_Extra_Stage() {
        //ボタンの取得
        GameObject extra_Button = GameObject.Find("ExtraButton");
        //ファイル読み込み
        string filePath = Application.dataPath + @"\DoremyHat.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);
        //帽子がすべて集まっているかどうか
        bool is_Open = true;
        for(int i = 1; i < text.rowLength; i++) {
            if(text.textWords[i, 1] == "false") {
                is_Open = false;
            }
        }
        //集まっていないとき押せなくする
        if (!is_Open) {
            extra_Button.GetComponent<Button>().interactable = false;
            extra_Button.GetComponentInChildren<Text>().color = new Color(1f, 0.8f, 0.8f);
        }
    }


    //確認画面の表示
    public void Display_Confirm_Canvas() {
        if(confirm_Canvas != null) {
            confirm_Canvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            canvas_Group.interactable = false;
            confirm_Canvas.transform.GetChild(1).GetComponent<Button>().Select();
        }
    }

    //確認画面の消去
    public void Delete_Confirm_Canvas() {
        if(confirm_Canvas != null) {
            confirm_Canvas.SetActive(false);
            canvas_Group.interactable = true;
            EventSystem.current.SetSelectedGameObject(null);
            Button continue_Button = GameObject.Find("ContinueButton").GetComponent<Button>();
            continue_Button.Select();
        }
    }
	
}
