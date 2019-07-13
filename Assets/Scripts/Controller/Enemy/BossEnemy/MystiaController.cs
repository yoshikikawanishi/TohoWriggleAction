﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private ObjectPool middle_Green_Pool;
    //スクリプト
    private BossEnemyController boss_Controller;
    private MoveBetweenTwoPoints _move;
    //自機
    private GameObject player;
    //バックデザイン
    private GameObject back_Design;

    //戦闘開始
    public bool start_Battle = false;

    //フェーズ毎開始用
    private bool start_Phase1_Routine = true;
    private bool start_Phase2_Routine = true;
    private bool start_Phase3_Routine = true;

    //鳥目用
    private GameObject bird_Eye_Mask;


	// Use this for initialization
	void Awake () {
        //コンポーネントの取得
        _anim = GetComponent<Animator>();
        //スクリプトの取得
        boss_Controller = GetComponent<BossEnemyController>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //バックデザイン
        back_Design = GameObject.Find("MystiaBackDesigns");
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        back_Design.SetActive(false);
        //鳥目用
        bird_Eye_Mask = GameObject.Find("BirdEyeMask");
        //オブジェクトプール
        middle_Green_Pool = gameObject.AddComponent<ObjectPool>();
        middle_Green_Pool.CreatePool(Resources.Load("Bullet/PooledBullet/MiddleGreenBulletPool") as GameObject, 30);
    }
	

	// Update is called once per frame
	void Update () {
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: Phase1(); break;
                case 2: Phase2(); break;
                case 3: Phase3(); break;
            }
        }
	}


    //アニメーション
    public void Change_Parameter(string change_Bool) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("SingBool", false);

        _anim.SetBool(change_Bool, true);
    }


    //フェーズ1
    private void Phase1() {
        if (start_Phase1_Routine) {
            start_Phase1_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }

    //フェーズ1コルーチン
    private IEnumerator Phase1_Routine() {
        back_Design.SetActive(true);
        while (true) {
            _move.Start_Move(new Vector3(200f, -108f), 0, 0.016f);
            yield return new WaitUntil(_move.End_Move);
            //斜め上に上がりながら鳥出す
            _move.Start_Move(transform.position + new Vector3(16f, -16f), 0, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            Change_Parameter("DashBool");
            _move.Start_Move(new Vector3(-260f, 160f), 0, 0.005f);
            while(transform.position.y < -100f) { yield return null; }
            Bird_Gen(false);
            while(transform.position.y < 100f) { yield return null; }
            Bird_Gen(true);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(1.0f);
            //後ろのほうを横切る
            transform.position = new Vector3(-250f, 90f);
            transform.localScale = new Vector3(-1, 1, 1);
            while(transform.position.x < 260f) {
                transform.position += new Vector3(3f, 0) * Time.timeScale;
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
            //突進
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(260f, player.transform.position.y + 64f);
            _move.Start_Move(new Vector3(-260f, transform.position.y), -80f, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(1.0f);
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position = new Vector3(-260f, player.transform.position.y + 64f);
            _move.Start_Move(new Vector3(260f, transform.position.y), -80f, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            //移動
            transform.localScale = new Vector3(1, 1, 1);
            Change_Parameter("IdleBool");
            _move.Start_Move(new Vector3(200f, -32f), 0, 0.016f);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(3.0f);
        }
    }
    //鳥の生成
    private void Bird_Gen(bool is_Right) {
        GameObject bird = Instantiate(Resources.Load("Enemy/MystiaBird") as GameObject);
        bird.transform.position = transform.position;
        bird.GetComponent<MystiaBird>().is_Right_Direction = is_Right;
        Destroy(bird, 8f);
    }



    //フェーズ2
    private void Phase2() {
        if (start_Phase2_Routine) {
            start_Phase2_Routine = false;
            StopCoroutine("Phase1_Routine");
            _move.StopAllCoroutines();
            back_Design.SetActive(false);
            StartCoroutine("Phase2_Routine");
        }
    }

    //フェーズ2コルーチン
    private IEnumerator Phase2_Routine() {
        //初期設定
        BulletFunctions _bullet = gameObject.AddComponent<BulletFunctions>();
        SpiralBulletFunction _spiral_Bullet = gameObject.AddComponent<SpiralBulletFunction>();
        _spiral_Bullet.Set_Bullet_Pool(middle_Green_Pool);
        transform.localScale = new Vector3(1, 1, 1);
        //初期位置に移動
        _move.Start_Move(new Vector3(150f, 0), 32f, 0.01f);
        yield return new WaitUntil(_move.End_Move);
        Change_Parameter("SingBool");
        back_Design.SetActive(true);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        while (true) {
            //鳥目にする
            StartCoroutine("Bird_Eye", false);
            yield return new WaitForSeconds(1.0f);
            //使い魔発射
            GameObject familiar = Resources.Load("Enemy/MystiaBird2") as GameObject;
            _bullet.Set_Bullet(familiar);
            _bullet.Odd_Num_Bullet(12, 30f, 70f, 10.0f);
            yield return new WaitForSeconds(1.5f);
            //弾発射
            float inter_Angle = 30f, span = 0.1f;
            _spiral_Bullet.Start_Spiral_Bullet(70f, 0, inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(1.2f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            _spiral_Bullet.Start_Spiral_Bullet(70f, 0, -inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(1.2f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            yield return new WaitForSeconds(10.0f);
            //鳥目解除
            StartCoroutine("Bird_Eye", true);
            yield return new WaitForSeconds(5.0f);
        }
    }


    //自機を鳥目にする、鳥目から回復する
    private IEnumerator Bird_Eye(bool is_Revive) {
        if (!is_Revive) {
            while (bird_Eye_Mask.transform.localScale.x > 3.0f) {
                bird_Eye_Mask.transform.localScale += new Vector3(-0.03f, -0.03f) * Time.timeScale;
                yield return null;
            }
        }
        else {
            while (bird_Eye_Mask.transform.localScale.x < 20f) {
                bird_Eye_Mask.transform.localScale += new Vector3(0.1f, 0.1f) * Time.timeScale;
                yield return null;
            }
        }
    }


    //フェーズ3
    private void Phase3() {
        if (start_Phase3_Routine) {
            start_Phase3_Routine = false;
            StopCoroutine("Phase2_Routine");
            _move.StopAllCoroutines();
            StartCoroutine(Bird_Eye(true));
            StartCoroutine("Phase3_Routine");
        }
    }

    //フェーズ3コルーチン
    private IEnumerator Phase3_Routine() {
        Change_Parameter("IdleBool");
        yield return new WaitForSeconds(1.5f);
        //初期設定
        BulletFunctions _bullet = gameObject.GetComponent<BulletFunctions>();
        SpiralBulletFunction _spiral_Bullet = gameObject.GetComponent<SpiralBulletFunction>();
        _spiral_Bullet.Set_Bullet_Pool(middle_Green_Pool);
        Change_Parameter("SingBool");
        while (true) {
            //鳥目にする
            StartCoroutine("Bird_Eye", false);
            yield return new WaitForSeconds(1.0f);
            //使い魔発射
            GameObject familiar = Resources.Load("Enemy/MystiaBird2") as GameObject;
            _bullet.Set_Bullet(familiar);
            _bullet.Odd_Num_Bullet(12, 30f, 50f, 10.0f);
            yield return new WaitForSeconds(1.5f);
            //弾発射
            float inter_Angle = 20f, span = 0.05f;
            _spiral_Bullet.Start_Spiral_Bullet(60f, 0, inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(13f * 0.05f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            _spiral_Bullet.Start_Spiral_Bullet(60f, 10, -inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(13f * 0.05f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            _spiral_Bullet.Start_Spiral_Bullet(60f, 20, inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(13f * 0.05f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            _spiral_Bullet.Start_Spiral_Bullet(60f, 30, -inter_Angle, span, 7.0f);
            yield return new WaitForSeconds(13f * 0.05f);
            _spiral_Bullet.Stop_Spiral_Bullet();
            yield return new WaitForSeconds(10.0f);
            //鳥目解除
            StartCoroutine("Bird_Eye", true);
            yield return new WaitForSeconds(1.0f);
        }
    }

}
