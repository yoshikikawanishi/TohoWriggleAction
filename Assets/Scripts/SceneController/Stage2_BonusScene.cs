using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BonusScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        //初回以外アイテム消す
        if (!gm.Is_First_Visit()) {
            Destroy(GameObject.Find("Items"));
        }
	}


}
