using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BonusScene : MonoBehaviour {

    private GameManager game_Manager;


	// Use this for initialization
	void Start () {
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //初回時ムービー
        if (game_Manager.Is_First_Visit()) {
            StartCoroutine("Movie");
        }
        //2回目以降アイテムを消す
        else {
            Destroy(GameObject.Find("Items"));
        }
    }


    //ムービー
    private IEnumerator Movie() {
        //初期設定
        GameObject player = GameObject.FindWithTag("PlayerTag");
        MessageDisplay _message = GetComponent<MessageDisplay>();
        player.GetComponent<WriggleController>().Set_Playable(false);
        //メッセージ表示
        if (game_Manager.Is_Visited("Stage2_BonusScene")) {
            _message.Start_Display("DoremyText", 1, 1);
        }
        else {
            _message.Start_Display("DoremyText", 2, 2);
        }
        yield return new WaitUntil(_message.End_Message);
        //終了設定
        player.GetComponent<WriggleController>().Set_Playable(true);
    }


}
