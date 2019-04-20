using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    //種類番号
    [SerializeField] private int kind_Num = 0;

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;

    //自機
    private GameObject player;


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");

        switch (kind_Num) {
            case 1: SoulEnemy_Start(); break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}


    //FixedUpdate
    private void FixedUpdate() {
        switch (kind_Num) {
            case 1: SoulEnemy(); break;
        }
    }



    //SoulEnemy
    private void SoulEnemy_Start() {
        StartCoroutine("SoulEnemy_Routine");
    }
    private void SoulEnemy() {
        if (_rigid.velocity.x > -200f) {
            _rigid.velocity += new Vector2(-3f, 0);

        }
        //左端に行ったら消す
        if (transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }
    private IEnumerator SoulEnemy_Routine() {
        yield return new WaitForSeconds(1.5f);
        //弾の発射

    }
}
