using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    //自機
    GameObject player;

    //ステージの左端
    [SerializeField] private float leftSide = 0f;
    //右端
    [SerializeField] private float rightSide = 32f;

    //自動スクロールかどうか
    [SerializeField] private bool is_Auto_Scroll = false;
    [SerializeField] private float scroll_Speed = 0;
    

    // Use this for initialization
    void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
    }


    // Update is called once per frame
    void Update() {

        //通常の面
        if (!is_Auto_Scroll) {
            transform.position = new Vector3(player.transform.position.x, 0, -10);
        }
        //強制スクロールの面
        else {
            transform.position += new Vector3(scroll_Speed, 0, 0);
        }
        
        //左端のときスクロールを止める
        if (transform.position.x < leftSide) {
            transform.position = new Vector3(leftSide, 0, -10);
        }
        //右端のときスクロールを止める、敵の生成を辞める
        if (transform.position.x >= rightSide) {
            transform.position = new Vector3(rightSide, 0, -10);
        }
    }
}
