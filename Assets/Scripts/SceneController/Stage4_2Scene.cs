using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage4_2Scene : MonoBehaviour {

    //スクリプト
    private EnemyGenerator _enemy_Gen;

    //カメラ、自機
    private GameObject main_Camera;
    private GameObject player;
    private WriggleController player_Controller;

    //敵生成開始
    private bool start_Enemy_Gen = false;

    
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
    }


    //update
    private void Update() {
        //敵生成
        Enemy_Gen();
        //スクロール時の自機の動き
        Set_Player_Parent();
        
        //シーン遷移
        if (player.transform.position.x > 6232f) {
            SceneManager.LoadScene("Stage4_BossScene");
        }      
        
    }


    //敵生成
    private void Enemy_Gen() {
        if (main_Camera.transform.position.x > 0 && !start_Enemy_Gen) {
            start_Enemy_Gen = true;
            _enemy_Gen.Start_Enemy_Gen("Stage4_2_Enemy_Gen", 0, 0, main_Camera);
        }
        //右端で敵生成終了
        if (main_Camera.transform.position.x >= 5999f) {
            _enemy_Gen.Stop_Enemy_Gen();
        }
    }


    //スクロール時の時期の動き
    private void Set_Player_Parent() {
        if (player_Controller.Get_Is_Fly() && player.transform.parent != main_Camera.transform) {
            player.transform.SetParent(main_Camera.transform);
        }
        else if (!player_Controller.Get_Is_Fly() && player.transform.parent != null) {
            player.transform.SetParent(null);
        }
    }


}
