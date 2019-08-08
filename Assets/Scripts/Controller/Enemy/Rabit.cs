using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabit : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;

    //自機
    private GameObject player;
    
    //ジャンプ
    private float jump_Speed = 180f;
    [SerializeField] private float horizon_Speed = 0;

    private float[] span;
    private float time = 0;

    private int count = 0;

    //横方向への移動開始
    private bool start_Horizon_Jump = false;


    //Awake
    private void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        span = GameObject.FindWithTag("ScriptsTag").GetComponent<Stage4_1Scene>().bgm_Match_Span;
	}
	
	// Update is called once per frame
	void Update () {
        //ジャンプ
        if (time < span[count % span.Length]) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            if (start_Horizon_Jump) {
                _rigid.velocity = new Vector2(horizon_Speed, jump_Speed);
            }
            else {
                _rigid.velocity = new Vector2(0, jump_Speed);
            }
            _anim.SetBool("JumpBool", true);
            count++;
        }

        //横方向への移動開始
        if(player.transform.position.x > transform.position.x - 300f && !start_Horizon_Jump) {
            start_Horizon_Jump = true;
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "GroundTag" || collision.tag == "ThroughGroundTag") {
            _anim.SetBool("JumpBool", false);
            _rigid.velocity = new Vector2(0, _rigid.velocity.y);
        }
    }
}
