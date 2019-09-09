using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaAttack : MonoBehaviour {

    //フェーズ1用
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
        public int progress;
        public Vector2 start_Pos;
    }
    
    //フェーズ2用
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
        public Vector2 start_Pos;
        public GameObject familiars;
    }
    
    //フェーズ3用
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
        public Vector2 start_Pos;
        public SpiralBulletFunction[] spiral = new SpiralBulletFunction[2];
    }

    //フェーズ4用
    [System.Serializable]
    public class Phase4_Status {
        public bool start_Routine = true;
        public Vector2 start_Pos;
        public GameObject enclosure_Stars;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;
    public Phase4_Status phase4;

    //コンポーネント
    private MoveBetweenTwoPoints _move;
    private Rigidbody2D _rigid;
    private MarisaController _controller;

    //弾
    private ObjectPool[] star_Bullet_Pool = new ObjectPool[5];  //赤、青、緑、黄、紫
    private ObjectPool[] big_Star_Bullet_Pool = new ObjectPool[2];  //赤、青


    //Awake
    private void Awake() {
        //取得
        _move = GetComponent<MoveBetweenTwoPoints>();
        _rigid = GetComponent<Rigidbody2D>();
        _controller = GetComponent<MarisaController>();
    }


    // Use this for initialization
    void Start () {
        //5色星弾のオブジェクトプール
        star_Bullet_Pool[0] = gameObject.AddComponent<ObjectPool>();
        star_Bullet_Pool[0].CreatePool(Resources.Load("Bullet/PooledBullet/RedStarBullet") as GameObject, 20);
        star_Bullet_Pool[1] = gameObject.AddComponent<ObjectPool>();
        star_Bullet_Pool[1].CreatePool(Resources.Load("Bullet/PooledBullet/BlueStarBullet") as GameObject, 20);
        star_Bullet_Pool[2] = gameObject.AddComponent<ObjectPool>();
        star_Bullet_Pool[2].CreatePool(Resources.Load("Bullet/PooledBullet/GreenStarBullet") as GameObject, 20);
        star_Bullet_Pool[3] = gameObject.AddComponent<ObjectPool>();
        star_Bullet_Pool[3].CreatePool(Resources.Load("Bullet/PooledBullet/YellowStarBullet") as GameObject, 20);
        star_Bullet_Pool[4] = gameObject.AddComponent<ObjectPool>();
        star_Bullet_Pool[4].CreatePool(Resources.Load("Bullet/PooledBullet/PurpleStarBullet") as GameObject, 20);
        //大型星弾のオブジェクトプール
        big_Star_Bullet_Pool[0] = gameObject.AddComponent<ObjectPool>();
        big_Star_Bullet_Pool[0].CreatePool(Resources.Load("Bullet/PooledBullet/BigStarBullet1") as GameObject, 20);
        big_Star_Bullet_Pool[1] = gameObject.AddComponent<ObjectPool>();
        big_Star_Bullet_Pool[1].CreatePool(Resources.Load("Bullet/PooledBullet/BigStarBullet2") as GameObject, 20);
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
        phase1.progress = 1;
        _move.Start_Move(phase1.start_Pos, 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Appear_Back_Design(transform.position);
        yield return new WaitForSeconds(1.5f);
        while (true) {
            //上を横断しながら弾をばらまく
            phase1.progress = 2;
            StartCoroutine("Cross_Scattered");
            while (phase1.progress == 2) { yield return null; }
            //真ん中上部に移動、画面を狭める
            Narrow_Screen();
            _controller.Change_Parameter("DashBool1", 1);
            _move.Start_Move(new Vector3(-70, 64f), 0, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            _controller.Change_Parameter("IdleBool", -1);
            yield return new WaitForSeconds(1.0f);
            //下部からレーザー、奇数段、全方位弾
            StartCoroutine(Phase1_Main_Bullet(0));
            yield return new WaitForSeconds(3.0f);
            StartCoroutine(Phase1_Main_Bullet(30f));
            yield return new WaitForSeconds(3.0f);
            //画面広げる
            Spread_Screen();
            yield return new WaitForSeconds(1.0f);
            //初期位置に戻る
            phase1.progress = 1;
            _controller.Change_Parameter("DashBool1", -1);
            _move.Start_Move(phase1.start_Pos, 0, 0.02f);
            yield return new WaitUntil(_move.End_Move);
            _controller.Change_Parameter("IdleBool", 1);
            yield return new WaitForSeconds(3.0f);
        }
    }


    //上を横断しながら弾をばらまく
    private IEnumerator Cross_Scattered() {
        //移動
        _controller.Change_Parameter("DashBool1", -1);
        _move.Start_Move(new Vector3(260f, 64f), -32f, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(0.5f);
        //横断、星弾落とす
        _controller.Change_Parameter("DashBool2", 1);
        _rigid.velocity = new Vector2(-300f, 0);
        while (transform.position.x > -260f) {
            Drop_Star_Bullet();
            yield return new WaitForSeconds(0.1f);
        }
        _rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        //横断、星弾落とす
        _controller.Change_Parameter("DashBool2", -1);
        _rigid.velocity = new Vector2(300f, 0);
        while (transform.position.x < 260f) {
            Drop_Star_Bullet();
            yield return new WaitForSeconds(0.1f);
        }
        _rigid.velocity = Vector2.zero;
        phase1.progress = 3;
    }

    //星弾落とす
    private void Drop_Star_Bullet() {
        GameObject bullet = Instantiate(Resources.Load("Bullet/YellowStarBullet") as GameObject);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().gravityScale = 8f;
        Destroy(bullet, 5.0f);
        UsualSoundManager.Shot_Sound();
    }

    //画面を狭める
    private void Narrow_Screen() {
        GameObject.Find("ScreenFrame").GetComponent<ScreenFrame>().Start_Narrow();
    }
    //画面広げる
    private void Spread_Screen() {
        GameObject.Find("ScreenFrame").GetComponent<ScreenFrame>().Start_Spread();
    }

    //レーザー、奇数弾、全方位弾
    private IEnumerator Phase1_Main_Bullet(float laser_Differ) {
        BulletPoolFunctions _bullet_Pool = GetComponent<BulletPoolFunctions>();
        //レーザー
        GameObject laser = Resources.Load("Bullet/UpperLaser") as GameObject;
        for (int i = -2; i <= 2; i += 1) {
            GameObject bullet = Instantiate(laser);
            bullet.transform.position = new Vector3(Random.Range(55f, 80f) * i -60 - laser_Differ, -140f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.0f);
        //全方位弾、奇数段
        UsualSoundManager.Shot_Sound();
        _bullet_Pool.Set_Bullet_Pool(star_Bullet_Pool[3]);
        _bullet_Pool.Odd_Num_Bullet(20, 18f, 80f, 5.0f);
        yield return new WaitForSeconds(0.4f);
        for(int i = 0; i < 5; i++) {
            _bullet_Pool.Odd_Num_Bullet(3, 12f, 100f-i*5, 5.0f);
            yield return new WaitForSeconds(0.05f);
        }
    }


    //フェーズ2
    public void Phase2() {
        if (phase2.start_Routine) {
            phase2.start_Routine = false;
            //フェーズ1の終了
            StopAllCoroutines();
            _move.StopAllCoroutines();
            _rigid.velocity = new Vector2(0, 0);
            Spread_Screen();
            _controller.Disappear_Back_Design();
            //フェーズ2の開始
            StartCoroutine("Phase2_Routine");
        }
    }
    private IEnumerator Phase2_Routine() {
        //無敵化、移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _controller.Change_Parameter("DashBool1", 1);
        _move.Start_Move(phase2.start_Pos, 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        _controller.Appear_Back_Design(transform.position);
        //使い魔生成
        _controller.Change_Parameter("IdleBool", 1);
        phase2.familiars.SetActive(true);
        phase2.familiars.transform.position = transform.position;
        MarisaFamiliar familiars_Controller = phase2.familiars.GetComponent<MarisaFamiliar>();
        yield return null;
        familiars_Controller.StartCoroutine("Appear");
        //使い魔弾発射
        yield return new WaitForSeconds(1.5f);
        familiars_Controller.Set_Bullet_Pools(star_Bullet_Pool);
        familiars_Controller.Start_Spiral_Bullets();

    }


    //フェーズ3
    public void Phase3() {
        if (phase3.start_Routine) {
            phase3.start_Routine = false;
            //フェーズ2の終了
            StopAllCoroutines();
            phase2.familiars.GetComponent<MarisaFamiliar>().Stop_Spiral_Bullets();
            phase2.familiars.SetActive(false);
            _controller.Disappear_Back_Design();
            //フェーズ3開始
            StartCoroutine("Phase3_Routine");
        }
    }
    private IEnumerator Phase3_Routine() {
        StartCoroutine("Phase2_Routine");
        yield return new WaitForSeconds(3.0f);
        //大型星弾発射
        for(int i = 0; i < 2; i++) {
            phase3.spiral[i] = gameObject.AddComponent<SpiralBulletFunction>();
            phase3.spiral[i].Set_Bullet_Pool(big_Star_Bullet_Pool[i]);
        }
        phase3.spiral[0].Start_Spiral_Bullet(80f, 190, 10f, 0.5f, 8.0f);
        phase3.spiral[1].Start_Spiral_Bullet(80f, -190, -10f, 0.5f, 8.0f);
    }


    //フェーズ4
    public void Phase4() {
        if (phase4.start_Routine) {
            phase4.start_Routine = false;
            //フェーズ3の終了
            StopAllCoroutines();
            phase2.familiars.GetComponent<MarisaFamiliar>().Stop_Spiral_Bullets();
            phase2.familiars.SetActive(false);
            phase3.spiral[0].Stop_Spiral_Bullet();
            phase3.spiral[1].Stop_Spiral_Bullet();
            _controller.Disappear_Back_Design();
            //フェーズ4開始
            StartCoroutine("Phase4_Routine");
        }
    }
    private IEnumerator Phase4_Routine() {
        //無敵化、移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _controller.Change_Parameter("DashBool1", -1);
        _move.Start_Move(phase4.start_Pos, 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parameter("AttackBool", 1);
        _controller.Appear_Back_Design(transform.position);
        yield return new WaitForSeconds(0.5f);
        //星弾で囲む
        phase4.enclosure_Stars.SetActive(true);
        UsualSoundManager.Laser_Sound();
        yield return new WaitForSeconds(3.0f);
        //マスタースパーク
        StartCoroutine("Mini_Master_Spark");
        yield return new WaitForSeconds(20f);
        //フェーズ2終了
        phase4.enclosure_Stars.GetComponent<EnclosureStarsParent>().Disappear();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        GetComponent<BossEnemyController>().life[3] = 1;
    }

    //マスタースパークの生成
    private IEnumerator Mini_Master_Spark() {
        GameObject master_Spark = Resources.Load("Bullet/MiniMasterSpark") as GameObject;
        //テキストから
        TextFileReader text = new TextFileReader();
        text.Read_Text("MiniMasterSparkText");        
        for (int i = 1; i < text.rowLength; i++) {
            yield return new WaitForSeconds(float.Parse(text.textWords[i, 4]));
            GameObject bullet = Instantiate(master_Spark);
            bullet.transform.position = new Vector2(float.Parse(text.textWords[i, 1]), float.Parse(text.textWords[i, 2]) - 24f);
            bullet.transform.Rotate(new Vector3(0, 0, float.Parse(text.textWords[i, 3])));
        }
        yield return new WaitForSeconds(3.929f);
        //円形
        for (float r = 0; r < 360f; r += 10) {
            GameObject bullet = Instantiate(master_Spark);
            bullet.transform.position = new Vector3(Mathf.Cos(r * Mathf.PI / 180), Mathf.Sin(r * Mathf.PI / 180)) * 300f - new Vector3(0, 24f);
            bullet.transform.Rotate(new Vector3(0, 0, -360 + r - 90));
            yield return new WaitForSeconds(0.179f);
        }
    }

}
