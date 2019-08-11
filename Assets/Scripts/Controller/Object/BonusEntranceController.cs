using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusEntranceController : MonoBehaviour {

    //遷移先
    [SerializeField] private string next_Scene_Name;
    [SerializeField] private Vector3 next_Pos;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //シーン遷移
        if (collision.tag == "PlayerBodyTag") {
            GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
            gm.StartCoroutine(gm.Load_Scene(next_Scene_Name, next_Pos));
        }
    }

}
