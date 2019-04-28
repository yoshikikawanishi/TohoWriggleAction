using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    //スクリプト
    private BossFunction _bossFunction;
    private MoveBetweenTwoPoints _move;
    private BulletFunctions _bulletFunction;

    //自機
    private GameObject player;

    //固有弾
    [SerializeField] private GameObject scales_Bullet;
    [SerializeField] private GameObject rice_Bullets;

    /* フェーズ1用 */
    private bool start_Routine1 = false;

    /* フェーズ2用 */
    private bool start_Routine2 = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        //スクリプトの取得
        _bossFunction = GetComponent<BossFunction>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _bulletFunction = GetComponent<BulletFunctions>();

        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");

        //戦闘開始
        _bossFunction.Set_Now_Phase(1);

	}
	

	// Update is called once per frame
	void Update () {

        switch (_bossFunction.Get_Now_Phase()) {
            case 1: Phase1(); break;
            case 2: Phase2(); break;
        }

        //クリア
        if (_bossFunction.Clear_Trigger()) {
            StopAllCoroutines();
        }
	}


    //フェーズ1
    private void Phase1() {
        if (!start_Routine1) {
            start_Routine1 = true;
            StartCoroutine("Phase1_Routine");
        }
    }
    
    
    //フェーズ1のルーチン
    private IEnumerator Phase1_Routine() {
        //移動
        _move.Set_Status(0, 0.03f);
        _move.StartCoroutine("Move_Two_Points", new Vector3(140f, 16f, 0));
        yield return new WaitUntil(_move.End_Move);
        while (_bossFunction.Get_Now_Phase() == 1) {
            //弾の発射
            _bulletFunction.Set_Bullet(scales_Bullet);
            for (int i = 0; i < 3; i++) {
                Shoot_Scales_Bullet(); 
                yield return new WaitForSeconds(1.0f);
            }
            //移動
            _move.Set_Status(-75f, 0.01f);
            _move.StartCoroutine("Move_Two_Points", new Vector3(-140f * transform.localScale.x, 16f, 0));
            yield return new WaitUntil(_move.End_Move);
            //向きの変更
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            yield return new WaitForSeconds(1.0f);
        }
    }


    //鱗粉弾の発射
    private void Shoot_Scales_Bullet() {
        for (int i = 0; i < 15; i++) {
            Vector2 v = new Vector2(Random.Range(-200f, 200f), Random.Range(50f, 200f));
            _bulletFunction.Shoot_Bullet(v, 5.0f);
        }
    }


    //フェーズ2
    private void Phase2() {
        if (!start_Routine2) {
            start_Routine2 = true;
            StopCoroutine("Phase1_Routine");
            _move.StopAllCoroutines();
            StartCoroutine("Phase2_Routine");
        }
    }


    //フェーズ2のルーチン
    private IEnumerator Phase2_Routine() {
        yield return new WaitForSeconds(1.0f);
        //移動
        Become_Invincible();
        _move.Set_Status(0, 0.02f);
        _move.StartCoroutine("Move_Two_Points", new Vector3(150f, 0));
        yield return new WaitUntil(_move.End_Move);
        Release_Invincible();
        //向き
        transform.localScale = new Vector3(1, 1, 1);
        //弾の発射
        int roop_Count = 0;
        while (_bossFunction.Get_Now_Phase() == 2) {
            //緑米弾
            _bulletFunction.Set_Bullet(rice_Bullets);
            float player_Angle = Angle_To_Player();
            for (int i = 0; i < 8; i++) {
                Shoot_Rice_Bullets(player_Angle, i);
                yield return new WaitForSeconds(0.2f);
            }
            //自機狙い赤弾
            _bulletFunction.Set_Bullet(Resources.Load("Bullet/RedBullet") as GameObject);
            for (int j = 0; j < 15; j++) {
                _bulletFunction.Odd_Num_Bullet(1, 0, 70f, 8.0f);
            }
            //鱗粉弾
            roop_Count++;
            if(roop_Count % 3 == 0) {
                _bulletFunction.Set_Bullet(scales_Bullet);
                Shoot_Scales_Bullet();
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    
    //緑コメ弾(4方向に出す)
    private void Shoot_Rice_Bullets(float player_Angle, int i) {
        float angle = player_Angle + 100f + i * 10f;
        _bulletFunction.Turn_Shoot_Bullet(120f, angle, 7.0f);
        angle = player_Angle + -100f - i * 10f;
        _bulletFunction.Turn_Shoot_Bullet(120f, angle, 7.0f);
        angle = player_Angle + 50f + i * 15f;
        _bulletFunction.Turn_Shoot_Bullet(120f, angle, 7.0f);
        angle = player_Angle + -50f - i * 15f;
        _bulletFunction.Turn_Shoot_Bullet(120f, angle, 7.0f);
    }


    //自機との角度を返す
    private float Angle_To_Player() {
        Vector2 dif = player.transform.position - transform.position;
        // ラジアン
        float radian = Mathf.Atan2(dif.y, dif.x);
        // 角度
        float degree = radian * Mathf.Rad2Deg;
        return degree + 180f;
    }


    //無敵化
    private void Become_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }

    //無敵解除
    private void Release_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }

}
