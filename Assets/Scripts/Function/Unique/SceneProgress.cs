using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneProgress : MonoBehaviour {

    //進行度の更新、初めて訪れたかどうか
    public bool Update_Is_Visited(string loaded_Scene) {
        bool is_First_Visited = true;

        //ファイル読み込み
        string filePath = Application.dataPath + @"\SceneProgress.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);

        //loaded_SceneのisVisitedの確認、書き換え
        for(int i = 0; i < text.rowLength; i++) {
            if(text.textWords[i, 0] == loaded_Scene) {
                if(text.textWords[i, 1] == "false") {
                    is_First_Visited = true;
                    text.textWords[i, 1] = "true";
                }
                else {
                    is_First_Visited = false;
                }
            }
        }

        //ファイルの書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#SceneName\t#isVisited");
        sw_Clear.Flush();
        sw_Clear.Close();
        StreamWriter sw = new StreamWriter(filePath, true);

        for(int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            sw.Write(text.textWords[i, 0] + "\t" + text.textWords[i, 1]);
        }
        sw.Flush();
        sw.Close();

        return is_First_Visited;
    }


    //データの消去
    public void Delete_Progress() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\SceneProgress.txt";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);

        //書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#SceneName\t#isVisited");
        sw_Clear.Flush();
        sw_Clear.Close();
        StreamWriter sw = new StreamWriter(filePath, true);

        for (int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            sw.Write(text.textWords[i, 0] + "\tfalse");
        }
        sw.Flush();
        sw.Close();
    }

}
