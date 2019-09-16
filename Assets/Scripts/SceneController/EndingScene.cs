using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour {


    //スクリプト
    private MessageDisplay _message;


	// Use this for initialization
	void Start () {
        //取得
        _message = GetComponent<MessageDisplay>();

        //クリアデータの保存
        Save_Clear_Data();

        //ムービー
        StartCoroutine("Ending_Movie");
	}
	
	
    //クリアデータの保存
    private void Save_Clear_Data() {
        ClearDataManager clear_Data_Manager = new ClearDataManager();
        clear_Data_Manager.Save_Clear_Data();
    }


    //エンディングムービー
    private IEnumerator Ending_Movie() {
        GetComponent<FadeInOut>().Start_Fade_In();
        yield return new WaitForSeconds(2.0f);
        _message.Start_Display("EndingText", 1, 1);
        yield return new WaitUntil(_message.End_Message);
        
        //スタッフロール？

        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
