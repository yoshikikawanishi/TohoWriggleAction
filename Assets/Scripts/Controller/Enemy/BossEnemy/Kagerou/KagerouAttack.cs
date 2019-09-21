using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouAttack : MonoBehaviour {

    //フェーズ1用
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
        public int bound_Count = 0;
        public GameObject drop_Bullet_Prefab;
    }


    //フェーズ2用
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
        public bool end_Rush = false;
        public GameObject bullet_Parent;
    }

    //フェーズ3用
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
        public GameObject mini_Wolfs;
        public GameObject grounds;
    }

    //フェーズ4用
    [System.Serializable]
    public class Phase4_Status {
        public bool start_Routine = true;
    }

    //フェーズ5用
    [System.Serializable]
    public class Phase5_Status {
        public bool start_Routine = true;
        public GameObject shot_Objects;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;
    public Phase4_Status phase4;
    public Phase5_Status phase5;

    //オブジェクトプール
    private ObjectPoolManager pool_Manager;
    private GameObject[] color_Bullet = new GameObject[5];
    private enum BulletColor {
        red = 0,
        blue = 1,
        purple = 2,
        yellow = 3,
        green = 4,
    }

    //コンポーネント
    private KagerouController _controller;
    private BossEnemyController boss_Controller;
    private Rigidbody2D _rigid;
    private MoveBetweenTwoPoints _move;
    private WolfRush _rush;
    private ScatterPoolBullet _scatter;
    private BulletPoolFunctions[] _bullet = new BulletPoolFunctions[2];
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        color_Bullet[0] = Resources.Load("Bullet/PooledBullet/RedBulletPool") as GameObject;
        color_Bullet[1] = Resources.Load("Bullet/PooledBullet/BlueBulletPool") as GameObject;
        color_Bullet[2] = Resources.Load("Bullet/PooledBullet/PurpleBulletPool") as GameObject;
        color_Bullet[3] = Resources.Load("Bullet/PooledBullet/YellowBulletPool") as GameObject;
        color_Bullet[4] = Resources.Load("Bullet/PooledBullet/GreenBulletPool") as GameObject;

        for (int i = 0; i < 5; i++) {
            pool_Manager.Create_New_Pool(color_Bullet[i], 40);
        }
        pool_Manager.Create_New_Pool(Resources.Load("Bullet/PooledBullet/RedMiddleBullet") as GameObject, 20);
        pool_Manager.Create_New_Pool(phase1.drop_Bullet_Prefab, 30);
        pool_Manager.Create_New_Pool(Resources.Load("Enemy/KagerouFamiliar") as GameObject, 18);

        //取得
        _controller = GetComponent<KagerouController>();
        boss_Controller = GetComponent<BossEnemyController>();
        _rigid = GetComponent<Rigidbody2D>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _rush = GetComponent<WolfRush>();
        _scatter = GetComponent<ScatterPoolBullet>();
        _bullet[0] = gameObject.AddComponent<BulletPoolFunctions>();
        _bullet[1] = gameObject.AddComponent<BulletPoolFunctions>();
    }


    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "GroundTag" && phase1.bound_Count < 5) {
            Bound();
        }
        else if(collision.gameObject.tag == "PlayerBodyTag") {
            _rigid.velocity = new Vector2(-1f, 1f) * 350f;
        }
    }



    //フェーズ1
    #region phase1
    public void Phase1() {
        if (phase1.start_Routine) {
            phase1.start_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }

    private IEnumerator Phase1_Routine() {
        _controller.Appear_Back_Design(new Vector3(0, 0), new Color(0.1f, 0.1f, 0.9f, 0.07f));
        while (boss_Controller.Get_Now_Phase() == 1) {
            //エフェクト
            _controller.Transform_Effect();
            yield return new WaitForSeconds(0.6f);
            //突進開始
            Start_Bound_Rush();
            GetComponent<CircleCollider2D>().isTrigger = false;
            //5かい跳ねるまで待つ
            while (phase1.bound_Count < 5) {
                Adjast_Angle();
                _rigid.velocity = _rigid.velocity.normalized * 350f;
                yield return new WaitForSeconds(0.016f);                
            }
            //止まって弾生成
            StartCoroutine("Stop_Rush");
            yield return new WaitForSeconds(1.0f);
            StartCoroutine("Generate_Drop_Bullet");
            yield return new WaitForSeconds(6.0f);
        }
    }

    //突進
    private void Start_Bound_Rush() {
        //変身、突進
        _controller.Change_Parametar("RushBool", 1);
        _rigid.velocity = new Vector2(-1f, 1f) * 350f;
        UsualSoundManager.Familiar_Appear_Sound();
    }

    //方向の調整
    private void Adjast_Angle() {
        //回転
        transform.localScale = new Vector3(1, 1, 1);
        if (_rigid.velocity.x >= 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        float dirVelocity = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(dirVelocity - 90, new Vector3(0, 0, 1));
    }

    //跳ね返り
    private void Bound() {
        phase1.bound_Count++;
        _controller.Shake_Camera(0.3f, 0.35f * phase1.bound_Count);
        boss_Controller.Damaged(3);
        UsualSoundManager.Shot_Sound();
    }

    //止まる
    private IEnumerator Stop_Rush() {
        _rigid.velocity = new Vector2(0, 0);        
        _controller.Shake_Camera(5.0f, 1.2f);
        GetComponent<CircleCollider2D>().isTrigger = true;
        phase1.bound_Count = 0;
        _controller.Play_Spread_Effect();
        yield return new WaitForSeconds(1.5f);
        _controller.Change_Parametar("IdleBool", 1);
        transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
    }

    //弾生成
    private IEnumerator Generate_Drop_Bullet() {
        ObjectPool pool = pool_Manager.Get_Pool(phase1.drop_Bullet_Prefab);
        for (int i = 0; i < 90; i++) {
            GameObject bullet = pool.GetObject();
            var pos = new Vector2(Random.Range(-250f, 250f), Random.Range(150f, 180f));
            bullet.transform.position = pos;
            bullet.GetComponent<EnemyBullet>().Delete_Pool_Bullet(5.0f);
            UsualSoundManager.Small_Shot_Sound();
            yield return new WaitForSeconds(0.035f);
        }
    }

    private void Stop_Phase1() {
        GetComponent<CircleCollider2D>().isTrigger = true;
        StopAllCoroutines();
        Stop_Rush();
        _rigid.velocity = new Vector2(0, 0);
    }
        
    #endregion

    //フェーズ2
    #region phase2
    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Routine");
        }
    }

    private IEnumerator Phase2_Routine() {
        //初期位置に移動
        _controller.Change_Parametar("IdleBool", 1);
        _move.Start_Move(new Vector3(200f, -16f), 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Appear_Back_Design(transform.position, new Color(0.2f, 0.2f, 0.8f, 0.07f));
        yield return new WaitForSeconds(0.5f);

        while (boss_Controller.Get_Now_Phase() == 2) {

            _controller.Transform_Effect();
            yield return new WaitForSeconds(0.5f);            

            //突進
            {
                //右から左
                _rush.Start_Rush(new Vector2(100f, 120f));
                _controller.Roar_Sound();
                yield return new WaitUntil(_rush.End_Rush);
                Deposit_Burst_Bullet();

                StartCoroutine(Rush_Deposite(-1));
                while (!phase2.end_Rush) { yield return null; }

                //左から右
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = new Vector3(transform.position.x, -transform.position.y);

                StartCoroutine(Rush_Deposite(1));
                while (!phase2.end_Rush) { yield return null; }

                _rush.Start_Rush(new Vector2(200f, -16f));
                yield return new WaitUntil(_rush.End_Rush);
                transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
                transform.localScale = new Vector3(1, 1, 1);
            }

            //ばらまき弾
            {
                _controller.Roar();
                _scatter.Set_Bullet_Pool(pool_Manager.Get_Pool(color_Bullet[(int)BulletColor.purple]));
                _scatter.Start_Scatter(80f, 50f, 5.0f, 10.0f);
                yield return new WaitForSeconds(5.0f);
                _scatter.Stop_Scatter();
            }
            yield return new WaitForSeconds(4.5f);
        }
    }


    //移動しながら弾を設置する
    private IEnumerator Rush_Deposite(int direction) {
        phase2.end_Rush = false;
        for (int i = 0; i < 7; i++) {
            _rush.Start_Rush(new Vector2(transform.position.x + 64f * direction, -transform.position.y));
            yield return new WaitUntil(_rush.End_Rush);
            Deposit_Burst_Bullet();
        }
        phase2.end_Rush = true;
    }


    //はじける弾の設置
    private void Deposit_Burst_Bullet() {
        string bullet_Path = "Bullet/KagerouBurstBullet";
        GameObject bullet = Instantiate(Resources.Load(bullet_Path) as GameObject);        
        bullet.transform.position = transform.position;
        bullet.transform.SetParent(phase2.bullet_Parent.transform);
        _controller.Shake_Camera(0.3f, 0.7f);
        UsualSoundManager.Shot_Sound();
    }
    #endregion

    //フェーズ3
    #region phase3
    public void Phase3() {
        if (phase3.start_Routine) {
            phase3.start_Routine = false;
            //フェーズ2中止
            StopCoroutine(Phase2_Routine());                    /*----------フェーズ2中止-------*/
            _rush.StopAllCoroutines();
            _scatter.Stop_Scatter();
            transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
            _controller.Delete_Back_Design();
            Destroy(phase2.bullet_Parent);
            //フェーズ3
            StartCoroutine("Phase3_Routine");
        }
    }

    private IEnumerator Phase3_Routine() {
        //無敵化、移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        _controller.Change_Parametar("DashBool", 1);
        _move.Start_Move(new Vector3(0f, 10f), 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parametar("IdleBool", 1);
        _controller.Appear_Back_Design(transform.position, new Color(0.4f, 0.2f, 0.6f, 0.07f));
        //透明化、魔法陣消す
        transform.GetChild(5).gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.15f);
        //耐久開始
        StartCoroutine("Start_Phase3_Timer");
        //敵生成
        phase3.grounds.SetActive(true);
        Phase3_Enemy_Gen();
    }


    //耐久開始
    private IEnumerator Start_Phase3_Timer() {
        AudioSource timer_Sound = GetComponents<AudioSource>()[0];
        while(boss_Controller.life[2] >= 0) {
            yield return new WaitForSeconds(1.0f);
            boss_Controller.life[2]--;
            if(boss_Controller.life[2] <= 5) {
                //カウントダウン効果音
                timer_Sound.Play();
            }
        }
        boss_Controller.Phase_Change(4);
    }

    //敵生成
    private void Phase3_Enemy_Gen() {
        phase3.mini_Wolfs.GetComponent<MiniWolfsParent>().Appear();
    }
    #endregion

    //フェーズ4
    #region phase4
    public void Phase4() {
        if (phase4.start_Routine) {
            phase4.start_Routine = false; 
            //フェーズ3終了
            Destroy(phase3.mini_Wolfs);
            Destroy(phase3.grounds);
            StopCoroutine("Phase3_Routine");
            transform.GetChild(5).gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            _controller.Delete_Back_Design();
            //フェーズ4開始
            StartCoroutine("Phase4_Routine");
        }
    }

    private IEnumerator Phase4_Routine() {
        //初期設定
        _bullet[0].Set_Bullet_Pool(pool_Manager.Get_Pool(color_Bullet[(int)BulletColor.red]));
        _bullet[1].Set_Bullet_Pool(pool_Manager.Get_Pool("RedMiddleBullet"));
        //移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _move.Start_Move(new Vector3(160f, -48f), 0, 0.02f);
        _controller.Change_Parametar("DashBool", -1);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parametar("IdleBool", 1);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        _controller.Appear_Back_Design(transform.position, new Color(0.6f, 0.2f, 0.4f, 0.07f));

        while (boss_Controller.Get_Now_Phase() == 4) {
            //ためエフェクト
            _controller.Play_Charge_Effect(2.667f);
            yield return new WaitForSeconds(2.667f);

            //全方位弾
            StartCoroutine("Phase4_Shot1");
            yield return new WaitForSeconds(2.667f);

            //偶数弾、奇数段
            StartCoroutine("Phase4_Shot2");
            StartCoroutine("Phase4_Shot3"); ;
            _move.Start_Random_Move(48f, 0.01f);

            yield return new WaitForSeconds(8.0f);
        }
    }

    //全方位弾
    private IEnumerator Phase4_Shot1() {
        _controller.Play_Spread_Effect();
        for (int i = 0; i < 10; i++) {
            _bullet[0].Diffusion_Bullet(18, 140f + i * 5, i * 2, 6.0f);
        }
        UsualSoundManager.Shot_Sound();
        yield return new WaitForSeconds(1.334f);
        _controller.Play_Spread_Effect();
        for (int i = 0;i < 10; i++) {
            _bullet[0].Diffusion_Bullet(18, 140f + i * 5, -i * 2, 6.0f);
        }
        UsualSoundManager.Shot_Sound();
    }

    //偶数弾
    private IEnumerator Phase4_Shot2() {
        for(float t = 0; t < 5.333f; t += 0.3333f) {
            UsualSoundManager.Small_Shot_Sound();
            _bullet[0].Even_Num_Bullet(18, 20f, 150f, 8.0f);
            yield return new WaitForSeconds(0.3333f);
        }
    }

    //奇数段
    private IEnumerator Phase4_Shot3() {
        for (float t = 0; t < 5.333f; t += 1.334f) {
            yield return new WaitForSeconds(1.334f);
            _bullet[1].Odd_Num_Bullet(30, 12f, 100f, 6.0f);
            UsualSoundManager.Shot_Sound();
        }
    }
    #endregion

    //フェーズ5
    #region phase5
    public void Phase5() {
        if (phase5.start_Routine) {
            phase5.start_Routine = false;
            //フェーズ4終了
            StopCoroutine("Phase4_Routine");
            StopCoroutine("Phase4_Shot1");
            StopCoroutine("Phase4_Shot2");
            StopCoroutine("Phase4_Shot3");
            _controller.Delete_Back_Design();
            //フェーズ5開始
            StartCoroutine("Phase5_Routine");
        }
    }

    private IEnumerator Phase5_Routine() {
        //初期設定
        _bullet[0].Set_Bullet_Pool(pool_Manager.Get_Pool(color_Bullet[0]));
        _bullet[1].Set_Bullet_Pool(pool_Manager.Get_Pool("KagerouFamiliar"));
        GameObject[] shot_Obj = new GameObject[4];
        for(int i = 0; i < 4; i++) {
            shot_Obj[i] = phase5.shot_Objects.transform.GetChild(i).gameObject;
        }
        //移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _move.Start_Move(new Vector3(0, -48f), 0, 0.02f);
        _controller.Change_Parametar("DashBool", 1);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parametar("IdleBool", 1);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        _controller.Appear_Back_Design(transform.position, new Color(0.8f, 0.2f, 0.2f, 0.07f));

        //ショット
        //本体
        StartCoroutine("Phase4_Red_Bullet", _bullet[0]);
        StartCoroutine("Phase4_Wolf_Bullet");
        while (boss_Controller.life[4] >= boss_Controller.LIFE[4] * 0.8f) {         
            yield return null;
        }

        //青
        StartCoroutine(Phase4_Color_Bullet(shot_Obj[0], BulletColor.blue));
        while (boss_Controller.life[4] >= boss_Controller.LIFE[4] * 0.6f) {
            yield return null;
        }

        //紫
        StartCoroutine(Phase4_Color_Bullet(shot_Obj[1], BulletColor.purple));
        while (boss_Controller.life[4] >= boss_Controller.LIFE[4] * 0.4f) {
            yield return null;
        }

        //黄色
        StartCoroutine(Phase4_Color_Bullet(shot_Obj[2], BulletColor.yellow));
        while (boss_Controller.life[4] >= boss_Controller.LIFE[4] * 0.2f) {
            yield return null;
        }

        //緑
        StartCoroutine(Phase4_Color_Bullet(shot_Obj[3], BulletColor.green));

    }

    //赤全方位弾
    private IEnumerator Phase4_Red_Bullet() {
        int i = 0;
        while (boss_Controller.Get_Now_Phase() == 5) {
            float center_Angle = Random.Range(0, 20f);
            _bullet[0].Diffusion_Bullet(20, 60f, center_Angle, 10.0f);
            UsualSoundManager.Shot_Sound();
            yield return new WaitForSeconds(0.333f);
            i++;
            if (i % 8 == 0) {
                yield return new WaitForSeconds(2.667f);
            }            
        }
    }

    //狼使い魔
    private IEnumerator Phase4_Wolf_Bullet() {
        while (boss_Controller.Get_Now_Phase() == 5) {
            yield return new WaitForSeconds(3.333f);
            _bullet[1].Diffusion_Bullet(12, 40f, 0, -1);
            UsualSoundManager.Shot_Sound();
            _controller.Play_Spread_Effect();
            yield return new WaitForSeconds(2.0f);
        }     
    }

    //色弾
    private IEnumerator Phase4_Color_Bullet(GameObject shot_Obj, BulletColor color) {
        BulletPoolFunctions b = shot_Obj.GetComponent<BulletPoolFunctions>();
        b.Set_Bullet_Pool(pool_Manager.Get_Pool(color_Bullet[(int)color]));
        ParticleSystem effect = shot_Obj.GetComponent<ParticleSystem>();
        while (boss_Controller.Get_Now_Phase() == 5) {
            effect.Play();
            yield return new WaitForSeconds(1.3333f);
            float center_Angle = Random.Range(0, 20f);
            b.Diffusion_Bullet(18, 120f, center_Angle, 6.0f);
            UsualSoundManager.Shot_Sound();
            yield return new WaitForSeconds(0.6667f);
        }
    }
    #endregion

    //フェーズ5終了
    public void Stop_Phase5() {
        StopAllCoroutines();
        _controller.Delete_Back_Design();
    }
    

}
