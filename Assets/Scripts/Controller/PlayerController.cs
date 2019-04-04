using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //コンポーネント
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private Animator _anim;
    //オーディオコンポーネント
    private AudioSource jump_Sound;

    //初期値
    private Color default_Color;
    private Vector3 default_Scale;
    private float default_Gravity;
    private float default_Drag;

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

    //操作可能かどうか
    private bool is_Playable = true;
    //飛行状態かどうか
    private bool is_Fly = false;
    //地面についているかどうか
    public bool is_Ground = true;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        //初期値の代入
        default_Color = _sprite.color;
        default_Scale = transform.localScale;
        default_Gravity = _rigid.gravityScale;
        default_Drag = _rigid.drag;
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
    }
    //空気抵抗を上げる
    private void Change_Drag() {
        _rigid.drag = 3.0f;
    }


    //移動
    private void Transition() {
        //右
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (_rigid.velocity.x < max_Speed) {
                _rigid.velocity += new Vector2(acc, 0);
            }
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow)) {
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


    //キック
    private void Kick() {
        if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.DownArrow)) {
            StartCoroutine("Kick_Routine");
            Debug.Log("AAA!");
        }
    }
    //キックのモーション
    private IEnumerator Kick_Routine() {
        //操作不能にする
        is_Playable = false;
        //攻撃判定をつける

        //アニメーションの変更
        Change_Parameter("KickBool");
        //地上ならスライディング
        if (is_Ground) {
            _rigid.drag = 3.0f;
            _rigid.velocity = new Vector2(200f * transform.localScale.x, 0);
            yield return new WaitForSeconds(0.5f);
        }
        //空中ならライダーキック
        else {
            _rigid.velocity = new Vector2(170f * transform.localScale.x, -150f);
            yield return new WaitUntil(Grounded);
            _rigid.velocity = new Vector2(10f * transform.localScale.x, 130f);
        }
        //硬直時間
        yield return new WaitForSeconds(0.1f);
        _rigid.drag = default_Drag;
        //攻撃判定を消す

        //操作可能にする
        is_Playable = true;
    }
    //着地判定
    private bool Grounded() {
        if (is_Ground) {
            return true;
        }
        return false;
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
        if(Input.GetKey(KeyCode.RightArrow) && transform.localScale.x < 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Input.GetKey(KeyCode.LeftArrow) && transform.localScale.x > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    //ジャンプ
    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Z) && is_Ground) {
            _rigid.velocity += new Vector2(0, JUMP_SPEED);
        }
        if (Input.GetKeyUp(KeyCode.Z) && _rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, JUMP_DEC);
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

        _anim.SetBool(next_Parameter, true);
    }


}
