using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyAttack : MonoBehaviour {

    //フェーズ1
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
        public GameObject shoot_Obj;
    }
    //フェーズ2
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
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
        public GameObject shoot_Obj;
        public GameObject shadow_Doremy;
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
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/DoremyBullet") as GameObject, 20);
	}


    /*----------------------------フェーズ1------------------------------*/

    public void Phase1() {
        if (phase1.start_Routine) {
            phase1.start_Routine = false;
            //フェーズ1開始
            StartCoroutine("Phase1_Routine");
        }
    }

    private IEnumerator Phase1_Routine() {
        //移動
        _controller.Change_Layer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _controller.Start_Warp(new Vector2(160f, -48f), 1);
        yield return new WaitForSeconds(1.5f);
        _controller.Change_Layer("EnemyLayer");

        //ショット
        DoremySpiralShoot _spiral = phase1.shoot_Obj.GetComponent<DoremySpiralShoot>();
        while (boss_Controller.Get_Now_Phase() == 1) {
            _spiral.Start_Spiral_Shoot();
            yield return new WaitForSeconds(6.0f);
            _spiral.Stop_Spiral_Shoot();
            //移動
            _controller.Move_Randome();
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void Stop_Phase1() {
        phase1.shoot_Obj.GetComponent<DoremySpiralShoot>().Stop_Spiral_Shoot();
        StopCoroutine("Phase2_Routine");
    }

    /*----------------------------フェーズ2------------------------------*/

    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Routine");
        }
    }

    private IEnumerator Phase2_Routine() {
        yield return null;
    }

    private void Stop_Phase2() {

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

    private void Stop_Phase3() {

    }


    /*----------------------------フェーズ4------------------------------*/
    public void Phase4() {
        if (phase4.start_Routine) {
            phase4.start_Routine = false;
            Stop_Phase3();
            StartCoroutine("Phase4_Routine");
        }
    }

    private IEnumerator Phase4_Routine() {
        //初期設定
        DoremyPhase4ShootObj phase4_Shoot = phase4.shoot_Obj.GetComponent<DoremyPhase4ShootObj>();
        //移動
        _controller.Change_Layer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _controller.Move(new Vector2(160f, 16f), 0.015f);
        yield return new WaitUntil(_controller.End_Move);
        _controller.Change_Layer("EnemyLayer");

        yield return new WaitForSeconds(1.5f);

        while (boss_Controller.Get_Now_Phase() == 4) {
            //四角に瞬間移動してショット
            {
                StartCoroutine(Phase4_Shoot1(new Vector2(200f, 120f), 45f));
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(Phase4_Shoot1(new Vector2(-200f, 120f), 135f));
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(Phase4_Shoot1(new Vector2(-200f, -120f), 225f));
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(Phase4_Shoot1(new Vector2(200f, -120f), -45f));
            }

            yield return new WaitForSeconds(1.5f);
            
            //中央で渦巻き弾
            {
                _controller.Start_Warp(new Vector2(0, 0), 1);
                yield return new WaitForSeconds(1.0f);
                phase4_Shoot.Start_Spiral_Shoot();
                yield return new WaitForSeconds(8.0f);
                phase4_Shoot.Stop_Spiral_Shoot();
            }

            yield return new WaitForSeconds(2.0f);
            
            //分裂してばらまき弾
            {
                _controller.Move(new Vector2(120f, 0), 0.02f);
                phase4.shadow_Doremy.transform.position = transform.position;
                phase4.shadow_Doremy.SetActive(true);
                phase4.shadow_Doremy.GetComponent<MoveBetweenTwoPoints>().Start_Move(new Vector3(-120f, 0), 0, 0.02f);
                yield return new WaitForSeconds(1.5f);
                phase4_Shoot.Start_Scatter_Shoot();
                yield return new WaitForSeconds(6.0f);
                phase4_Shoot.Stop_Scatter_Shoot();
                phase4.shadow_Doremy.SetActive(false);
            }

            yield return new WaitForSeconds(4.0f);
        }
    }

    private IEnumerator Phase4_Shoot1(Vector2 pos, float center_Angle) {
        _controller.Start_Warp(pos, (int)(pos.x / Mathf.Abs(pos.x)));
        yield return new WaitForSeconds(1.0f);
        phase4.shoot_Obj.GetComponent<DoremyPhase4ShootObj>().Shoot_Five_Way_Bullet(center_Angle);
    }

    private void Stop_Phase4() {
        StopCoroutine("Phase4_Routine");
        StopCoroutine("Phase4_Shoot1");
        phase4.shoot_Obj.GetComponent<DoremyPhase4ShootObj>().Stop_Scatter_Shoot();
        phase4.shoot_Obj.GetComponent<DoremyPhase4ShootObj>().Stop_Spiral_Shoot();
        phase4.shadow_Doremy.SetActive(false);
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
