using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3_1Scene : MonoBehaviour {

    //自機
    private GameObject player;


	// Use this for initialization
	void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
	}
	
	// Update is called once per frame
	void Update () {
		if(player.transform.position.x > 8816f) {
            SceneManager.LoadScene("Stage3_2Scene");
        }
	}
}
