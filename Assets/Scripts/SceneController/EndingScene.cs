using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour {


    //スクリプト
    private Ending_Movie _movie;

	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Ending_Movie>();

        //クリアデータの保存
        Save_Clear_Data();

        //ムービー
        _movie.StartCoroutine("Start_Ending_Movie");
	}
	
	
    //クリアデータの保存
    private void Save_Clear_Data() {
        ClearDataManager clear_Data_Manager = new ClearDataManager();
        clear_Data_Manager.Save_Clear_Data();
    }


    
}
