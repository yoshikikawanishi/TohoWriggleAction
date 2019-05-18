using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;

    //時間
    private float time = 0;
    //速度
    private float speed = 150f;
    //初期位置
    private float default_Pos_x;
    //移動距離
    [SerializeField] private float moving_Distance = 96f;


    // Use this for initialization
    void Start() {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();

        //初期値代入
        default_Pos_x = transform.position.x;
    }


    // Update is called once per frame
    void Update() {
        //角度の調整
        if (-1f < _rigid.velocity.x && _rigid.velocity.x < 1f) {
            transform.eulerAngles += new Vector3(0, 0, -5f * transform.localScale.x);
        }
        //速度
        //左向き
        if (transform.localScale.x == 1) {
            if (transform.position.x > default_Pos_x - moving_Distance) {
                _rigid.velocity = new Vector2(-speed, _rigid.velocity.y);
            }
            else {
                transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            }
        }
        //右向き
        if(transform.localScale.x == -1) {
            if(transform.position.x < default_Pos_x + moving_Distance) {
                _rigid.velocity = new Vector2(speed, _rigid.velocity.y);
            }
            else {
                transform.localScale = new Vector3(1, transform.localScale.y, 1);
            }
        }
        

    }

}
