using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyAttack : MonoBehaviour {

    //フェーズ1
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
    }
    //フェーズ2
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
        public GameObject shoot_Obj;
    }
    //フェーズ3
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
    }
    //フェーズ4
    [System.Serializable]
    public class Phase4_Status {
        public bool start_Routine = true;
    }
    //フェーズ5
    [System.Serializable]
    public class Phase5_Status {
        public bool start_Routine = true;
    }
    //フェーズ6
    [System.Serializable]
    public class Phase6_Status {
        public bool start_Routine = true;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;
    public Phase4_Status phase4;
    public Phase5_Status phase5;
    public Phase5_Status phase6;

    //スクリプト
    private DoremyController _controller;
    private BossEnemyController boss_Controller;
    private ObjectPoolManager pool_Manager;


    //Awake
    private void Awake() {
        //取得
        _controller = GetComponent<DoremyController>();
        boss_Controller = GetComponent<BossEnemyController>();
    }


    // Use this for initialization
    void Start () {
        //オブジェクトプール
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/SmallBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/RedMiddleBullet") as GameObject, 20);
	}
	
	
    /*----------------------------フェーズ1------------------------------*/
    public void Phase1() {

    }

    private IEnumerator Phase1_Routine() {
        yield return null;
    }

    private void Stop_Phase1() {

    }

    /*----------------------------フェーズ2------------------------------*/
    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            //フェーズ1終了
            Stop_Phase1();
            //フェーズ2開始
            StartCoroutine("Phase2_Routine");
        }
    }

    private IEnumerator Phase2_Routine() {
        //移動
        _controller.Change_Layer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _controller.Start_Warp(new Vector2(160f, -48f));
        yield return new WaitForSeconds(1.5f);
        _controller.Change_Layer("EnemyLayer");

        //ショット
        DoremySpiralShoot _spiral = phase2.shoot_Obj.GetComponent<DoremySpiralShoot>();
        while (boss_Controller.Get_Now_Phase() == 2) {
            _spiral.Start_Spiral_Shoot();
            yield return new WaitForSeconds(6.0f);
            _spiral.Stop_Spiral_Shoot();
            //移動
            _controller.Move_Randome();
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void Stop_Phase2() {
        phase2.shoot_Obj.GetComponent<DoremySpiralShoot>().Stop_Spiral_Shoot();
        StopCoroutine("Phase2_Routine");
    }


    /*----------------------------フェーズ3------------------------------*/
    public void Phase3() {
        if (phase3.start_Routine) {
            phase3.start_Routine = false;
            //フェーズ2終了
            Stop_Phase2();
            //フェーズ3開始
            StartCoroutine("Phase3_Routine");
        }
    }

    private IEnumerator Phase3_Routine() {
        yield return null;
    }


    /*----------------------------フェーズ4------------------------------*/
    public void Phase4() {

    }

    private IEnumerator Phase4_Routine() {
        yield return null;
    }


    /*----------------------------フェーズ5------------------------------*/
    public void Phase5() {

    }

    private IEnumerator Phase5_Routine() {
        yield return null;
    }


    /*----------------------------フェーズ6------------------------------*/
    public void Phase6() {

    }

    private IEnumerator Phase6_Routine() {
        yield return null;
    }

}
