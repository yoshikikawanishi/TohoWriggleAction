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
