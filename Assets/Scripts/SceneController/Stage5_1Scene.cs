using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage5_1Scene : MonoBehaviour {

    //ステージの右端
    [SerializeField] private float right_Side = 5000f;

    //自機
    private GameObject player;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
	}
	

	// Update is called once per frame
	void Update () {
	    //シーン遷移
        if(player.transform.position.x > right_Side + 230f) {
            SceneManager.LoadScene("Stage5_BossScene");
        }
	}
}
