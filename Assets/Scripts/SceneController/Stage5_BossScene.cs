using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_BossScene : MonoBehaviour {

    //スクリプト
    private Stage5_BossMovie _movie;
    private GameManager game_Manager;


    // Use this for initialization
    void Start() {
        //取得
        _movie = GetComponent<Stage5_BossMovie>();
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //ムービー開始
        if (game_Manager.Is_First_Visit()) {
            _movie.StartCoroutine("Before_Movie_First");
        }
        else {
            _movie.StartCoroutine("Before_Movie_Second");
        }
    }
}
