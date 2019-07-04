using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEnemyController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        //登場後の流れ
        StartCoroutine("SoulEnemy_Routine");
        UsualSoundManager.Small_Shot_Sound();
    }
	

    //FixedUpdate
    private void FixedUpdate() {
        //加速
        if (_rigid.velocity.x > -200f) {
            _rigid.velocity += new Vector2(-3f, 0);
        }
        //左端に行ったら消す
        if (transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }


    //登場後の流れ
    private IEnumerator SoulEnemy_Routine() {
        yield return new WaitForSeconds(12f/7f);
        //弾の発射
        var bullet = Instantiate(Resources.Load("Bullet/PurpleBullet")) as GameObject;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-140f, 0);
        Destroy(bullet, 5.0f);
        //効果音
        GetComponents<AudioSource>()[1].Play();
    }
}
