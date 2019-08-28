using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_BonusScene : MonoBehaviour {

    //スクリプト
    private GameManager game_Manager;

    // Use this for initialization
    void Start() {
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //初回以外アイテム消す
        if (!game_Manager.Is_First_Visit()) {
            Destroy(GameObject.Find("Items"));
        }
    }


    


}
