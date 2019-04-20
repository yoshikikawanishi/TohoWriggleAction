﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Controller : MonoBehaviour {

    //自機
    private GameObject player;

    //SoulEnemyを生成するトリガーのx座標
    [SerializeField] private float soul_Gen_Line = 0;
    private bool start_Soul_Gen = false;


	// Use this for initialization
	void Start () {
        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");
	}
	
	// Update is called once per frame
	void Update () {
        //敵生成
        Enemy_Gen();
	}


    //敵の生成
    private void Enemy_Gen() {
        //自機が指定のX座標についた時生成開始
        if(player.transform.position.x > soul_Gen_Line && !start_Soul_Gen) {
            start_Soul_Gen = true;
            StartCoroutine("Enemy_Gen_Routine");
        }
    }
    private IEnumerator Enemy_Gen_Routine() {
        //SoulEnemyの生成
        for(int i = 0; i < 6; i++) {
            for (int j = 0; j < 4; j++) {
                var soul = Instantiate(Resources.Load("Enemy/SoulEnemy")) as GameObject;
                float noise = Random.Range(-18f, 18f);
                if (i % 2 == 0) {
                    soul.transform.position = new Vector3(player.transform.position.x + 216f, 132f - j * 64f + noise, 0);
                }
                else {
                    soul.transform.position = new Vector3(player.transform.position.x + 216f, -132f + j * 64f + noise, 0);
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(1.5f);
        //SunFlowerEnemyの生成
        var sun = Instantiate(Resources.Load("Enemy/SunFlowerEnemy")) as GameObject;
        sun.transform.position = new Vector3(player.transform.position.x + 200f, 216f, 0);
        yield return new WaitForSeconds(6.0f);
        GameObject[] suns = new GameObject[2];
        suns[0] = Instantiate(Resources.Load("Enemy/SunFlowerEnemy")) as GameObject;
        suns[1] = Instantiate(Resources.Load("Enemy/SunFlowerEnemy")) as GameObject;
        suns[0].transform.position = new Vector3(player.transform.position.x + 200f, 264f, 0);
        suns[1].transform.position = new Vector3(player.transform.position.x + 200f, 160f, 0);
    }
}