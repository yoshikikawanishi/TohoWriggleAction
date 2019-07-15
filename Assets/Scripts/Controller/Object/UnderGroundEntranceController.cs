using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnderGroundEntranceController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerBodyTag") {
            SceneManager.LoadScene("UnderGroundScene");
        }
    }
}
