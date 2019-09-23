using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Extra_BossMovie : MonoBehaviour {

    //オブジェクト
    private GameObject player;
    private GameObject doremy;
    //スクリプト
    private MessageDisplay _message;
    private PauseManager _pause;
    private BGMManager bgm_Manager;


	//Awake
	void Awake () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        doremy = GameObject.Find("Doremy");
        _message = GetComponent<MessageDisplay>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        bgm_Manager = GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>();
	}


    //ボス前ムービー
    public void Start_Before_Movie() {
        StartCoroutine("Play_Before_Movie");
    }

    private IEnumerator Play_Before_Movie() {
        //初期設定
        player.GetComponent<WriggleController>().Set_Playable(false);
        _pause.Set_Pausable(false);

        yield return new WaitForSeconds(0.5f);
        
        //ドレミー登場
        MoveBetweenTwoPoints doremy_Move = doremy.GetComponent<MoveBetweenTwoPoints>();
        doremy_Move.Start_Move(new Vector3(140f, 48f), -48f, 0.01f);
        yield return new WaitUntil(doremy_Move.End_Move);

        yield return new WaitForSeconds(0.5f);

        //会話
        _message.Start_Display("DoremyText", 1, 1);
        yield return new WaitUntil(_message.End_Message);
        

        //戦闘開始
        player.GetComponent<WriggleController>().Set_Playable(true);
        _pause.Set_Pausable(true);
        bgm_Manager.Change_BGM_Index(9);
        doremy.GetComponent<DoremyController>().start_Battle = true;
    }

    
    //クリア後ムービー
    public void Start_Clear_Movie() {
        StartCoroutine("Play_Clear_Movie");
    }

    private IEnumerator Play_Clear_Movie() {
        yield return new WaitForSeconds(1.5f);
        ClearDataManager.Save_Clear_Extra();
        GetComponent<FadeInOut>().Start_Fade_Out();
        bgm_Manager.Start_Fade_Out(0.01f, 2.1f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
