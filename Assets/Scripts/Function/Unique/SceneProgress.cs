using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneProgress : MonoBehaviour {

    //進行度の更新、初めて訪れたかどうか
    public bool Update_Is_Visited(string loaded_Scene) {
       
        //ファイル読み込み
        string filePath = Application.dataPath + @"\SceneProgress.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);

        //loaded_Sceneがファイルにあるか確認
        for(int i = 0; i < text.rowLength; i++) {
            if(text.textWords[i, 0] == loaded_Scene) {
                return false;
            }
        }

        //ファイルに名前がなかった場合書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#HasVisitedScene");
        sw_Clear.Flush();
        sw_Clear.Close();
        StreamWriter sw = new StreamWriter(filePath, true);

        for(int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            sw.Write(text.textWords[i, 0]);
        }
        sw.Write("\n" + loaded_Scene);
        sw.Flush();
        sw.Close();

        return true;
    }


    //シーンの検索
    public bool Is_Exist_Scene(string scene_Name) {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\SceneProgress.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);
        for(int i = 1; i < text.rowLength; i++) {
            if(text.textWords[i, 0] == scene_Name) {
                return true;
            }
        }
        return false;
    }


    //データの消去
    public void Delete_Progress() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\SceneProgress.txt";
       
        //1行目以外消す
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#HasVisitedScene");
        sw_Clear.Flush();
        sw_Clear.Close();
    }

}
