using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base_2Scene : MonoBehaviour {

    //スクリプト
    private Base_2Movie _movie;
    //自機
    private GameObject player;
    //カメラ
    private CameraController main_Camera_Controller;

    //右端
    [SerializeField] private float right_Side;


    // Use this for initialization
    void Start() {
        //スクリプト
        _movie = GetComponent<Base_2Movie>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //カメラ
        main_Camera_Controller = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
        //受け止めろ！イベント
        _movie.StartCoroutine("Catch_Event");
        
    }

    //Update
    private void Update() {
        if (_movie.is_End_Movie) {
            //シーン遷移
            if (player.transform.position.x > right_Side) {
                StartCoroutine("Change_Scene");
            }

            //休憩エリア
            if (player.transform.position.x < -400f && main_Camera_Controller.enabled != false) {
                main_Camera_Controller.enabled = false;
                GameObject.FindWithTag("MainCamera").transform.position = new Vector3(-704f, 0, -10f);
            }
            else if (player.transform.position.x > -400f && main_Camera_Controller.enabled == false) {
                main_Camera_Controller.enabled = true;
            }
        }
    }

    private IEnumerator Change_Scene() {
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Stage4_1Scene");
    }

}
