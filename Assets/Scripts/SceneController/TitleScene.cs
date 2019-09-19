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

    //エクストラクリアマーク
    [SerializeField] private GameObject extra_Clear_Mark;

    
    // Use this for initialization
    void Start () {
        //取得
        canvas = GameObject.Find("Canvas");
        canvas_Group = canvas.GetComponent<CanvasGroup>();
        confirm_Canvas = GameObject.Find("ConfirmCanvas");
        confirm_Canvas.SetActive(false);

        //ボタンの初期位置
        GameObject continue_Button = GameObject.Find("ContinueButton");
        GameObject play_Guide_Button = GameObject.Find("PlayGuideButton");
        if (PlayerPrefs.HasKey("Scene")) {
            continue_Button.GetComponent<Button>().Select();
        }
        else {
            continue_Button.GetComponent<Button>().interactable = false;
            continue_Button.GetComponentInChildren<Text>().color = new Color(1f, 0.8f, 0.8f);
            play_Guide_Button.GetComponent<Button>().Select();
        }

        //ハイスコアの表示
        Display_High_Score();
        //エクストラクリアマークの表示
        Display_Clear_Mark();

        //エクストラステージの開放、ドレミー帽子の表示
        Judge_Extra_Stage();

	}


    //ハイスコアの表示
    private void Display_High_Score() {
        GameObject high_Score_Text = GameObject.Find("HighScoreText");
        int high_Score = ClearDataManager.Get_High_Score();
        high_Score_Text.GetComponent<Text>().text = "High Score : " + high_Score.ToString("D10");
    }


    //エクストラクリアマークの表示
    private void Display_Clear_Mark() {
        if (ClearDataManager.Is_Clear_Extra()) {
            extra_Clear_Mark.SetActive(true);
        }
    }


    //エクストラステージの開放、ドレミー帽子の表示
    private void Judge_Extra_Stage() {
        //取得
        GameObject extra_Button = GameObject.Find("ExtraButton");
        GameObject[] doremy_Hat_Images = new GameObject[3];
        GameObject parent = GameObject.Find("DoremyHats");
        for (int i = 0; i < 3; i++) {
            doremy_Hat_Images[i] = parent.transform.GetChild(i).gameObject;
            doremy_Hat_Images[i].SetActive(false);
        }
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\DoremyHat.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);
        
        //帽子の収集率を反映
        bool is_Open = true;
        for(int i = 1; i < text.rowLength; i++) {
            if(text.textWords[i, 1] == "false") {
                is_Open = false;
            }
            if (text.textWords[i, 1] == "true") {
                doremy_Hat_Images[i - 1].SetActive(true);
            }
        }
        //集まっていないときボタンを押せなくする
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
