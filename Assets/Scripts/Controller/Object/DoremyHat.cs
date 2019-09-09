using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DoremyHat : MonoBehaviour {

    [SerializeField] private string scene_Name;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            Put_Effect();
            Save_Getting();
            Destroy(gameObject);
        }
    }


    //エフェクト
    private void Put_Effect() {
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.SetParent(null);
        child.GetComponent<AudioSource>().Play();
        child.GetComponent<ParticleSystem>().Play();
    }


    //記録
    private void Save_Getting() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\DoremyHat.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        //ファイルの書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#SceneName" + "\t" + "#HasGet");
        sw_Clear.Flush();
        sw_Clear.Close();

        StreamWriter sw = new StreamWriter(filePath, true);
        for (int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            if(text.textWords[i, 0] == scene_Name) {
                sw.Write(scene_Name + "\t" + "true");
            }
            else {
                sw.Write(text.textWords[i, 0] + "\t" + text.textWords[i, 1]);
            }
        }      
        sw.Flush();
        sw.Close();
    }


    //データの消去
    static public void Delete_Data() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\DoremyHat.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        //ファイルの書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write("#SceneName" + "\t" + "#HasGet");
        sw_Clear.Flush();
        sw_Clear.Close();

        StreamWriter sw = new StreamWriter(filePath, true);
        for (int i = 1; i < text.rowLength; i++) {
            sw.Write("\n");
            sw.Write(text.textWords[i, 0] + "\t" + "false");
        }
        sw.Flush();
        sw.Close();
    }
}
