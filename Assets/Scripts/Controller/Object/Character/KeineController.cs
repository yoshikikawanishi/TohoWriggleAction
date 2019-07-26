using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineController : MonoBehaviour {

    //イベント中キャッチされたかどうか
    private bool is_Catched = false;


	// Use this for initialization
	void Start () {
		
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            is_Catched = true;
        }
    }

    //イベント中キャッチされたかどうか
    public bool Get_Is_Catched() {
        return is_Catched;
    }
}
