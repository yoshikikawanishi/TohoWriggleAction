using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;
    //オーディオコンポーネント
    private AudioSource jump_Sound;
    private AudioSource kick_Sound;
    
    //子供
    private GameObject player_Kick;
    private CapsuleCollider2D _collider;
    private GameObject hit_Decision;

    //初期値
    private float default_Gravity;
    private float default_Drag;
    private Vector2 default_Collider_Size;
    private Vector2 default_Collider_Offset;

    //移動速度
    private float max_Speed = 180.0f;
    private float acc = 13f;
    private float dec = 0.8f;

    //操作可能かどうか
    private bool is_Playable = true;
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
        _anim = GetComponent<Animator>();
        //オーディオコンポーネントの取得
        jump_Sound = GetComponents<AudioSource>()[0];
        kick_Sound = GetComponents<AudioSource>()[1];

        //子供の取得
        player_Kick = transform.Find("PlayerKick").gameObject;
        _collider = GetComponentInChildren<CapsuleCollider2D>();
        hit_Decision = transform.Find("HitDecision").gameObject;

        //初期値の代入
        default_Gravity = _rigid.gravityScale;
        default_Drag = _rigid.drag;
        default_Collider_Size = _collider.size;
        default_Collider_Offset = _collider.offset;

	}
	

	// Update is called once per frame
	void Update () {

        if (is_Playable) {
            Change_Fly_Status();              
            Transition();
            Animation();

            if (!is_Fly) {
                Jump();           
                Squat();
                Kick();
            }

        }

	}


    //飛行状態の切り替え
    private void Change_Fly_Status() {
        if(Input.GetKey(KeyCode.LeftShift) && !is_Fly) {
            is_Fly = true;
            max_Speed = 110f;  //速度
            acc = 13f;  //加速度
            dec = 0.8f; //減速度
            _rigid.gravityScale = 0;    //重力
            Invoke("Change_Drag", 0.1f);    //空気抵抗
            _collider.size = new Vector2(default_Collider_Size.x, default_Collider_Size.x); //当たり判定
            _collider.offset = default_Collider_Offset; //当たり判定
            hit_Decision.SetActive(true);   //当たり判定
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && is_Fly) {
            is_Fly = false;
            max_Speed = 180f; //速度
            _rigid.gravityScale = default_Gravity;  //重力
            _rigid.drag = default_Drag; //空気抵抗
            _collider.size = default_Collider_Size; //当たり判定
            _collider.offset = default_Collider_Offset; //当たり判定
            hit_Decision.SetActive(false);  //当たり判定
        }
        //しゃがみの解除
        if (is_Squat && is_Fly) {
            is_Squat = false;
        }
    }

    //空気抵抗を上げる
    private void Change_Drag() {
        if (is_Fly) {
            _rigid.drag = 3.0f;
        }
    }


    //しゃがみ
    private void Squat() {
        if (is_Ground && Input.GetKey(KeyCode.DownArrow)) {
            Change_Parameter("SquatBool");
            _collider.size = new Vector2(default_Collider_Size.x, default_Collider_Size.x) + new Vector2(0, 0.1f);
            _collider.offset = new Vector2(default_Collider_Offset.x, -8f);
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
        //左右移動
        if (Input.GetKey(KeyCode.RightArrow) && !is_Squat) {
            if (_rigid.velocity.x < max_Speed) {
                _rigid.velocity += new Vector2(acc, 0);
            }
        }
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
            if (Input.GetKey(KeyCode.UpArrow)) {
                if (_rigid.velocity.y < max_Speed) {
                    _rigid.velocity += new Vector2(0, acc);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
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


    //ジャンプ
    private void Jump() {
        if (Input.GetKeyDown(KeyCode.Z) && is_Ground) {
            _rigid.velocity += new Vector2(0, 350f);
            jump_Sound.Play();
        }
        if (Input.GetKeyUp(KeyCode.Z) && _rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, 0.5f);
        }
        //空中で動きにくくする
        if (!is_Ground) {
            acc = 5f;
            dec = 0.98f;
        }
        else {
            acc = 13f;
            dec = 0.8f;
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
        //ステータスの変化
        is_Playable = false;   
        player_Kick.SetActive(true);    
        Change_Parameter("KickBool");
        kick_Sound.Play();
        //地上ならスライディング
        if (is_Ground) {
            _rigid.drag = 3.0f;
            _rigid.velocity = new Vector2(200f * transform.localScale.x, 0);
        }
        //空中ならライダーキック
        else {
            _rigid.velocity = new Vector2(200f * transform.localScale.x, -250f);
        }
        //キックが敵にヒットした時跳ね返る(PlayerKickControllerで衝突判定)
        for(float time = 0; time < 0.3f; time += Time.deltaTime) {            
            if (Is_Hit_Kick()) {
                _rigid.velocity = new Vector2(40f * -transform.localScale.x, 180f);
                yield return new WaitForSeconds(0.2f);
                break;
            }
            yield return null;
        }
        //ステータスを戻す
        _rigid.drag = default_Drag;
        player_Kick.SetActive(false);
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
    public void Change_Parameter(string next_Parameter) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("FlyBool", false);
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("FlyIdleBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("KickBool", false);
        _anim.SetBool("SquatBool", false);

        _anim.SetBool(next_Parameter, true);
    }


    //is_PlayableのSetter
    public void Set_Playable(bool is_Playable) {
        this.is_Playable = is_Playable;
    }
    //Getter
    public bool Get_Playable() {
        return is_Playable;
    }

    //is_FlyのSetter
    public void Set_Is_Fly(bool is_Fly) {
        this.is_Fly = is_Fly;
    }
    public bool Get_Is_Fly() {
        return is_Fly;
    }
}
