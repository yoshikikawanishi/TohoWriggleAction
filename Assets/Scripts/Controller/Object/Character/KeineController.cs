using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineController : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    
    //イベント中キャッチされたかどうか
    private bool is_Catched = false;

    //Awake
    private void Awake() {
        _anim = GetComponent<Animator>();
    }



    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" && this.enabled) {
            is_Catched = true;
        }
    }

    //イベント中キャッチされたかどうか
    public bool Get_Is_Catched() {
        return is_Catched;
    }


    //アニメーション変更
    public void Change_Parameter(bool is_Drop) {
        if (is_Drop) {
            _anim.SetBool("DropBool", true);
        }
        else {
            _anim.SetBool("DropBool", false);
        }
    }

}
