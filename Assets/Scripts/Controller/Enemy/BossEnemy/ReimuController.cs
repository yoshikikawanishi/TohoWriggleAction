using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuController : MonoBehaviour {

    //自機
    private GameObject player;
    //コンポーネント
    private Animator _anim;
    //オブジェクトプール
    private ObjectPool _pool;

    //戦闘開始
    public bool start_Battle = false;

    //ショットスパン
    private float shot_Span = 0.2f;
    private float shot_Time = 0;


    // Use this for initialization
    void Awake () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //コンポーネント
        _anim = GetComponent<Animator>();
        //オブジェクトプール
        _pool = gameObject.AddComponent<ObjectPool>();
        GameObject reimu_Bullet = Resources.Load("Bullet/PooledBullet/ReimuTalismanBullet2") as GameObject;
        _pool.CreatePool(reimu_Bullet, 10);
    }
	

	// Update is called once per frame
	void Update () {
 
	}


    //アニメーション
    public void Change_Parameter(string change_Bool) {
        _anim.SetBool("DashBool", false);
        _anim.SetBool("AvoidBool", false);

        _anim.SetBool(change_Bool, true);
    }
}
