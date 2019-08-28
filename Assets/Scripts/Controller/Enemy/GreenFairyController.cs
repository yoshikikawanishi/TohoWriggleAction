using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFairyController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private BulletFunctions _bulletFunc;

    //自機
    private GameObject player;

    //時間
    private float time = 1.8f;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        _bulletFunc = gameObject.AddComponent<BulletFunctions>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        
        //弾のセット
        GameObject bullet = Resources.Load("Bullet/GreenBullet") as GameObject;
        _bulletFunc.Set_Bullet(bullet);
    }
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 540f) {
            //移動
            _rigid.velocity = new Vector2(-10f, _rigid.velocity.y);
            //ショット
            if (time < 2.0f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                _bulletFunc.Turn_Shoot_Bullet(70f, 150f, 7.0f);
                UsualSoundManager.Shot_Sound();
            }
        }
        //下に落ちたとき消す
        if(transform.position.y < -160f) {
            Destroy(gameObject);
        }
	}
}
