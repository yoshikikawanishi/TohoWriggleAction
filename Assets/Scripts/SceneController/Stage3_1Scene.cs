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

        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            GetComponent<FadeInOut>().Start_Fade_In();
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(player.transform.position.x > 8816f) {
            SceneManager.LoadScene("Stage3_2Scene");
        }
	}
}
