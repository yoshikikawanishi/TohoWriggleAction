using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    //フィールド
    private TextFileReader _text = new TextFileReader();
    private int start_ID = 1;
    private int end_ID = 1;
    private GameObject parent;

    private bool end_Generate;


    //敵生成開始
    public void Start_Enemy_Gen(string fileName, int start_ID, int end_ID, GameObject parent) {
        end_Generate = false;
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        this.parent = parent;
        _text.Read_Text(fileName);
        if(start_ID == 0 || end_ID == 0) {
            this.start_ID = 1;
            this.end_ID = _text.rowLength - 1;
        }
        StartCoroutine("Enemy_Gen");
    }


    /*  ファイルの形式
       
        #ID #NAME   #Pos_X  #Pos_Y  #Direction  #IntervalTime
        1   EnemyName   0   0   1   1.0       
    */


    //敵生成コルーチン
    private IEnumerator Enemy_Gen() {
        for(int i = start_ID; i <= end_ID; i++) {
            yield return new WaitForSeconds(float.Parse(_text.textWords[i, 5]));
            GameObject enemy = Instantiate(Resources.Load("Enemy/" + _text.textWords[i, 1])) as GameObject;
            enemy.transform.SetParent(parent.transform);
            enemy.transform.localPosition = new Vector3(int.Parse(_text.textWords[i, 2]), int.Parse(_text.textWords[i, 3]));
            enemy.transform.position -= new Vector3(0, 0, enemy.transform.position.z);
            enemy.transform.localScale = new Vector3(int.Parse(_text.textWords[i, 4]), 1, 1);
        }
        end_Generate = true;
    }


    //敵生成停止用
    public void Stop_Enemy_Gen() {
        StopCoroutine("Enemy_Gen");
    }


    //生成終了検知用
    public bool End_Generate() {
        if (end_Generate) {
            end_Generate = false;
            return true;
        }
        return false;
    }


}
