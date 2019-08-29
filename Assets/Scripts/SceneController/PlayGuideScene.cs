using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerManager pm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        pm.life = 3;
        pm.stock = 3;
	}
	
	
}
