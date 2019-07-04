using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_2Scene : MonoBehaviour {

    //自機、カメラ
    private GameObject player;
    private WriggleController player_Controller;
    private GameObject main_Camera;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        main_Camera = GameObject.FindWithTag("MainCamera");
	}

    // Update is called once per frame
    void Update() {
        //スクロール時の時期の動き
        if(main_Camera.transform.position.x > 5800f && player != null) {
            if (player_Controller.Get_Is_Fly() && player.transform.parent != main_Camera.transform) {
                player.transform.SetParent(main_Camera.transform);
            }
            else if (!player_Controller.Get_Is_Fly() && player.transform.parent != null) {
                player.transform.SetParent(null);
            }
        }
    }
}
