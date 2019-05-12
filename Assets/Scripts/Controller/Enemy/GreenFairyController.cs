﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFairyController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;
    private BulletFunctions _bulletFunc;
    //オーディオ
    private AudioSource shot_Sound;

    //時間
    private float time = 0;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        _bulletFunc = gameObject.AddComponent<BulletFunctions>();
        //オーディオ
        shot_Sound = GetComponents<AudioSource>()[1];

        //弾のセット
        GameObject bullet = Resources.Load("Bullet/GreenBullet") as GameObject;
        _bulletFunc.Set_Bullet(bullet);
    }
	
	// Update is called once per frame
	void Update () {
        if (_renderer.isVisible) {
            //移動
            _rigid.velocity = new Vector2(-10f, _rigid.velocity.y);
            //ショット
            if (time < 2.0f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                _bulletFunc.Turn_Shoot_Bullet(70f, 150f, 7.0f);
                shot_Sound.Play();
            }
        }
	}
}