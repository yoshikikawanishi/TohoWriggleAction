using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraFrontScene : MonoBehaviour {

    //スクリプト
    private PlayerManager player_Manager;

    //自機
    private GameObject player;

    private bool is_Loaded_Scene = false;
   

    // Use this for initialization
    void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        player_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();

        //初期設定
        player_Manager.life = 9;
        player_Manager.stock = 1;
        player_Manager.power = 128;
        player_Manager.score = 3;
    }
	
	// Update is called once per frame
	void Update () {
	    //シーン遷移
        if(player.transform.position.x > 230f && !is_Loaded_Scene) {
            is_Loaded_Scene = true;
            SceneManager.LoadScene("ExtraScene");
        }
	}
}
