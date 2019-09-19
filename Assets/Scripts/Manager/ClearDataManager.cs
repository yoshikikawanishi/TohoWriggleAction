using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ClearDataManager  {
    

    //クリアデータの保存
    public static void Save_Clear_Data() {
     
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";
      
        //スコアの取得
        int score = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().score;

        //現在のハイスコアの取得
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);
        int high_Score = int.Parse(text.textWords[1, 1]);

        //ハイスコアの更新
        if (score > high_Score) {
            text.textWords[1, 1] = score.ToString();
            StreamWriter sw_Clear = new StreamWriter(filePath, false);
            sw_Clear.Write("#NAME\t#VALUE");
            for (int i = 1; i < text.rowLength; i++) {
                sw_Clear.Write("\n" + text.textWords[i, 0] + "\t" + text.textWords[i, 1]);
            }
            sw_Clear.Flush();
            sw_Clear.Close();
        }
    }


    //エクストラのクリアを保存
    public static void Save_Clear_Extra() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        //ファイル書き換え
        text.textWords[2, 1] = "true";
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#NAME\t#VALUE");
        for (int i = 1; i < text.rowLength; i++) {
            sw_Clear.Write("\n" + text.textWords[i, 0] + "\t" + text.textWords[i, 1]);
        }
        sw_Clear.Flush();
        sw_Clear.Close();
    }    


    //ハイスコアの取得
    public static int Get_High_Score() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";        
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        int high_Score = int.Parse(text.textWords[1, 1]);

        return high_Score;
    }


    //エクストラクリア状況を取得
    public static bool Is_Clear_Extra() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        if(text.textWords[2, 1] == "true") {
            return true;
        }
        return false;
    }


    //データの消去
    public static void Delete_Data() {
        string filePath = Application.dataPath + @"\StreamingAssets\ClearData.txt";

        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#NAME\t#VALUE");
        sw_Clear.Write("\nHighScore\t" + 0.ToString("D10"));
        sw_Clear.Write("\nClearExtra\tfalse");
        sw_Clear.Flush();
        sw_Clear.Close();
    }




}
