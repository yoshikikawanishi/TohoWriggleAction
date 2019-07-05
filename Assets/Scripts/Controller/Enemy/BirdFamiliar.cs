﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFamiliar : MonoBehaviour {

    //自機
    private GameObject player;
    private GameObject main_Camera;
    //行動開始
    private bool start_Action = false;


    // Use this for initialization
    void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.FindWithTag("MainCamera");
        
    }
	

    //FixedUpDate
    private void FixedUpdate() {
        //行動
        if (main_Camera.transform.position.x > transform.position.x - 240f && !start_Action) {
            start_Action = true;
            transform.SetParent(main_Camera.transform);
            StartCoroutine("Shot");
            Destroy(gameObject, 7.0f);
        }
        if (start_Action) {
            transform.localPosition += transform.right * 1.5f * Time.timeScale;
        }
    }


    //攻撃
    private IEnumerator Shot() {
        yield return new WaitForSeconds(3f / 7f);
        GameObject bullet = Resources.Load("Bullet/BirdFamiliarBullet") as GameObject;
        for (int i = 0; i < 15; i++) {
            GameObject b = Instantiate(bullet);
            b.transform.position = transform.position;
            UsualSoundManager.Small_Shot_Sound();
            b.transform.eulerAngles = transform.eulerAngles;
            b.transform.SetParent(main_Camera.transform);
            yield return new WaitForSeconds(3f / 7f);
        }
    }
}