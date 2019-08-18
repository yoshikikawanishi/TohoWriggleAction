using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_BonusScene : MonoBehaviour {

    //スクリプト
    private GameManager game_Manager;

    //メッセージ番号
    private int start_ID = 1, end_ID = 1;


    // Use this for initialization
    void Start() {
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
        switch (has_Visied_Bonus()) {
            case 1: Set_Message_ID(1, 1); break;
            case 2: Set_Message_ID(1, 1); break;
            case 3: Set_Message_ID(1, 1); break;
            case 4: Set_Message_ID(1, 1); break;
        }
        _message.Start_Display("DoremyText", start_ID, end_ID);
        yield return new WaitUntil(_message.End_Message);
        //終了設定
        player.GetComponent<WriggleController>().Set_Playable(true);
    }


    //メッセージ番号設定
    private void Set_Message_ID(int s, int e) {
        start_ID = s;
        end_ID = e;
    }


    //2面ボーナス、3面ボーナスに訪れたかどうか
    private int has_Visied_Bonus() {
        if (game_Manager.Is_Visited("Stage2_BonusScene")) {
            if (game_Manager.Is_Visited("Stage3_BonusScene")) {
                return 1;   //2&3
            }
            else {
                return 2;   //2
            }
        }
        else {
            if (game_Manager.Is_Visited("Stage3_BonusScene")) {
                return 3;   //3
            }
            else {
                return 4;   //X
            }
        }
    }


}
