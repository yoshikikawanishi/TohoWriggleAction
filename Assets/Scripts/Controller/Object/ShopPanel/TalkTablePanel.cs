using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTablePanel : MonoBehaviour {

    private UnderGroundShoppingManager shop_Manager;

	// Use this for initialization
	void Start () {
        shop_Manager = GameObject.Find("Scripts").GetComponent<UnderGroundShoppingManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
