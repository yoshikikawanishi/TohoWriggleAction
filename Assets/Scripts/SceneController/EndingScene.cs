using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour {


    //スクリプト
    private Ending_Movie _movie;
    private BossEnemyController boss_Controller;


	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Ending_Movie>();
        boss_Controller = GameObject.Find("Reimu").GetComponent<BossEnemyController>();

        //クリアデータの保存
        Save_Clear_Data();

        //ムービー
        _movie.StartCoroutine("Start_Ending_Movie");
	}


    //Update
    private void Update() {
        //霊夢撃破後
        if (boss_Controller.Clear_Trigger()) {
            _movie.StartCoroutine("Play_Clear_Movie");
        }
    }

    //クリアデータの保存
    private void Save_Clear_Data() {
        ClearDataManager.Save_Clear_Data();
    }


    
}
