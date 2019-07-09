using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_BossScene : MonoBehaviour {

    //スクリプト
    private Stage3_BossMovie _movie;


	// Use this for initialization
	void Start () {
        //スクリプト
        _movie = GetComponent<Stage3_BossMovie>();

        //ムービー開始
        _movie.StartCoroutine("Before_Boss_Movie");
	}
	
	
}
