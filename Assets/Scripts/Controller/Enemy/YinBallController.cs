using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinBallController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();

        //登場後の流れ
        StartCoroutine("YinBall_Routine");
    }


    //FixedUpdate
    private void FixedUpdate() {
        _rigid.velocity = new Vector2(-80f, -150f);
        //下まで行ったら消す
        if (transform.position.y < -200f) {
            Destroy(gameObject);
        }
    }


    //登場後の流れ
    private IEnumerator YinBall_Routine() {
        yield return new WaitForSeconds(6f/7f);
        //弾の発射
        BulletFunctions b = GetComponent<BulletFunctions>();
        var bullet = Resources.Load("Bullet/PurpleBullet") as GameObject;
        b.Set_Bullet(bullet);
        b.Odd_Num_Bullet(1, 0, 100f, 7.0f);
        //効果音
        GetComponents<AudioSource>()[1].Play();
    }
}
