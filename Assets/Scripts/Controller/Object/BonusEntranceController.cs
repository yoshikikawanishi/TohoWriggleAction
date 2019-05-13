using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusEntranceController : MonoBehaviour {

    //スクリプト
    private BonusSceneManager _bonus_Scene_Manager;
    //入ったことがあるかどうか
    [SerializeField] private string entrance_Key;
    

    //Start
    private void Start() {
        //スクリプトの取得
        _bonus_Scene_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<BonusSceneManager>();   
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            //ボーナスシーンに入る
            if (SceneManager.GetActiveScene().name != "BonusScene" && PlayerPrefs.GetInt(entrance_Key) == 0) {
                _bonus_Scene_Manager.Enter_Bonus_Scene();
                PlayerPrefs.SetInt(entrance_Key, 1);
            }
            //ボーナスシーンから出る
            else if(SceneManager.GetActiveScene().name == "BonusScene"){
                _bonus_Scene_Manager.StartCoroutine("Exit_Bonus_Scene");
            }
        }
    }
}
