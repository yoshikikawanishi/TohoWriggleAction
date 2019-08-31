using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_BossScene : MonoBehaviour {

    //スクリプト
    private Stage5_BossMovie _movie;


	// Use this for initialization
	void Start () {
        //取得
        _movie = GetComponent<Stage5_BossMovie>();

        //ムービー開始
        _movie.StartCoroutine("Before_Movie_First");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
