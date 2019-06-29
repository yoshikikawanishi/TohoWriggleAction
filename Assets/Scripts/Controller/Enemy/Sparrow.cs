using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparrow : MonoBehaviour {

    //自機、カメラ
    private GameObject player;
    private GameObject main_Camera;
    //コンポーネント
    private Animator _anim;
    private Renderer _renderer;
    private Rigidbody2D _rigid;

    //行動開始
    private bool start_Action = false;


	// Use this for initialization
	void Start () {
        //自機、カメラ
        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.FindWithTag("MainCamera");
        //コンポーネント
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();
        _rigid = GetComponent<Rigidbody2D>();
        //当たり判定を消す
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        _renderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        //自機が通り過ぎたら後ろから登場
        if(Mathf.Abs(player.transform.position.x - transform.position.x) < 32f && !start_Action) {
            start_Action = true;
            StartCoroutine("Sparrow_Action");
        } 
    }


    //動き
    private IEnumerator Sparrow_Action() {
        transform.SetParent(main_Camera.transform);
        transform.position = new Vector3(player.transform.position.x - 150f, transform.position.y);
        _renderer.enabled = true;
        //下降
        _rigid.velocity = new Vector2(150f, 0);
        for(float t = 0; t < 1.0f; t += Time.deltaTime) {
            yield return null;
            transform.position += new Vector3(0, -3f + t*3) * Time.timeScale;
        }
        //直進
        while (transform.position.x < player.transform.position.x + 360f) {
            yield return null;
        }
        transform.SetParent(null);
        //突進
        _anim.SetBool("AttackBool", true);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        transform.position = player.transform.position + new Vector3(360f, 0);
        _rigid.velocity = new Vector2(-200f, 0);
        yield return new WaitForSeconds(2.5f);
        //上昇
        for(float t = 0; transform.position.y < 140f; t += Time.deltaTime) {
            yield return null;
            transform.position += new Vector3(0, 1f + t) * Time.timeScale;
        }
        //通り過ぎたら消す
        Destroy(gameObject);
    }

    
}
