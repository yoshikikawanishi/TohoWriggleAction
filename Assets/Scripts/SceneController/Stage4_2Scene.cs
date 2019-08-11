using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_2Scene : MonoBehaviour {

    //スクリプト
    private EnemyGenerator _enemy_Gen;

    //カメラ、自機
    private GameObject main_Camera;
    private GameObject player;
    private WriggleController player_Controller;
    
    //Awake
    private void Awake() {
        //取得
        _enemy_Gen = GetComponent<EnemyGenerator>();
    }


    // Use this for initialization
    void Start () {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        //敵生成
        _enemy_Gen.Start_Enemy_Gen("Stage4_2_Enemy_Gen", 1, 13, main_Camera);
    }


    //update
    private void Update() {
        //マスパテスト
        if(main_Camera.transform.position.x >= 2000 && main_Camera.transform.position.x < 2010) {
            Debug.Log(Time.time);
        }
        //スクロール時の自機の動き
        if (player_Controller.Get_Is_Fly() && player.transform.parent != main_Camera.transform) {
            player.transform.SetParent(main_Camera.transform);
        }
        else if (!player_Controller.Get_Is_Fly() && player.transform.parent != null) {
            player.transform.SetParent(null);
        }
    }


}
