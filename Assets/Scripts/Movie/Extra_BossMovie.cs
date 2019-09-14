using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extra_BossMovie : MonoBehaviour {

    //オブジェクト
    private GameObject player;
    private GameObject doremy;
    //スクリプト
    private MessageDisplay _message;
    private PauseManager _pause;

    private bool is_First_Visit = false;


	//Awake
	void Awake () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        doremy = GameObject.Find("Doremy");
        _message = GetComponent<MessageDisplay>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();        
	}


    //Start
    private void Start() {
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            is_First_Visit = true;
        }
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
        if (is_First_Visit) {
            _message.Start_Display("DoremyText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }

        //戦闘開始
        player.GetComponent<WriggleController>().Set_Playable(true);
        _pause.Set_Pausable(true);
        doremy.GetComponent<DoremyController>().start_Battle = true;
    }
}
