﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaBird : MonoBehaviour {

    public bool is_Phase1 = true;

    //方向
    public bool is_Right_Direction = true;

    //移動のスタートと終わり位置
    private Vector3 start_Pos;
    private Vector3 end_Pos;

    //ショット
    private float time = 0;
    private float span;


    // Use this for initialization
    void Start() {
        start_Pos = GameObject.Find("Mystia").transform.position;
        if (is_Phase1) {
            //左向き
            if (!is_Right_Direction) {
                transform.localScale = new Vector3(-1, 1, 1);
                end_Pos = start_Pos + new Vector3(-200f, 0);
            }
            else {
                end_Pos = start_Pos + new Vector3(200f, 0);
            }
            //移動
            StartCoroutine("Move");
            span = 3f / 7f;
        }
        else {
            span = 12f / 7f;
        }
        time = span;
    }
	

	// Update is called once per frame
	void Update () {
        //ショット
        if(time < span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            GameObject bullet = Instantiate(Resources.Load("Bullet/MystiaScalesBullets") as GameObject);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            if (is_Phase1) {
                bullet.transform.Rotate(new Vector3(0, 0, 180f));
            }
            Destroy(bullet, 15f);
            UsualSoundManager.Small_Shot_Sound();
        }
	}


    //移動
    private IEnumerator Move() {
        float now_Location = 0;
        float slerp_Height;
        if (is_Right_Direction) {
            slerp_Height = -64f;
        }
        else {
            slerp_Height = 64f;
        }
        while (now_Location <= 1) {
            now_Location += 0.01f * Time.timeScale;
            transform.position = Vector3.Lerp(start_Pos, end_Pos, now_Location);
            transform.position += new Vector3(0, slerp_Height * Mathf.Sin(now_Location * Mathf.PI), 0);
            transform.Rotate(0, 0, 0.4f);
            yield return new WaitForSeconds(0.016f);
        }
        if (is_Right_Direction) {
            while (transform.position.y < 180f) {
                transform.position += new Vector3(1.5f, 1.5f);
                yield return new WaitForSeconds(0.016f);
            }
        }
        else {
            while (transform.position.y > -180f) {
                transform.position += new Vector3(-1.5f, -1.5f);
                yield return new WaitForSeconds(0.016f);
            }
        }
    }
}
