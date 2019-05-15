using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEnemy2Controller : EnemyFunction {
    //コンポーネント
    private Rigidbody2D _rigid;

    //初期位置
    private Vector3 default_Pos;

    //速度
    [SerializeField] private float speed;
    //1ループの長さ
    [SerializeField] private float roop_Time_Length;

  

    //Awake
    private void Awake() {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        //初期値代入
        default_Pos = transform.position;
    }


    //起動時
    private void OnEnable() {
        transform.position = default_Pos;
        //再生
        Invoke("Revive", roop_Time_Length);
        //初速
        _rigid.velocity = new Vector2(0, speed);
    }


    //Update
    private void Update() {
        //消滅
        if(default_Pos.y < 0) {
            if(transform.position.y > 160f) {
                gameObject.SetActive(false);
            }
        }
        else {
            if (transform.position.y < -160f) {
                gameObject.SetActive(false);
            }
        }      
    }


    //消滅時
    override protected void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(base.vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPと回復アイテムの生成
        Put_Out_Item();
        //点滅中に消滅したときの対処
        _sprite.color = new Color(1, 1, 1, 1);
        gameObject.SetActive(false);
    }


    //再生
    private void Revive() {
        base.life = 4;
        gameObject.SetActive(true);
    }

}
