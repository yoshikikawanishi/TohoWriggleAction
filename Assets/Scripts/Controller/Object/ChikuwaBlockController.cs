using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChikuwaBlockController : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;

    //自機が乗っている時間
    private float stay_Time = 0;

    //落ち始めのフラグ
    private bool start_Drop_Flag = false;


    //start
    private void Start() {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            //色を変える
            _sprite.color = new Color(1, 0.7f, 0.7f);
        }
        //地面をすり抜け
        else if (collision.tag == "GroundTag") {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            //色を戻す
            _sprite.color = new Color(1, 1, 1);
        }    
        //地面のすり抜けをなくす
        else if(collision.tag == "GroundTag") {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }


    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            if (stay_Time < 1.0f) {
                stay_Time += Time.deltaTime;
            }
            else if (!start_Drop_Flag) {
                start_Drop_Flag = true;
                StartCoroutine("Drop");
            }
        }
    }


    //落下
    private IEnumerator Drop() {
        while(transform.position.y > -160f) {
            transform.position -= new Vector3(0, 1f, 0);
            yield return null;
        }
        Destroy(gameObject);
    }

}
