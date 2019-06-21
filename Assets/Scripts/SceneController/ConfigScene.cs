using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ボタンの取得
        Button jump_Button = GameObject.Find("Jump/Submit").GetComponent<Button>();

        jump_Button.Select();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
