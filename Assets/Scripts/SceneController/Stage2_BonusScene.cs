using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BonusScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //初回時ムービー
        if (gm.Is_First_Visit()) {
            StartCoroutine("Movie");
        }
        //2回目以降アイテムを消す
        else{
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
        _message.Start_Display("DoremyText", 1, 1);
        yield return new WaitUntil(_message.End_Message);
        //終了設定
        player.GetComponent<WriggleController>().Set_Playable(true);
    }

}
