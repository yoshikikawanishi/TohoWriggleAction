using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlownEnemy : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Renderer _renderer;

    private bool start_Action = false;


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        //画面内に入ったら動き出す
        if (_renderer.isVisible && !start_Action) {
            start_Action = true;
            StartCoroutine("Shot");
            _rigid.velocity = new Vector2(-37f * transform.localScale.x, 0);
        }
        //左端に行ったら消す
        if (transform.position.x < -320f) {
            Destroy(gameObject);
        }
    }

    //攻撃
    private IEnumerator Shot() {
        yield return new WaitForSeconds(1.0f);
        BulletFunctions _bullet = gameObject.AddComponent<BulletFunctions>();
        GameObject bullet = Resources.Load("Bullet/RedBullet") as GameObject;
        _bullet.Set_Bullet(bullet);
        _bullet.Odd_Num_Bullet(1, 0, 100f, 7.0f);
        UsualSoundManager.Shot_Sound();
    }
}
