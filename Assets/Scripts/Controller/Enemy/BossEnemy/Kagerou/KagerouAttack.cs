using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouAttack : MonoBehaviour {

    //フェーズ1用
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
        public bool end_Rush = false;
    }

    //フェーズ2用
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
        public GameObject mini_Wolfs;
        public GameObject grounds;
    }

    //フェーズ3用
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
    }

    //フェーズ4用
    [System.Serializable]
    public class Phase4_Status {
        public bool start_Routine = true;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;
    public Phase4_Status phase4;

    //オブジェクトプール
    private ObjectPoolManager pool_Manager;
    private GameObject blue_Bullet;
    private GameObject red_Bullet;
    private GameObject purple_Bullet;

    //コンポーネント
    private KagerouController _controller;
    private BossEnemyController boss_Controller;
    private MoveBetweenTwoPoints _move;
    private WolfRush _rush;
    private ScatterPoolBullet _scatter;
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        blue_Bullet = Resources.Load("Bullet/PooledBullet/BlueBulletPool") as GameObject;
        red_Bullet = Resources.Load("Bullet/PooledBullet/RedBulletPool") as GameObject;
        purple_Bullet = Resources.Load("Bullet/PooledBullet/PurpleBulletPool") as GameObject;
        pool_Manager.Create_New_Pool(blue_Bullet, 20);
        pool_Manager.Create_New_Pool(red_Bullet, 20);
        pool_Manager.Create_New_Pool(purple_Bullet, 20);
        //取得
        _controller = GetComponent<KagerouController>();
        boss_Controller = GetComponent<BossEnemyController>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _rush = GetComponent<WolfRush>();
        _scatter = GetComponent<ScatterPoolBullet>();
    }
	
	
    //フェーズ1
    public void Phase1() {
        if (phase1.start_Routine) {
            phase1.start_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }

    private IEnumerator Phase1_Routine() {
        //初期位置に移動
        _controller.Change_Parametar("IdleBool", 1);
        _move.Start_Move(new Vector3(200f, -16f), 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(1.0f);

        while (boss_Controller.Get_Now_Phase() == 1) {

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
                while (!phase1.end_Rush) { yield return null; }

                //左から右
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position = new Vector3(transform.position.x, -transform.position.y);

                StartCoroutine(Rush_Deposite(1));
                while (!phase1.end_Rush) { yield return null; }

                _rush.Start_Rush(new Vector2(200f, -16f));
                yield return new WaitUntil(_rush.End_Rush);
                transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
                transform.localScale = new Vector3(1, 1, 1);
            }

            //ばらまき弾
            {
                _controller.Roar();
                _scatter.Set_Bullet_Pool(pool_Manager.Get_Pool(purple_Bullet));
                _scatter.Start_Scatter(80f, 50f, 5.0f, 10.0f);
                yield return new WaitForSeconds(5.0f);
                _scatter.Stop_Scatter();
            }
            yield return new WaitForSeconds(4.5f);
        }
    }


    //移動しながら弾を設置する
    private IEnumerator Rush_Deposite(int direction) {
        phase1.end_Rush = false;
        for (int i = 0; i < 7; i++) {
            _rush.Start_Rush(new Vector2(transform.position.x + 64f * direction, -transform.position.y));
            yield return new WaitUntil(_rush.End_Rush);
            Deposit_Burst_Bullet();
        }
        phase1.end_Rush = true;
    }


    //はじける弾の設置
    private void Deposit_Burst_Bullet() {
        string bullet_Path = "Bullet/KagerouBurstBullet";
        GameObject bullet = Instantiate(Resources.Load(bullet_Path) as GameObject);        
        bullet.transform.position = transform.position;
    }
    

    //フェーズ2
    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            //フェーズ1中止
            StopCoroutine(Phase1_Routine());
            _rush.StopAllCoroutines();
            _scatter.Stop_Scatter();
            //フェーズ2
            StartCoroutine("Phase2_Routine");
        }
    }

    private IEnumerator Phase2_Routine() {
        //無敵化、移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        _controller.Change_Parametar("DashBool", 1);
        _move.Start_Move(new Vector3(0f, 10f), 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parametar("IdleBool", 1);
        //透明化、魔法陣消す
        transform.GetChild(5).gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.15f);
        //耐久開始
        StartCoroutine("Start_Phase2_Timer");
        //敵生成
        phase2.grounds.SetActive(true);
        Phase2_Enemy_Gen();
    }


    //耐久開始
    private IEnumerator Start_Phase2_Timer() {
        while(boss_Controller.life[1] >= 1) {
            yield return new WaitForSeconds(1.0f);
            boss_Controller.life[1]--;
            if(boss_Controller.life[1] <= 10) {
                //カウントダウン効果音
                Debug.Log("Phase2 Timer Count Down Sound");
            }
        }
    }

    //敵生成
    private void Phase2_Enemy_Gen() {
        phase2.mini_Wolfs.GetComponent<MiniWolfsParent>().Appear();
    }


    //フェーズ3
    public void Phase3() {
        //フェーズ2終了
        Destroy(GameObject.Find("MiniKagerous"));
    }

    //フェーズ4
    public void Phase4() {

    }

}
