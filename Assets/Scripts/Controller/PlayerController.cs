using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //コンポーネント
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private CapsuleCollider2D _collider;
    //オーディオコンポーネント
    private AudioSource jump_Sound;

    //スクリプト
    private ObjectPool _pool;
    private PlayerManager _playerManager;

    //子供
    private GameObject player_Kick;
    private GameObject hit_Decision;
    private GameObject[] options = new GameObject[2];

    //初期値
    private float default_Gravity;
    private float default_Drag;
    private Vector2 default_Collider_Size;
    private Vector2 default_Collider_Offset;

    //移動速度
    private float max_Speed = 150.0f;
    private float acc = 15f;
    private float dec = 0.8f;
    //通常時
    private float DASH_SPEED = 150.0f;
    //飛行時
    private float FLY_SPEED = 120f;

    //ジャンプ
    private float JUMP_SPEED = 300.0f;
    private float JUMP_DEC = 0.5f;

    //ショット
    private float BULLET_SPEED = 500.0f;
    private float time = 0;
    private float SHOT_SPAN = 0.1f;

    //操作可能かどうか
    public bool is_Playable = true;
    //飛行状態かどうか
    private bool is_Fly = false;
    //地面についているかどうか
    public bool is_Ground = true;
    //キックが当たったかどうか
    public bool is_Hit_Kick = false;
    //しゃがみ状態かどうか
    private bool is_Squat = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
        //オーディオコンポーネントの取得
        jump_Sound = GetComponents<AudioSource>()[0];
        //スクリプトの取得
        _pool = GetComponent<ObjectPool>();
        _playerManager = GameObject.Find("CommonScripts").GetComponent<PlayerManager>();

        //子供の取得
        player_Kick = transform.Find("PlayerKick").gameObject;
        hit_Decision = transform.Find("HitDecision").gameObject;
        options[0] = transform.Find("PlayerOption_0").gameObject;
        options[1] = transform.Find("PlayerOption_1").gameObject;

        //初期値の代入
        default_Gravity = _rigid.gravityScale;
        default_Drag = _rigid.drag;
        default_Collider_Size = _collider.size;
        default_Collider_Offset = _collider.offset;

        //オブジェクトプール
        GameObject player_Bullet = Resources.Load("Bullet/PooledBullet/PlayerBullet") as GameObject;
        _pool.CreatePool(player_Bullet, 10);
	}
	

	// Update is called once per frame
	void Update () {

        if (is_Playable) {
            //飛行状態の切り替え
            if(Input.GetKey(KeyCode.LeftShift) && !is_Fly) {
                is_Fly = true;
            }
            else if(!Input.GetKey(KeyCode.LeftShift) && is_Fly) {
                is_Fly = false;
            }
            //飛行状態に伴うステータスの変化
            Change_Fly_Status();      
            
            //移動
            Transition();
            //アニメーション
            Animation();
            //ショット
            Shot();
 
            if (!is_Fly) {
                //ジャンプ
                Jump();
                //空中で動きにくくする
                if (!is_Ground) {
                    acc = 5f;
                    dec = 0.98f;
                }
                else {
                    acc = 10f;
                    dec = 0.8f;
                }
                //しゃがみ
                Squat();
                //キック
                Kick();
            }

        }
	}


    //飛行状態の切り替えに伴うステータスの変化
    private void Change_Fly_Status() {
        //飛行状態切り替えに伴う移動速度の変化
        if (is_Fly && max_Speed !=FLY_SPEED) {
            max_Speed = FLY_SPEED;
        }
        else if(!is_Fly && max_Speed != DASH_SPEED) {
            max_Speed = DASH_SPEED;
        }
        //飛行状態切り替えに伴う重力の変化
        if (is_Fly && _rigid.gravityScale == default_Gravity) {
            _rigid.gravityScale = 0;
        }
        else if (!is_Fly && _rigid.gravityScale != default_Gravity) {
            _rigid.gravityScale = default_Gravity;
        }
        //飛行状態の切り替えに伴う空気抵抗の変化
        if (is_Fly && _rigid.drag == default_Drag) {
            Invoke("Change_Drag", 0.5f);
        }
        else if (!is_Fly && _rigid.drag != default_Drag) {
            _rigid.drag = default_Drag;
        }
        //飛行状態の切り替えに伴う当たり判定の変化
        if(is_Fly && _collider.size.y != default_Collider_Size.x) {
            _collider.size = new Vector2(default_Collider_Size.x, default_Collider_Size.x);
            _collider.offset = default_Collider_Offset;
        }
        else if(!is_Fly && _collider.size.y != default_Collider_Size.y) {
            _collider.size = default_Collider_Size;
            _collider.offset = default_Collider_Offset;
        }
        //当たり判定の表示
        if(is_Fly && !hit_Decision.activeSelf) {
            hit_Decision.SetActive(true);
        }
        else if(!is_Fly && hit_Decision.activeSelf) {
            hit_Decision.SetActive(false);
        }
        //しゃがみの解除
        if (is_Squat && is_Fly) {
            is_Squat = false;
        }
    }
    //空気抵抗を上げる
    private void Change_Drag() {
        _rigid.drag = 3.0f;
    }


    //しゃがみ
    private void Squat() {
        if (is_Ground && Input.GetKey(KeyCode.DownArrow)) {
            Change_Parameter("SquatBool");
            _collider.size = new Vector2(default_Collider_Size.x, default_Collider_Size.x) + new Vector2(0, 0.1f);
            _collider.offset = new Vector2(default_Collider_Offset.x, -9f);
            is_Squat = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            _collider.size = default_Collider_Size;
            _collider.offset = default_Collider_Offset;
            is_Squat = false;
        }
    }


    //移動
    private void Transition() {
        //右
        if (Input.GetKey(KeyCode.RightArrow) && !is_Squat) {
            if (_rigid.velocity.x < max_Speed) {
                _rigid.velocity += new Vector2(acc, 0);
            }
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow) && !is_Squat) {
            if (_rigid.velocity.x > -max_Speed) {
                _rigid.velocity += new Vector2(-acc, 0);
            }
        }
        //減速
        else {
            _rigid.velocity *= new Vector2(dec, 1);
        }
        //飛行時上下移動
        if (is_Fly) {
            //上
            if (Input.GetKey(KeyCode.UpArrow)) {
                if (_rigid.velocity.y < FLY_SPEED) {
                    _rigid.velocity += new Vector2(0, acc);
                }
            }
            //下
            else if (Input.GetKey(KeyCode.DownArrow)) {
                if (_rigid.velocity.y > -FLY_SPEED) {
                    _rigid.velocity += new Vector2(0, -acc);
                }
            }
            //減速
            else {
                _rigid.velocity *= new Vector2(1, dec);
            }
        }
    }


    //ジャンプ
    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Z) && is_Ground) {
            _rigid.velocity += new Vector2(0, JUMP_SPEED);
            jump_Sound.Play();
        }
        if (Input.GetKeyUp(KeyCode.Z) && _rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, JUMP_DEC);
        }
        //しゃがみの解除
        is_Squat = false;
    }


    //キック
    private void Kick() {
        if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.DownArrow)) {
            StartCoroutine("Kick_Routine");
        }
    }
    //キックのモーション
    private IEnumerator Kick_Routine() {
        //操作不能にする
        is_Playable = false;
        //攻撃判定をつける
        player_Kick.SetActive(true);
        //アニメーションの変更
        Change_Parameter("KickBool");
        //地上ならスライディング
        if (is_Ground) {
            _rigid.drag = 3.0f;
            _rigid.velocity = new Vector2(200f * transform.localScale.x, 0);
        }
        //空中ならライダーキック
        else {
            _rigid.velocity = new Vector2(200f * transform.localScale.x, -250f);
        }
        //キックが敵か地面にヒットしたとき(PlayerKickControllerで衝突判定)
        for(float time = 0; time < 0.6f; time += Time.deltaTime) {
            yield return null;
            if (Is_Hit_Kick()) {
                _rigid.velocity = new Vector2(40f * -transform.localScale.x, 180f);
                yield return new WaitForSeconds(0.4f);
                break;
            }
        }
        _rigid.drag = default_Drag;
        //攻撃判定を消す
        player_Kick.SetActive(false);
        //操作可能にする
        is_Playable = true;
    }    
    //キックが当たったかどうか
    private bool Is_Hit_Kick() {
        if (is_Hit_Kick) {
            is_Hit_Kick = false;
            return true;
        }
        return false;
    }


    //ショット
    private void Shot() {
        if (Input.GetKey(KeyCode.X)) {
            if (time < SHOT_SPAN) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                //弾の発射
                for (int i = 0; i < 2; i++) {
                    var bullet = _pool.GetObject();
                    bullet.transform.position = options[i].transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(BULLET_SPEED * transform.localScale.x, 0);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.X)) {
            time = SHOT_SPAN;
        }
    }


    //アニメーション
    private void Animation() {
        //通常時
        if (!is_Fly) {
            //地上
            if (is_Ground) {
                if (Input.GetKey(KeyCode.RightArrow)) {
                    Change_Parameter("DashBool");                
                }
                else if (Input.GetKey(KeyCode.LeftArrow)) {
                    Change_Parameter("DashBool");
                }
                else {
                    Change_Parameter("IdleBool");
                }
            }
            //空中
            else {
                Change_Parameter("JumpBool");
            }
        }

        //飛行時
        else {
            if (Input.GetKey(KeyCode.RightArrow)) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetKey(KeyCode.UpArrow)) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                Change_Parameter("FlyBool");
            }
            else {
                Change_Parameter("FlyIdleBool");
            }
        }

        //向き
        if(Input.GetKey(KeyCode.RightArrow)) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Input.GetKey(KeyCode.LeftArrow)) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    //アニメーション変更
    private void Change_Parameter(string next_Parameter) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("FlyBool", false);
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("FlyIdleBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("KickBool", false);
        _anim.SetBool("SquatBool", false);

        _anim.SetBool(next_Parameter, true);
    }

}
