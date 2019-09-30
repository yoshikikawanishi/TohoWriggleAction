﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingReimuAttaack : MonoBehaviour {

    [SerializeField] private GameObject yin_Ball_Bullet;
    [SerializeField] private GameObject homing_Bullet;

    private ObjectPoolManager pool_Manager;
    private MoveBetweenTwoPoints _move;

    private BulletFunctions     homing_Shoot_Function;
    private BulletFunctions     yin_Ball_Shoot_Function;
    private BulletPoolFunctions talisman_Shoot_Function;

    private GameObject player;

    private bool start_Attack_Trigger = true;


    //Start
    private void Start() {
        //取得
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        player = GameObject.FindWithTag("PlayerTag");
        //オブジェクトプール
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/RedTalismanBullet") as GameObject, 30);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/WhiteTalismanBullet") as GameObject, 30);
        //アタッチ
        homing_Shoot_Function = gameObject.AddComponent<BulletFunctions>();
        yin_Ball_Shoot_Function = gameObject.AddComponent<BulletFunctions>();
        talisman_Shoot_Function = gameObject.AddComponent<BulletPoolFunctions>();
    }


    //攻撃用
    public void Do_Reimu_Attack() {
        if (start_Attack_Trigger) {
            start_Attack_Trigger = false;
            StartCoroutine("Play_Reimu_Attack_Routine");
        }
    }


    //攻撃用コルーチン
    private IEnumerator Play_Reimu_Attack_Routine() {
        yield return null;
        //耐久開始
        StartCoroutine("Down_Boss_Life");
        //攻撃
        int count = 0;
        while (true) {
            //4種の攻撃からランダムの順に
            List<int> list = new List<int> { 1, 2, 3, 4 };
            while(list.Count > 0) {
                int num = Random.Range(0, list.Count);
                switch (list[num]) {
                    case 1://ホーミング弾
                        StartCoroutine(Shoot_Homing_Bullet(count + 4));
                        yield return new WaitForSeconds(2.25f);
                        break;
                    case 2://陰陽玉弾
                        StartCoroutine(Shoot_Yin_Ball_Bullet(count + 1));
                        yield return new WaitForSeconds(3.0f);
                        break;
                    case 3://お札弾
                        StartCoroutine(Shoot_Talisman_Bullet(count + 1));
                        yield return new WaitForSeconds(1.5f * (count + 1));
                        break;
                    case 4://夢想封印      
                        Debug.Log("Muso Huin"); //TODO:夢想封印
                        break;
                }
                list.RemoveAt(num);
            }
            count++;
        }
    }


    //耐久用体力減少
    private IEnumerator Down_Boss_Life() {
        BossEnemyController boss_Controller = GetComponent<BossEnemyController>();
        while(boss_Controller.Get_Now_Phase() == 1) {            
            yield return new WaitForSeconds(1.0f);
            boss_Controller.Damaged(1);
            if(boss_Controller.life[0] < 10) {
                Debug.Log("Count Down Timer");
            }
        }
    }


    //ホーミング弾発射
    private IEnumerator Shoot_Homing_Bullet(int num) {
        for (int i = 0; i < 3; i++) {
            homing_Shoot_Function.Set_Bullet(homing_Bullet);
            homing_Shoot_Function.Some_Way_Bullet(num, 140f, 0, 40f, 7.0f);
            _move.Start_Random_Move(32f, 0.02f);
            yield return new WaitForSeconds(0.75f);
        }
    }


    //陰陽玉発射
    private IEnumerator Shoot_Yin_Ball_Bullet(int num) {
        Debug.Log("ChargeEffect");  //TODO:溜めエフェクト
        yield return new WaitForSeconds(1.5f);
        AngleTwoPoints _angle = new AngleTwoPoints();
        float angle = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        yin_Ball_Shoot_Function.Set_Bullet(yin_Ball_Bullet);
        for (int i = 0; i < num; i++) {
            angle += (360 / num) * i;
            yin_Ball_Shoot_Function.Turn_Shoot_Bullet(150f, angle, 0);            
        }
    }


    //お札弾発射
    private IEnumerator Shoot_Talisman_Bullet(int shoot_Count) {
        for(int i = 0; i < shoot_Count; i++) {
            //赤弾
            talisman_Shoot_Function.Set_Bullet_Pool(pool_Manager.Get_Pool("RedTalismanBullet"));
            for(int j = 0; j < 5; j++) {
                talisman_Shoot_Function.Diffusion_Bullet(18, 120f - j * 4f, j * 2f, 7.0f);
            }
            //白弾
            talisman_Shoot_Function.Set_Bullet_Pool(pool_Manager.Get_Pool("WhiteTalismanBullet"));
            for(int j = 5; j < 10; j++) {
                talisman_Shoot_Function.Diffusion_Bullet(18, 120f - j * 4f, -j * 2 , 7.0f);
            }
            yield return new WaitForSeconds(0.75f);
        }
    }


    //攻撃中止用
    public void Stop_Reimu_Attack() {
        StopAllCoroutines();
    }
}
