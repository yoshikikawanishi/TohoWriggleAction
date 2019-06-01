using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossController : MonoBehaviour {

    //スクリプト
    private Stage2_Boss_Movie _movie;


    // Use this for initialization
    void Start () {
        //スクリプトの取得
        _movie = GetComponent<Stage2_Boss_Movie>();

        //ボス前ムービー開始
        _movie.StartCoroutine("Before_Movie");
    }
	
	// Update is called once per frame
	void Update () {      
        
	}  

}
