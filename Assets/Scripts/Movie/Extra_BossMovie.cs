using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extra_BossMovie : MonoBehaviour {

    //オブジェクト
    private GameObject player;
    private GameObject doremy;
    //スクリプト
    private MessageDisplay _message;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        doremy = GameObject.Find("Doremy");
        _message = GetComponent<MessageDisplay>();
	}
	
	
    //ボス前ムービー
    public void Start_Before_Movie() {
        StartCoroutine("Play_Before_Movie");
    }

    private IEnumerator Play_Before_Movie() {
        yield return null;
    }
}
