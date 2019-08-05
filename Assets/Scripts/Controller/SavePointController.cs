using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointController : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;
    private UIController ui_Controller;


	// Use this for initialization
	void Start () {
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        ui_Controller = GameObject.Find("Canvas").GetComponent<UIController>();
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機と接触したときセーブ
        if (collision.tag == "PlayerBodyTag") {
            _gameManager.SaveData();
            ui_Controller.Save_UI();
        }
    }
}
