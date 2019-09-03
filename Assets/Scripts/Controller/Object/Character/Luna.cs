using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luna : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private Renderer _renderer;

    float time = 0;
    //転んだかどうか
    private bool is_Falled = false;


	// Use this for initialization
	void Start () {    
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();
    }

    //Update
    private void Update() {
        if (!is_Falled && _renderer.isVisible) {
            if (time < 2.0f) {
                time += Time.deltaTime;
                transform.position += new Vector3(0.3f, 0) * transform.localScale.x * Time.timeScale;
            }
            else {
                time = 0;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(!is_Falled && (collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag")) {
            is_Falled = true;
            Damaged();
        }
    }


    //被弾
    private void Damaged() {
        //点の発射
        for (int i = 0; i < 50; i++) {
            var score = Instantiate(Resources.Load("Score")) as GameObject;
            score.transform.position = transform.position;
            score.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-150f, 150f), Random.Range(400f, 600f));
        }
        //アニメーション変更
        _renderer.sortingOrder = -1;
        _anim.SetBool("FallBool", true);
    }

}
