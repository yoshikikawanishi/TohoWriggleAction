using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusEntranceController : MonoBehaviour {

    //スクリプト
    private BonusSceneManager _bonus_Scene_Manager;
    //キー、そのボーナスシーンに入ったことがあるかどうか
    [SerializeField] private string entrance_Key;
    //遷移先
    [SerializeField] private string bonus_Scene_Name;


    //Start
    private void Start() {
        //スクリプトの取得
        _bonus_Scene_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<BonusSceneManager>();   
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            //ボーナスシーンから出る
            if (SceneManager.GetActiveScene().name == bonus_Scene_Name) {
                _bonus_Scene_Manager.StartCoroutine("Exit_Bonus_Scene");
            }
            //ボーナスシーンに入る
            else if (PlayerPrefs.GetInt(entrance_Key) == 0) {
                _bonus_Scene_Manager.Enter_Bonus_Scene(bonus_Scene_Name);
                PlayerPrefs.SetInt(entrance_Key, 1);
            }
        }
    }
}
