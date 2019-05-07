﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour {

    //自機
    GameObject player;

    //ステージの左端
    [SerializeField] private float leftSide = 0f;
    //右端
    public float rightSide = 32f;

    //強制スクロールの範囲
    [SerializeField] private float scroll_Left_Side = 0;
    [SerializeField] private float scroll_Right_Side = 0;
    [SerializeField] private float scroll_Speed = 0;
    

    // Use this for initialization
    void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
    }


    //FixedUpdaet
    private void FixedUpdate() {
        if (player != null) {
            //強制スクロール
            if (scroll_Left_Side < transform.position.x && transform.position.x <= scroll_Right_Side) {
                transform.position += new Vector3(scroll_Speed, 0, 0);

            }        
            //自機追従
            else {
                transform.position = new Vector3(player.transform.position.x, 0, -10);
            }
        }
        //左端のときスクロールを止める
        if (transform.position.x < leftSide) {
            transform.position = new Vector3(leftSide, 0, -10);
        }
        //右端のときスクロールをとめる
        if (transform.position.x >= rightSide) {
            transform.position = new Vector3(rightSide, 0, -10);
        }
        
        
    }

}
