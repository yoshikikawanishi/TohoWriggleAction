using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BonusScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //初回以外アイテム消す
        if (!gm.Is_First_Visit()) {
            Destroy(GameObject.Find("Items"));
        }
        //帽子取得済みなら消す
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\DoremyHat.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);
        if (text.textWords[1, 1] == "true") {
            Destroy(GameObject.Find("DoremyHat"));
        }
    }


}
