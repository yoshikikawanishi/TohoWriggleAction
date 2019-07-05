using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class KeyConfig  {
    
    public void Create_InputManager() {
        InputManagerGenerator inputManagerGenerator = new InputManagerGenerator();
        inputManagerGenerator.Clear();
        //Horizontal, Vertical
        {
            var name = "Horizontal";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 1, false));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "left", "right", "left", "right"));
        }
        {
            var name = "Vertical";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 2, true));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "down", "up", "down", "up"));
        }

        //テキストファイルから読み込む
        string filePath = Application.dataPath + @"\KeyConfig.csv";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);
        //ジャンプ、決定
        {
            var name = "Jump/Submit";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[1, 1], text.textWords[1, 2]));
            name = "Submit";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[1, 1], text.textWords[1, 2]));
        }
        //ショット、戻る
        {
            var name = "Shot/Cancel";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[2, 1], text.textWords[2, 2]));
            name = "Cancel";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[2, 1], text.textWords[2, 2]));
        }
        //飛行
        {
            var name = "Fly";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[3, 1], text.textWords[3, 2]));
        }
        //ポーズ
        {
            var name = "Pause";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, text.textWords[4, 1], text.textWords[4, 2]));
        }
    }


    //コンフィグの変更
    public void Change_Button(string change_Axis, string button, bool is_Pad) {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\KeyConfig.csv";
        TextReader text = new TextReader();
        text.Read_Text_File(filePath);

        int colum = 1;
        int row = 1;
        //キーボード
        if (!is_Pad) {
            colum = 1;
        }
        //ゲームパッド
        else {
            colum = 2;
        }
        //何行目か
        for (int i = 1; i < text.rowLength; i++) {
            if (change_Axis == text.textWords[i, 0]) {
                row = i;
                break;
            }
        }
        //ファイル書き換え
        string before_Button = text.textWords[row, colum];
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#NAME\t#KEYCODE\t#BUTTON");
        sw_Clear.Flush();
        sw_Clear.Close();
        StreamWriter sw = new StreamWriter(filePath, true);
        for (int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            for (int j = 0; j < text.columnLength; j++) {
                if (i == row && j == colum) {
                    sw.Write(button + "\t");
                }
                else if(text.textWords[i, j] == button) {
                    sw.Write(before_Button + "\t");
                }
                else {
                    sw.Write(text.textWords[i, j] + "\t");
                }
            }
        }
        sw.Flush();
        sw.Close();
    }

}
