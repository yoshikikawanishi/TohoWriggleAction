using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeEntrance : MonoBehaviour {


    //フィールド
    [SerializeField] private string next_Scene;
    [SerializeField] private Vector2 player_Pos;


    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" && Input.GetKeyDown(KeyCode.DownArrow)) {
            Change_Scene();
        }
    }

    //シーン遷移
    private void Change_Scene() {
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        gm.StartCoroutine(gm.Load_Scene(next_Scene, player_Pos));
    }


}
