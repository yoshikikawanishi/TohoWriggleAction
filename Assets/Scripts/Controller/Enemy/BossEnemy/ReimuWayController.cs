using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuWayController : MonoBehaviour {

    //オブジェクトプール
    private ObjectPool _pool;

    //ショットを打つかどうか
    private bool is_Shot_Bullet;

    //ショットスパン
    private float shot_Span = 0.2f;
    private float shot_Time = 0;


	// Use this for initialization
	void Start () {
        //オブジェクトプール
        _pool = gameObject.AddComponent<ObjectPool>();
        GameObject reimu_Bullet = Resources.Load("Bullet/PooledBullet/ReimuTalismanBullet") as GameObject;
        _pool.CreatePool(reimu_Bullet, 10);
	}

    // Update is called once per frame
    void Update() {
        //ショット
        if (is_Shot_Bullet) {
            if (shot_Time < shot_Span) {
                shot_Time += Time.deltaTime;
            }
            else {
                shot_Time = 0;
                GameObject bullet = _pool.GetObject();
                bullet.transform.position = transform.position;
            }
        }
    }


    //is_Shot_BulletのSetter
    public void Set_Is_Shot_Bullet(bool is_Shot_Bullet) {
        this.is_Shot_Bullet = is_Shot_Bullet;
    }
}
