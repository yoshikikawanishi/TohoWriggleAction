using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ClearDataManager  {
    

    //クリアデータの保存
    public void Save_Clear_Data() {
     
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";
      
        //文字列化
        int score = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().score;

        //現在のハイスコアの取得
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);
        int high_Score = int.Parse(text.textWords[1, 1]);

        //ハイスコアの更新
        if (score > high_Score) {
            StreamWriter sw_Clear = new StreamWriter(filePath, false);
            sw_Clear.Write("#NAME\t#VALUE");
            sw_Clear.Write("\nHighScore\t" + score.ToString());

            sw_Clear.Flush();
            sw_Clear.Close();
        }

    }


    //ハイスコアの取得
    public int Get_High_Score() {

        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";

        //ファイル読み込み
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);
        int high_Score = int.Parse(text.textWords[1, 1]);

        return high_Score;
    }


    //データの消去
    public void Delete_Data() {
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";

        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#NAME\t#VALUE");
        sw_Clear.Write("\nHighScore" + 0.ToString("D10"));
        sw_Clear.Flush();
        sw_Clear.Close();
    }




}
