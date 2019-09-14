using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStage : MonoBehaviour {

    //スクリプト
    private Extra_BossMovie _movie;


	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Extra_BossMovie>();       

        //ムービー開始
        _movie.Start_Before_Movie();
	}
	
	
}
