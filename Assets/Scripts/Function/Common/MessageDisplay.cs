using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour {

    //テキストの加工前の一行を入れる変数
    public string[] textMessage;
    //テキストの複数列を入れる2次元配列
    public string[,] textWords;
    //テキスト内の行数を取得する変数
    private int rowLength = 0;
    //テキスト内の列数を取得する変数
    private int columnLength = 0;

    //表示する用のパネル
    GameObject messagePanel;
    //メッセージ表示のテキストコンポーネント
    private Text messageText;
    //キャラ名表示のテキストコンポーネント
    private Text nameText;

    //表示するID番号
    private int start_ID = 1;
    private int end_ID = 1;

    //表示終了、
    private bool endMessage = false;
    
    //メッセージ表示の速度
    private float textSpeed = 0.07f;
   

    // Update is called once per frame
    private void Update() {
        //表示スピード
        if (Input.GetButtonDown("Submit")) {
            textSpeed = 0.005f;
        }
        if (Input.GetButtonUp("Submit")) {
            textSpeed = 0.07f;
        }
    }


    //表示開始
    public void Start_Display(string fileName, int start_ID, int end_ID) {
        //テキストファイルの読み込み
        Read_Text(fileName);
        //セリフ枠の表示、テキスト、アイコンの取得
        Display_Panel();
        //番号の代入
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        //セリフの表示
        StartCoroutine("Print_Message");
    }



    //テキストファイルの読み込み
    private void Read_Text(string fileName) {
        TextAsset textasset = new TextAsset(); //テキストファイルのデータを取得するインスタンスを作成
        textasset = Resources.Load("Texts/" + fileName, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
        if(textasset == null) {
            Debug.Log("Can't Find TextFile");
            return;
        }
        string TextLines = textasset.text; //テキスト全体をstring型で入れる変数を用意して入れる

        //Splitで一行づつを代入した1次配列を作成
        textMessage = TextLines.Split('\n'); //

        //行数と列数を取得
        columnLength = textMessage[0].Split('\t').Length;
        rowLength = textMessage.Length;

        //2次配列を定義
        textWords = new string[rowLength, columnLength];

        for (int i = 0; i < rowLength; i++) {

            string[] tempWords = textMessage[i].Split('\t'); //textMessageをカンマごとに分けたものを一時的にtempWordsに代入

            for (int n = 0; n < columnLength; n++) {
                textWords[i, n] = tempWords[n]; //2次配列textWordsにカンマごとに分けたtempWordsを代入していく
            }
        }
    }


    //メッセージパネルの表示
    private void Display_Panel() {
        GameObject canvas = GameObject.Find("Canvas");
        messagePanel = canvas.transform.Find("MessagePanel").gameObject;
        messagePanel.SetActive(true);
        //テキストを取得
        messageText = messagePanel.transform.GetChild(0).GetComponent<Text>();
        messageText.text = "";
        //キャラ名表示のテキストを取得
        nameText = messagePanel.transform.GetChild(1).GetComponent<Text>();
        nameText.text = "";
    }


    //メッセージ表示
    private IEnumerator Print_Message() {
        //効果音の取得
        AudioSource sound = messagePanel.GetComponent<AudioSource>();
        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //名前とアイコン
            nameText.text = textWords[i, 1];
            //セリフ
            int lineLength = textWords[i, 3].Length;
            for(int j = 0; j < lineLength; j++) {
                if (textWords[i, 3][j] == '/') {
                    messageText.text += "\n";
                }
                else {
                    messageText.text += textWords[i, 3][j];
                    sound.Play();
                }
                for (float t = 0; t < textSpeed; t += 0.016f) { yield return null; }
            }
            //1行分表示後決定が押されるのを待つ
            yield return new WaitUntil(Wait_Input_Z);
            //次の行へ
            messageText.text = "";
        }
        //表示終了
        messagePanel.SetActive(false);
        endMessage = true;
    }


    //Zが入力されるのを待つ
    private bool Wait_Input_Z() {
        if (Input.GetButtonDown("Submit")) {
            return true;
        }
        return false;
    }


    //メッセージ表示終了を他スクリプトで検知する用
    public bool End_Message() {
        if (endMessage) {
            endMessage = false;
            return true;
        }
        return false;
    }
    
}
