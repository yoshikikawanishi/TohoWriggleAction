using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private AudioSource shot_Sound;
    //スクリプト
    private BossEnemyController __BossEnemyController;
    private MoveBetweenTwoPoints _move;
    private BulletFunctions _bulletFunction;

    //自機
    private GameObject player;

    //子供
    private GameObject back_Design;

    //固有弾
    [SerializeField] private GameObject scales_Bullet;
    [SerializeField] private GameObject rice_Bullets;

    //戦闘開始
    public bool start_Battle = false;

    /* フェーズ1用 */
    private bool start_Routine1 = false;

    /* フェーズ2用 */
    private bool start_Routine2 = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _anim = GetComponent<Animator>();
        shot_Sound = GetComponents<AudioSource>()[0];
        //スクリプトの取得
        __BossEnemyController = GetComponent<BossEnemyController>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _bulletFunction = GetComponent<BulletFunctions>();

        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");

        //子供の取得
        back_Design = transform.GetChild(0).gameObject;
        back_Design.transform.SetParent(null);

	}
	

	// Update is called once per frame
	void Update () {
        if (start_Battle) {
            switch (__BossEnemyController.Get_Now_Phase()) {
                case 1: Phase1(); break;
                case 2: Phase2(); break;
            }
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
        //バックデザイン
        back_Design.SetActive(true);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        //移動
        _move.Start_Move(new Vector3(110f, -16f, 0), 0, 0.03f);
        yield return new WaitUntil(_move.End_Move);
        while (__BossEnemyController.Get_Now_Phase() == 1) {
            //弾の発射
            Change_Parameter("AttackBool");
            _bulletFunction.Set_Bullet(scales_Bullet);
            for (int i = 0; i < 2; i++) {
                Shoot_Scales_Bullet();
                //小移動
                Move(32f, 0.01f);
                Change_Parameter("AttackBool");
                yield return new WaitUntil(_move.End_Move);
            }
            //移動
            _move.Start_Move(new Vector3(-110f * transform.localScale.x, -16f, 0), -75f, 0.01f);
            Change_Parameter("DashBool");
            yield return new WaitUntil(_move.End_Move);
            //向きの変更
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            Change_Parameter("AttackBool");
            yield return new WaitForSeconds(1.0f);
        }
    }


    //鱗粉弾の発射
    private void Shoot_Scales_Bullet() {
        for (int i = 0; i < 15; i++) {
            Vector2 v = new Vector2(Random.Range(-200f, 200f), Random.Range(50f, 200f));
            _bulletFunction.Shoot_Bullet(v, 5.0f);
        }
        //エフェクト
        var effect = Instantiate(Resources.Load("Effect/LarvaScalesEffect")) as GameObject;
        effect.transform.position = transform.position;
        shot_Sound.Play();
        Destroy(effect, 1.5f);
    }


    //フェーズ2
    private void Phase2() {
        if (!start_Routine2) {
            start_Routine2 = true;
            //フェーズ1の中止
            StopCoroutine("Phase1_Routine");
            _move.StopAllCoroutines();
            back_Design.SetActive(false);
            //フェーズ2
            StartCoroutine("Phase2_Routine");
        }
    }


    //フェーズ2のルーチン
    private IEnumerator Phase2_Routine() {
        yield return new WaitForSeconds(1.0f);
        //移動
        Become_Invincible();
        _move.Start_Move(new Vector3(160f, -32f), 32f, 0.1f);
        Change_Parameter("DashBool");
        yield return new WaitUntil(_move.End_Move);
        Release_Invincible();
        //向き
        transform.localScale = new Vector3(1, 1, 1);
        //バックデザイン
        back_Design.SetActive(true);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        //弾の発射
        int roop_Count = 0;
        while (__BossEnemyController.Get_Now_Phase() == 2) {
            Change_Parameter("AttackBool");
            //緑米弾
            _bulletFunction.Set_Bullet(rice_Bullets);
            float player_Angle = Angle_To_Player();
            for (int i = 0; i < 8; i++) {
                Shoot_Rice_Bullets(player_Angle, i);
                UsualSoundManager.Small_Shot_Sound();
                yield return new WaitForSeconds(0.3f);
            }
            //自機狙い赤弾
            _bulletFunction.Set_Bullet(Resources.Load("Bullet/RedBullet") as GameObject);
            _bulletFunction.Odd_Num_Bullet(1, 0, 70f, 8.0f);
            shot_Sound.Play();
            roop_Count++;
            if(roop_Count % 3 == 0) {
                yield return new WaitForSeconds(1.0f);
                //鱗粉弾
                _bulletFunction.Set_Bullet(scales_Bullet);
                Shoot_Scales_Bullet();
                yield return new WaitForSeconds(3.0f);
            }
            //小移動
            else {
                Move(32f, 0.005f);
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


    //アニメーションの変更
    public void Change_Parameter(string changeBool) {
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("DashBool", false);
        _anim.SetBool("AttackBool", false);

        _anim.SetBool(changeBool, true);
    }


    //移動
    private void Move(float length, float speed) {
        Change_Parameter("DashBool");
        _move.Start_Random_Move(length, speed);
    }

}
