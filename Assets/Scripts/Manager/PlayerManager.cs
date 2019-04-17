using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;

    //ライフ
    public int life = 3;
    //スコア
    public int score = 0;
    //パワー
    public int power = 0;


	// Use this for initialization
	void Start () {
        //スクリプト
        _gameManager = GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //自機の復活の処理
    public IEnumerator Revive() {
        yield return new WaitForSeconds(1.0f);
        //セーブのロード
        _gameManager.StartCoroutine("LoadData");
        yield return null;
        life = 3;
        //点滅
        GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerCollisionController>().StartCoroutine("Blink");
    }


    //ライフアップ
    private void Life_Up() {
        if(life < 9) {
            life++;
        }
        
    }


    //点の獲得
    public void Get_Score() {
        score += 100;
    }


    //Pの獲得
    public void Get_Power() {
        if (power <= 128) {
            power++;
        }
    }

}
