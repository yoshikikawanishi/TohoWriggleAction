using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWolf : MonoBehaviour {

    private GameObject player;

    private Rigidbody2D _rigid;
    private Renderer _renderer;

    public ParticleSystem transform_Effect;

    private float default_X;
    private float move_Speed = 2f;
    private float move_Length = 80f;
    

    //Awake
    private void Awake() {
        //初期位置
        default_X = transform.position.x;
        transform.position += new Vector3(Random.Range(-32f, 32f), 0);
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        transform_Effect = transform.Find("TransformEffect").GetComponent<ParticleSystem>();
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }


    // Use this for initialization
    void Start () {
       
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
    public void Transform_Wolf() {
        GetComponent<Enemy>().Set_Is_Invincible(true);
        StopCoroutine("Shot");
        //変身
        GetComponent<Animator>().SetBool("TransformBool", true);
        GetComponent<CircleCollider2D>().radius = 20f;
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
        while (true) {
            Look_Player();
            yield return new WaitForSeconds(1.1f);
            _rigid.velocity = transform.up * 300f;
            Rush_Effect();
            while (_renderer.isVisible) {
                yield return new WaitForSeconds(0.016f);
            }
            _rigid.velocity = new Vector2(0, 0);
            //画面外の適当な場所にワープ
            Move_Along_Outframe();
        }
    }


    //画面外の適当な場所にワープ
    private void Move_Along_Outframe() {
        int direction = Random.Range(0, 3);
        Vector2 pos = new Vector2();
        switch (direction) {
            case 0: pos = new Vector2(-250f, Random.Range(-160f, 150f)); break;
            case 1: pos = new Vector2(250f, Random.Range(-160f, 150f)); break;
            case 2: pos = new Vector2(Random.Range(-250f, 300f), 150f); break;
        }
        transform.position = pos;
    }

    
    //突進エフェクト
    private void Rush_Effect() {
        var effect = transform.GetChild(1);
        if(transform.localScale.x == 1) {
            effect.localRotation = Quaternion.AngleAxis(90, new Vector3(0, 0, 1));
        }
        else {
            effect.localRotation = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
        }
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
    }
    


    
    
}
