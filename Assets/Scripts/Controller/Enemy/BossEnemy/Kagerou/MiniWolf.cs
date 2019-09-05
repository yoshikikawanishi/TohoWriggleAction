using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWolf : MonoBehaviour {

    private GameObject player;
    private GameObject kagerou;

    private BossEnemyController boss_Controller;
    private Rigidbody2D _rigid;

    public ParticleSystem transform_Effect;

    private float default_X;
    private float move_Speed = 2f;
    private float move_Length = 80f;

    private int mini_Kagerous_Num = 6;
    
    private bool is_Hit_Wall = false;
    

    //Awake
    private void Awake() {
        //初期位置
        default_X = transform.position.x;
        transform.position += new Vector3(Random.Range(-32f, 32f), 0);
    }


    // Use this for initialization
    void Start () {
        //取得
        boss_Controller = GameObject.Find("Kagerou").GetComponent<BossEnemyController>();
        player = GameObject.FindWithTag("PlayerTag");
        kagerou = GameObject.Find("Kagerou");
        transform_Effect = transform.Find("TransformEffect").GetComponent<ParticleSystem>();
        _rigid = GetComponent<Rigidbody2D>();
    }


    //OnEnable
    private void OnEnable() {
        //ショット
        StartCoroutine("Shot");
    }


    //ちび移動
    public void Transition() {
        if(transform.localScale.x == 1) {
            transform.position += new Vector3(-move_Speed, 0) * Time.timeScale;
            if(transform.position.x < default_X - move_Length) {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if(transform.localScale.x == -1) {
            transform.position += new Vector3(move_Speed, 0) * Time.timeScale;
            if (transform.position.x > default_X + move_Length) {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    //ちびショット
    private IEnumerator Shot() {
        BulletFunctions _bullet = GetComponent<BulletFunctions>();
        _bullet.Set_Bullet(Resources.Load("Bullet/PurpleBullet") as GameObject);
        float t = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(t);
        while (true) {
            _bullet.Odd_Num_Bullet(1, 0, 100f, 5.0f);
            yield return new WaitForSeconds(2.0f);
        }
    }


    //狼に変身
    public IEnumerator Transform_Wolf() {
        GetComponent<Enemy>().Set_Is_Invincible(true);
        StopCoroutine("Shot");
        //エフェクト
        kagerou.GetComponent<KagerouController>().Roar();
        transform_Effect.Play();
        yield return new WaitForSeconds(1.0f);
        //変身
        GetComponent<Animator>().SetBool("TransformBool", true);
        GetComponent<CircleCollider2D>().radius = 20f;
        Look_Player();
    }



    //自機の方向を向く
    public void Look_Player() {
        if (transform.position.x > player.transform.position.x) {    //右をむく
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(GetComponent<LookAtSlerp>() == null) {
            gameObject.AddComponent<LookAtSlerp>();
        }
        GetComponent<LookAtSlerp>().Start_LookAt_Routine(player.transform.position, Vector2.up, 1.0f);
    }


    //突進
    public IEnumerator Rush() {
        Look_Player();
        yield return new WaitForSeconds(1.5f);
        //初速
        transform.Rotate(new Vector3(0, 0, Random.Range(-45f, 45f)));
        _rigid.velocity = Vector2.up * 200f;
        yield return new WaitForSeconds(0.5f);
        //自機をホーミング
        for (float t = 0; t < 1.0f; t++) {
            transform.LookAt2D(player.transform, Vector2.up);
            _rigid.velocity = (Vector2)transform.up * 6f;
            float dirVelocity = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(dirVelocity - 90f, new Vector3(0, 0, 1));
            yield return new WaitForSeconds(0.016f);
        }
    }

    


    //壁との衝突検知
    public bool Is_Hit_Wall() {
        if(Mathf.Abs(transform.position.x) > 220f || Mathf.Abs(transform.position.y) > 120f) {
            _rigid.velocity = new Vector2(0, 0);
            StopCoroutine(Rush());
            return true;
        }
        return false;
    }
    
}
