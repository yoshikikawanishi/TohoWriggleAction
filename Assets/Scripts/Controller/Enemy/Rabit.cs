using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabit : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;

    //ジャンプ
    private float jump_Speed = 180f;
    [SerializeField] private float horizon_Speed = 0;

    private float[] span;
    private float time = 0;

    private int count = 0;


    //Awake
    private void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        span = GameObject.FindWithTag("ScriptsTag").GetComponent<Stage4_1Scene>().bgm_Match_Span;
	}
	
	// Update is called once per frame
	void Update () {
		if(time < span[count % span.Length]) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            _rigid.velocity = new Vector2(horizon_Speed, jump_Speed);
            _anim.SetBool("JumpBool", true);
            count++;
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "GroundTag") {
            _anim.SetBool("JumpBool", false);
        }
    }
}
