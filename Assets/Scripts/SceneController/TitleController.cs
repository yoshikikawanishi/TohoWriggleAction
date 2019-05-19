using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //ボタンの初期位置
        Button continue_Button = GameObject.Find("ContinueButton").GetComponent<Button>();
        continue_Button.Select();
	}
	
}
