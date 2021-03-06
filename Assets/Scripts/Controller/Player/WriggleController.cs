﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

//PlayerControllerに飛行とキックを加えたクラス、
public class WriggleController : PlayerController {

    //オーディオ
    private AudioSource kick_Sound;
    private AudioSource fly_Sound;

    //子供
    private GameObject hit_Decision;
    private GameObject player_Kick;

    //飛行状態かどうか
   public bool is_Fly = false;
    private bool can_Fly = true;
    //キックが当たったかどうか
    public bool is_Hit_Kick = false;

    //飛行時間
    private float fly_Time = 0f;

    //初期値
    private float default_Gravity;
    private float default_Drag;


    // Use this for initialization
    new void Awake () {
        base.Awake();
        //オーディオの取得
        kick_Sound = GetComponents<AudioSource>()[1];
        fly_Sound = GetComponents<AudioSource>()[3];
        //子供
        hit_Decision = transform.Find("HitDecision").gameObject;
        player_Kick = transform.Find("PlayerKick").gameObject;
        //初期値の代入
        default_Gravity = _rigid.gravityScale;
        default_Drag = _rigid.drag;
    }
	
	// Update is called once per frame
	void Update () {
        if (base.is_Playable) {
            Transition();
            Animation();
            Change_Fly_Status();
            if (!is_Fly) {
                Jump();
                Squat();
                Kick();
            }
        }
    }


    //移動
    public new void Transition() {
        //通常時
        base.Transition();
        //飛行時上下移動追加
        if (is_Fly) {
            if (Input.GetAxisRaw("Vertical") > 0) {
                if (_rigid.velocity.y < max_Speed) {
                    _rigid.velocity += new Vector2(0, acc);
                }
            }
            else if (Input.GetAxisRaw("Vertical") < 0) {
                if (_rigid.velocity.y > -max_Speed) {
                    _rigid.velocity += new Vector2(0, -acc);
                }
            }
            //減速
            else {
                _rigid.velocity *= new Vector2(1, dec);
            }
        }
    }


    //飛行状態の切り替え
    private void Change_Fly_Status() {
        if (InputManager.Instance.GetKey(MBLDefine.Key.Fly) && !is_Fly && can_Fly) {
            is_Fly = true;
            Fly();
        }
        else if ((!InputManager.Instance.GetKey(MBLDefine.Key.Fly) && is_Fly)) {
            is_Fly = false;
            Quit_Fly();
        }
        //体力切れ
        if (is_Fly) {
            if (fly_Time < 5.0f) {
                fly_Time += Time.deltaTime;
            }
            else {
                StartCoroutine("Fly_Interval_Time");
            }
        }
 
        //しゃがみの解除
        if (is_Squat && is_Fly) {
            is_Squat = false;
        }
    }

    //飛行
    private void Fly() {
        is_Ground = false;
        max_Speed = 110f;  //速度
        acc = 13f;  //加速度
        dec = 0.8f; //減速度
        _rigid.gravityScale = 0;    //重力
        _rigid.drag = 3.0f;    //空気抵抗
        _collider.size = new Vector2(10f, 10f); //当たり判定
        _collider.offset = default_Collider_Offset; //当たり判定
        hit_Decision.SetActive(true);   //当たり判定
        fly_Sound.Play();
    }


    //飛行中止
    private void Quit_Fly() {
        max_Speed = 180f; //速度
        _rigid.gravityScale = default_Gravity;  //重力
        _rigid.drag = default_Drag; //空気抵抗
        _collider.size = default_Collider_Size; //当たり判定
        _collider.offset = default_Collider_Offset; //当たり判定
        hit_Decision.SetActive(false);  //当たり判定
    }


    //体力切れ
    private IEnumerator Fly_Interval_Time() {
        can_Fly = false;
        is_Fly = false;
        Quit_Fly();
        yield return new WaitForSeconds(1.0f);
        can_Fly = true;
    }


    //体力回復(WriggleFootで着地判定時に呼ぶ)
    public IEnumerator Heal_Fly_Time() {
        while(fly_Time > 0) {
            if(is_Fly && !is_Ground) {
                break;
            }
            fly_Time -= Time.deltaTime * 5f;
            yield return null;
        }
    }


    //キック
    private void Kick() {
        if (InputManager.Instance.GetKey(MBLDefine.Key.Shot) && Input.GetAxisRaw("Vertical") < 0) {
            StartCoroutine("Kick_Routine");
        }
    }
    //キックのモーション
    private IEnumerator Kick_Routine() {
        is_Hit_Kick = false;
        //ステータスの変化
        is_Playable = false;
        player_Kick.SetActive(true);
        Change_Parameter("KickBool");
        kick_Sound.Play();
        //地上ならスライディング
        Vector2 velocity = new Vector2();
        if (is_Ground) {
            velocity = new Vector2(200f * transform.localScale.x, 0);
        }
        //空中ならライダーキック
        else {
            velocity = new Vector2(240f * transform.localScale.x, -270f);
        }
        _rigid.velocity = velocity;
        //キックが敵にヒットした時跳ね返る(WriggleKickCollisionで衝突判定)
        for (float time = 0; time < 0.3f; time += 0.015f) {
            if (is_Hit_Kick) {
                is_Hit_Kick = false;
                _rigid.velocity = new Vector2(40f * -transform.localScale.x, 180f);
                yield return new WaitForSeconds(0.2f);
                break;
            }
            _rigid.velocity = new Vector2(velocity.x, _rigid.velocity.y);
            yield return new WaitForSeconds(0.015f);
        }
        //ステータスを戻す
        player_Kick.SetActive(false);
        is_Playable = true;
    }

    public void Stop_Kick() {
        StopCoroutine("Kick_Routine");
        player_Kick.SetActive(false);
        is_Playable = true;
    }


    public void Set_Is_Hit_Kick(bool is_Hit_Kick) {
        this.is_Hit_Kick = is_Hit_Kick;
    }


    //アニメーション
    public override void Animation() {
        //通常時
        if (!is_Fly) {
            //地上
            if (is_Ground) {
                if (Input.GetAxisRaw("Horizontal") > 0) {
                    Change_Parameter("DashBool");
                }
                else if (Input.GetAxisRaw("Horizontal") < 0) {
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
            if (Input.GetAxisRaw("Horizontal") > 0) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetAxisRaw("Horizontal") < 0) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetAxisRaw("Vertical") > 0) {
                Change_Parameter("FlyBool");
            }
            else if (Input.GetAxisRaw("Vertical") < 0) {
                Change_Parameter("FlyBool");
            }
            else {
                Change_Parameter("FlyIdleBool");
            }
        }
        //向き
        if (Input.GetAxisRaw("Horizontal") > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //アニメーション変更
    public override void Change_Parameter(string next_Parameter) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("FlyBool", false);
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("FlyIdleBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("KickBool", false);
        _anim.SetBool("SquatBool", false);

        _anim.SetBool(next_Parameter, true);
    }


    //is_FlyのSetter
    public void Set_Is_Fly(bool is_Fly) {
        this.is_Fly = is_Fly;
    }
    public bool Get_Is_Fly() {
        return is_Fly;
    }

    //fly_TimeのGetter
    public float Get_Fly_Time() {
        return fly_Time;
    }
}
