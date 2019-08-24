using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_BossScene : MonoBehaviour {



	// Use this for initialization
	void Start () {
        //ムービー開始
        GetComponent<Stage4_BossMovie>().StartCoroutine("Before_Boss_Movie");
	}



	
}
