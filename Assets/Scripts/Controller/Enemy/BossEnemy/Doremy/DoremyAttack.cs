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
        public GameObject shoot_Obj;
        public GameObject vacuum_Effect;
    }
    //フェーズ3
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
        public GameObject shoot_Obj;

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
        public GameObject doreKing;
    }
    //フェーズ6
    [System.Serializable]
    public class Phase6_Status {
        public bool start_Routine = true;
        public GameObject shoot_Obj;
        public GameObject ground;
        public List<GameObject> objects;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;
    public Phase4_Status phase4;
    public Phase5_Status phase5;
    public Phase6_Status phase6;

    //スクリプト
    private DoremyController _controller;
    private BossEnemyController boss_Controller;
    private ObjectPoolManager pool_Manager;
    private MoveBetweenTwoPoints _move;

    //自機
    private GameObject player;


    //Awake
    private void Awake() {
        //取得
        _controller = GetComponent<DoremyController>();
        boss_Controller = GetComponent<BossEnemyController>();
        _move = GetComponent<MoveBetweenTwoPoints>();
    }


    // Use this for initialization
    void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        //オブジェクトプール
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/SmallBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/RedMiddleBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/DoremyBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/RingBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/EllipseBullet") as GameObject, 50);
    }


    /*----------------------------フェーズ1------------------------------------*/
    #region phase1
    public void Phase1() {
        if (phase1.start_Routine) {
            phase1.start_Routine = false;
            //フェーズ1開始
            StartCoroutine("Phase1_Routine");
        }
    }

    private IEnumerator Phase1_Routine() {
        //移動
        _controller.Start_Warp(new Vector2(160f, -16f), 1);
        yield return new WaitForSeconds(1.8f);
        _controller.Play_Charge_Effect(2.5f);
        _controller.Appear_Back_Design(new Vector3(160f, -32f), new Color(0.2f, 0.2f, 0.2f, 0.15f));
        yield return new WaitForSeconds(2.5f);

        //ショット
        DoremySpiralShoot _spiral = phase1.shoot_Obj.GetComponent<DoremySpiralShoot>();
        while (boss_Controller.Get_Now_Phase() == 1) {
            _spiral.Start_Spiral_Shoot();
            _controller.Play_Spread_Effect();
            yield return new WaitForSeconds(6.0f);
            _spiral.Stop_Spiral_Shoot();
            //移動
            _controller.Move_Randome();
            yield return new WaitForSeconds(3.0f);
            _controller.Play_Charge_Effect(2.5f);
            yield return new WaitForSeconds(2.5f);

        }
    }

    private void Stop_Phase1() {
        phase1.shoot_Obj.GetComponent<DoremySpiralShoot>().Stop_Spiral_Shoot();
        StopCoroutine("Phase2_Routine");
        _controller.Delete_Back_Design();
    }
    #endregion
    /*----------------------------フェーズ2------------------------------------*/
    #region phase2
    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Routine");
        }
    }

    private IEnumerator Phase2_Routine() {
        DoremyPhase2ShootObj phase2_Shoot = phase2.shoot_Obj.GetComponent<DoremyPhase2ShootObj>();

        while (boss_Controller.Get_Now_Phase() == 2) {
            //移動
            _controller.Warp_In_Phase_Change(new Vector2(160f, -32f), 1);
            yield return new WaitForSeconds(3.5f);
            _controller.Appear_Back_Design(transform.position, new Color(0.3f, 0.3f, 0.3f, 0.15f));
            //4Wayショット
            for (int i = 0; i < 4; i++) {
                phase2_Shoot.Shoot_Ring_Bullet();
                yield return new WaitForSeconds(0.5f);
                Vector2 noise = new Vector2(Random.Range(0, 30f), Random.Range(0, 48f));
                _controller.Start_Warp(new Vector2(190f, player.transform.position.y) + noise, 1);
                yield return new WaitForSeconds(1.0f);
            }

            //溜め
            _controller.Move(new Vector2(160f, 0), 0.02f);
            _controller.Play_Charge_Effect(2.5f);
            yield return new WaitForSeconds(2.5f);

            //無敵化
            _controller.Change_Layer("InvincibleLayer");
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 0.5f, 0.3f);

            //ナイトメア弾発射
            _controller.Play_Spread_Effect();
            GameObject nightmare_Bullet = phase2_Shoot.Shoot_Nightmare_Bullet();

            //引き寄せ開始
            phase2.shoot_Obj.GetComponent<Vacuum>().Start_Vacuum(player, 400f);
            phase2.vacuum_Effect = Instantiate(Resources.Load("Effect/PowerChargeEffects") as GameObject);
            phase2.vacuum_Effect.transform.position = transform.position;

            //全方位弾
            phase2_Shoot.Start_Diffusion_Shoot();

            //終了
            while (nightmare_Bullet != null) { yield return null; }
            phase2.shoot_Obj.GetComponent<Vacuum>().Stop_Vacuum();
            phase2_Shoot.Stop_Diffusion_Shoot();
            Destroy(phase2.vacuum_Effect);
            _controller.Change_Layer("EnemyLayer");
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            GetComponent<BossEnemyController>().Damaged(40);
        }
    }

    private void Stop_Phase2() {
        StopCoroutine("Phase2_Routine");
        phase2.shoot_Obj.GetComponent<DoremyPhase2ShootObj>().Stop_Shoot();
        phase2.shoot_Obj.GetComponent<Vacuum>().Stop_Vacuum();
        Destroy(phase2.vacuum_Effect);
        _controller.Change_Layer("EnemyLayer");
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        _controller.Delete_Back_Design();
    }
    #endregion
    /*----------------------------フェーズ3------------------------------------*/
    #region phase3
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
        DoremyPhase3ShootObj phase3_Shoot = phase3.shoot_Obj.GetComponent<DoremyPhase3ShootObj>();
        //移動
        _controller.Warp_In_Phase_Change(new Vector2(150f, 0), 1);
        yield return new WaitForSeconds(2.5f);
        _controller.Appear_Back_Design(new Vector3(0, 0), new Color(0.4f, 0.4f, 0.4f, 0.15f));

        while (boss_Controller.Get_Now_Phase() == 3) {
            //奇数段
            for (int i = 0; i < 3; i++) {
                //ワープ
                _controller.Do_Randome_Warp();
                yield return new WaitForSeconds(1.0f);
                _controller.Do_Randome_Warp();
                yield return new WaitForSeconds(1.0f);
                //ショット
                phase3_Shoot.Shoot_Bounce_Bullet();
                yield return new WaitForSeconds(1.5f);
            }
            //全方位弾
            {
                _controller.Start_Warp(new Vector2(0, 0), 1);
                yield return new WaitForSeconds(1.0f);
                _controller.Play_Charge_Effect(1.0f);
                yield return new WaitForSeconds(1.0f);
                phase3_Shoot.Shoot_Diffusion_Bullet();
                _controller.Play_Spread_Effect();
                yield return new WaitForSeconds(1.0f);
                phase3_Shoot.Shoot_Diffusion_Bullet();
                _controller.Play_Spread_Effect();
            }
            yield return new WaitForSeconds(0.2f);
            //爆撃
            {
                _controller.Start_Warp(new Vector2(0, 100f), 1);
                yield return new WaitForSeconds(1.0f);
                phase3_Shoot.Start_Drop_Bullet(18, 0.4f);
                yield return new WaitForSeconds(7.2f);
            }
        }
    }

    private void Stop_Phase3() {
        StopCoroutine("Phase3_Routine");
        phase3.shoot_Obj.GetComponent<DoremyPhase3ShootObj>().Stop_Shoot();
        _controller.Delete_Back_Design();
    }
    #endregion
    /*----------------------------フェーズ4------------------------------------*/
    #region phase4
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
        _controller.Appear_Back_Design(new Vector3(0, 0), new Color(0.5f, 0.5f, 0.5f, 0.15f));

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
                UsualSoundManager.Laser_Sound();
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
        _controller.Delete_Back_Design();
    }
    #endregion
    /*----------------------------フェーズ5------------------------------------*/
    #region phase5
    public void Phase5() {
        if (phase5.start_Routine) {
            phase5.start_Routine = false;
            Stop_Phase4();
            StartCoroutine("Phase5_Routine");
        }
    }

    private IEnumerator Phase5_Routine() {
        DoreKing doreKing_Controller = phase5.doreKing.GetComponent<DoreKing>();
        //移動
        _controller.Warp_In_Phase_Change(new Vector2(260f, -140f), 1);
        yield return new WaitForSeconds(3.5f);
        //ドレキング登場
        _controller.Change_Parameter("TransformBool", 1);
        _move.Start_Move(new Vector3(96f, -30f), 0, 0.01f);
        gameObject.AddComponent<CameraShake>().Shake(2.5f, 1.0f, true);
        yield return new WaitUntil(_move.End_Move);
        phase5.doreKing.SetActive(true);
        _controller.Appear_Back_Design(new Vector3(0, 0), new Color(0.5f, 0.5f, 0.5f, 0.15f));

        while (boss_Controller.Get_Now_Phase() == 5) {
            //ランダムでパターン選択
            List<int> num_List = new List<int>() { 1, 2, 3, 4 };
            int num = 1;
            while (num_List.Count > 0) {
                //パターン選択
                num = num_List[Random.Range(0, num_List.Count)];
                num_List.Remove(num);
                //攻撃
                switch (num) {
                    case 1:
                        doreKing_Controller.Shoot_In_Under_Obj();
                        yield return new WaitForSeconds(1.5f);
                        break;
                    case 2:
                        doreKing_Controller.Shoot_In_Upper_Obj();
                        yield return new WaitForSeconds(0.8f);
                        break;
                    case 3:
                        doreKing_Controller.Start_Spiral_Shoot();
                        yield return new WaitForSeconds(2.5f);
                        break;
                    case 4:
                        yield return new WaitForSeconds(0.5f);
                        doreKing_Controller.Put_Out_Beam();
                        yield return new WaitForSeconds(1.0f);
                        break;
                }
            }
        }
    }

    private void Stop_Phase5() {
        _move.Start_Move(new Vector3(260f, -140f), 0, 0.01f);
        phase5.doreKing.GetComponent<DoreKing>().Stop_Shoot();
        StopCoroutine("Phase5_Routine");
        _controller.Delete_Back_Design();
    }
    #endregion
    /*----------------------------フェーズ6------------------------------------*/
    #region phase6
    public void Phase6() {
        if (phase6.start_Routine) {
            phase6.start_Routine = false;
            Stop_Phase5();
            StartCoroutine("Phase6_Routine");
        }
    }

    private IEnumerator Phase6_Routine() {
        //ドレキング終了
        _controller.Change_Layer("InvincibleLayer");
        yield return new WaitForSeconds(2.0f);
        _controller.Change_Parameter("IdleBool", 1);
        _controller.Warp_In_Phase_Change(new Vector2(0, 0), 1);
        phase6.ground.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        _controller.Appear_Back_Design(new Vector3(0, 0, 0), new Color(0.7f, 0.7f, 0.7f, 0.15f));
        _controller.Play_Charge_Effect(1.5f);
        yield return new WaitForSeconds(1.5f);
        _controller.Play_Spread_Effect();

        //ショット開始
        while (boss_Controller.Get_Now_Phase() == 6) {
            //赤
            Start_Shoot(new Color(0.8f, 0.2f, 0.2f), 1, -15f);
            yield return new WaitForSeconds(3.0f);
            //青
            Start_Shoot(new Color(0.2f, 0.2f, 0.8f), -1, 50f);
            yield return new WaitForSeconds(3.0f);
            //体力が減るごとに増やす
            if (boss_Controller.life[5] < boss_Controller.LIFE[5] * 0.75f) {
                Start_Shoot(new Color(0.3f, 0.7f, 0.3f), 1, -15f);
                yield return new WaitForSeconds(2.0f);
            }
            if (boss_Controller.life[5] < boss_Controller.LIFE[5] * 0.5f) {
                Start_Shoot(new Color(0.7f, 0.7f, 0.1f), -1, 50f);
                yield return new WaitForSeconds(1.5f);
            }
            if (boss_Controller.life[5] < boss_Controller.LIFE[5] * 0.25f) {
                Start_Shoot(new Color(0.7f, 0.1f, 0.7f), 1, -15f);
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(7.0f);
        }
    }

    //ショット開始
    private void Start_Shoot(Color bullet_Color, int rotate_Direction, float spiral_Span) {
        GameObject shoot_Obj = Instantiate(phase6.shoot_Obj);
        phase6.objects.Add(shoot_Obj);
        shoot_Obj.transform.position = transform.position + new Vector3(32f, 0);
        shoot_Obj.GetComponent<RotateSpreadObject>().Set_Status(transform.position, 1.0f * rotate_Direction, 0.42f);
        shoot_Obj.GetComponent<DoremyPhase6ShootObj>().Start_Spiral_Shoot(bullet_Color, spiral_Span);
        shoot_Obj.GetComponent<DoremyPhase6ShootObj>().Invoke("Stop_Shoot", 10.0f);
        _controller.Play_Spread_Effect();
        UsualSoundManager.Shot_Sound();
    }

    public void Stop_Phase6() {
        StopCoroutine("Phase6_Routine");
        phase6.ground.SetActive(false);
        player.transform.Find("PlayerBody").gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        foreach(var obj in phase6.objects) {
            if(obj != null) {
                Destroy(obj);
            }
        }
        _controller.Delete_Back_Design();
    }
    #endregion

}
