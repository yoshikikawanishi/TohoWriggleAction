using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BonusScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //2回目以降アイテムを消す
        if (!GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>().Is_First_Visit()) {
            Destroy(GameObject.Find("Items"));
        }
	}

}
