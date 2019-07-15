using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusEntranceController : MonoBehaviour {

    //スクリプト
    private BonusSceneManager _bonus_Scene_Manager;

    //遷移先
    [SerializeField] private string next_Scene_Name;
    [SerializeField] private Vector3 next_Pos;

    //2回目以降消滅する入り口かどうか
    [SerializeField] private bool is_Delete = true;

    //Start
    private void Start() {
        _bonus_Scene_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<BonusSceneManager>();
        if(PlayerPrefs.GetInt(next_Scene_Name) == 0 && is_Delete) {
            Destroy(gameObject);
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //シーン遷移
        if (collision.tag == "PlayerBodyTag") {
            _bonus_Scene_Manager.Change_Scene(next_Scene_Name, next_Pos);
        }
    }
}
