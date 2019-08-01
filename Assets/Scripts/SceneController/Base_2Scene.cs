using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base_2Scene : MonoBehaviour {

    //スクリプト
    private Base_2Movie _movie;
    //自機
    private GameObject player;

    //右端
    [SerializeField] private float right_Side;


    // Use this for initialization
    void Start() {
        //スクリプト
        _movie = GetComponent<Base_2Movie>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //受け止めろ！イベント
        _movie.StartCoroutine("Catch_Event");
        
    }

    //Update
    private void Update() {
        //シーン遷移
        if(player.transform.position.x > right_Side) {
            SceneManager.LoadScene("Stage4_1Scene");
        }
    }

}
