using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_2Scene : MonoBehaviour {

    //スクリプト
    private GameManager game_Manager;
    private Base_2Movie _movie;


    // Use this for initialization
    void Start() {
        //スクリプト
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        _movie = GetComponent<Base_2Movie>();
        //受け止めろ！イベント
        if (game_Manager.Is_First_Visit()) {
            _movie.StartCoroutine("Catch_Event");
        }
    }
	
	
}
