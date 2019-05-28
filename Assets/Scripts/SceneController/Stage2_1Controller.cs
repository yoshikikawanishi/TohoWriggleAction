using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_1Controller : MonoBehaviour {

    //テキストの加工前の一行を入れる変数
    public string[] textMessage;
    //テキストの複数列を入れる2次元は配列
    public string[,] textWords;
    //テキスト内の行数を取得する変数
    private int rowLength;
    //テキスト内の列数を取得する変数
    private int columnLength;

    //カメラ
    private GameObject main_Camera;

    //敵生成開始
    private bool start_Enemy_Gen = false;


    // Use this for initialization
    void Start () {
        //カメラ
        main_Camera = GameObject.Find("Main Camera");

        //テキストファイルの読み込み
        Read_Text("Stage2_1_Enemy_Gen");
    }

    // Update is called once per frame
    void Update () {
        //敵生成開始
        if (!start_Enemy_Gen && main_Camera.transform.position.x > 6700f) {
            start_Enemy_Gen = true;
            StartCoroutine("Enemy_Gen");
        }
	}


    //テキストファイルの読み込み
    private void Read_Text(string file_Name) {
        TextAsset textasset = new TextAsset();
        textasset = Resources.Load("Texts/" + file_Name, typeof(TextAsset)) as TextAsset; //Resourcesフォルダから対象テキストを取得
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


    //敵生成
    private IEnumerator Enemy_Gen() {
        for(int i = 1; i < rowLength; i++) {
            yield return new WaitForSeconds(float.Parse(textWords[i, 1]));
            GameObject enemy = Instantiate(Resources.Load("Enemy/" + textWords[i, 0]) as GameObject);
            Vector3 pos = new Vector3(main_Camera.transform.position.x + float.Parse(textWords[i, 2]), float.Parse(textWords[i, 3]));
            enemy.transform.position = pos;
        }
    }
}
