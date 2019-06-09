using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//横移動、ジャンプ、しゃがみ、をできるクラス
public abstract class PlayerController : MonoBehaviour {


    //コンポーネント
    protected Rigidbody2D _rigid;
    protected Animator _anim;
    //オーディオコンポーネント
    private AudioSource jump_Sound;
    
    //子供
    protected CapsuleCollider2D _collider;  

    //初期値  
    protected Vector2 default_Collider_Size;
    protected Vector2 default_Collider_Offset;

    //移動速度
    protected float max_Speed = 180.0f;
    protected float acc = 13f;
    protected float dec = 0.8f;

    //操作可能かどうか
    protected bool is_Playable = true;
    //地面についているかどうか
    public bool is_Ground = true;
    //しゃがみ状態かどうか
    protected bool is_Squat = false;


	// Use this for initialization
	protected void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        //オーディオコンポーネントの取得
        jump_Sound = GetComponents<AudioSource>()[0];

        //子供の取得
        _collider = GetComponentInChildren<CapsuleCollider2D>();
        
        //初期値代入    
        default_Collider_Size = _collider.size;
        default_Collider_Offset = _collider.offset;

	}
	

	// Update is called once per frame
	void Update () {
        if (is_Playable) {             
            Transition();
            Animation();
            Jump();           
            Squat();
        }
	} 


    //しゃがみ
    protected void Squat() {
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
    public void Transition() {
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
    }


    //ジャンプ
    protected void Jump() {
        if (Input.GetKey(KeyCode.Z) && is_Ground) {
            _rigid.velocity = new Vector2(_rigid.velocity.x, 320f);
            jump_Sound.Play();
            is_Ground = false;
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


    //アニメーション
    public abstract void Animation();

    //アニメーション変更
    public abstract void Change_Parameter(string next_Parameter);


    //is_PlayableのSetter
    public void Set_Playable(bool is_Playable) {
        this.is_Playable = is_Playable;
    }
    //Getter
    public bool Get_Playable() {
        return is_Playable;
    }


    //重力
    public void Set_Gravity(float gravity) {
        _rigid.gravityScale = gravity;
    }



}
