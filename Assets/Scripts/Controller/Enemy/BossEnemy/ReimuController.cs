using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuController : MonoBehaviour {

    //自機
    private GameObject player;
    //オブジェクトプール
    private ObjectPool _pool;

    //戦闘開始
    public bool start_Battle = false;

    //ショットスパン
    private float shot_Span = 0.2f;
    private float shot_Time = 0;


    // Use this for initialization
    void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //オブジェクトプール
        _pool = gameObject.AddComponent<ObjectPool>();
        GameObject reimu_Bullet = Resources.Load("Bullet/PooledBullet/ReimuTalismanBullet2") as GameObject;
        _pool.CreatePool(reimu_Bullet, 10);
    }
	

	// Update is called once per frame
	void Update () {
        if (start_Battle && player != null) {
            if(shot_Time < shot_Span) {
                shot_Time += Time.deltaTime;
            }
            else {
                shot_Time = 0;
                GameObject bullet = _pool.GetObject();
                bullet.transform.position = transform.position;
            }
        }
	}
}
