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
        public GameObject enclosure_Stars;
    }
    
    //フェーズ3用
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
        public Vector2 start_Pos;
    }

    public Phase1_Status phase1;
    public Phase2_Status phase2;
    public Phase3_Status phase3;

    //コンポーネント
    private MoveBetweenTwoPoints _move;
    private Rigidbody2D _rigid;

    //弾
    private ObjectPool yellow_Star_Bullet_Pool;


    //Awake
    private void Awake() {
        //取得
        _move = GetComponent<MoveBetweenTwoPoints>();
        _rigid = GetComponent<Rigidbody2D>();
    }


    // Use this for initialization
    void Start () {
        //弾のオブジェクトプール
        yellow_Star_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        yellow_Star_Bullet_Pool.CreatePool(Resources.Load("Bullet/PooledBullet/YellowStarBullet") as GameObject, 10);
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
        yield return new WaitForSeconds(1.5f);
        while (true) {
            //上を横断しながら弾をばらまく
            phase1.progress = 2;
            StartCoroutine("Cross_Scattered");
            while (phase1.progress == 2) { yield return null; }
            //真ん中上部に移動、画面を狭める
            Narrow_Screen();
            _move.Start_Move(new Vector3(-70, 64f), 0, 0.012f);
            yield return new WaitUntil(_move.End_Move);
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
            _move.Start_Move(phase1.start_Pos, 0, 0.02f);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(3.0f);
        }
    }


    //上を横断しながら弾をばらまく
    private IEnumerator Cross_Scattered() {
        //移動
        _move.Start_Move(new Vector3(260f, 64f), -32f, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(0.5f);
        //横断、星弾落とす
        _rigid.velocity = new Vector2(-300f, 0);
        while (transform.position.x > -260f) {
            Drop_Star_Bullet();
            yield return new WaitForSeconds(0.1f);
        }
        _rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        //横断、星弾落とす
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
        GameObject bullet = yellow_Star_Bullet_Pool.GetObject();
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().gravityScale = 8f;
        bullet.GetComponent<EnemyBullet>().Delete_Pool_Bullet(5.0f);
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
        _bullet_Pool.Set_Bullet_Pool(yellow_Star_Bullet_Pool);
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
            StopAllCoroutines();
            _move.StopAllCoroutines();
            _rigid.velocity = new Vector2(0, 0);
            Spread_Screen();
            StartCoroutine("Phase2_Routine");
        }
    }
    private IEnumerator Phase2_Routine() {
        //初期設定
        //無敵化、移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        _move.Start_Move(phase2.start_Pos, 0, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(0.5f);
        //星弾で囲む
        phase2.enclosure_Stars.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        //マスタースパーク
        StartCoroutine("Mini_Master_Spark");
        yield return new WaitForSeconds(20f);
        //フェーズ2終了
        phase2.enclosure_Stars.GetComponent<EnclosureStarsParent>().Disappear();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }

    //マスタースパークの生成
    private IEnumerator Mini_Master_Spark() {
        GameObject master_Spark = Resources.Load("Bullet/MiniMasterSpark") as GameObject;
        //テキストから
        TextReader text = new TextReader();
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
