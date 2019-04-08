using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour {

    //オブジェクトプール
    private ObjectPool _pool;
    //自機
    private GameObject player;
    private PlayerController player_Controller;

    //ショット
    private float BULLET_SPEED = 500.0f;
    private float time = 0;
    private float SHOT_SPAN = 0.1f;

    
    // Use this for initialization
    void Start () {
        //オブジェクトプール
        _pool = GetComponent<ObjectPool>();
        GameObject player_Bullet = Resources.Load("Bullet/PooledBullet/PlayerBullet") as GameObject;
        _pool.CreatePool(player_Bullet, 5);

        //自機
        player = transform.parent.gameObject;
        player_Controller = GetComponentInParent<PlayerController>();
    }
	

	// Update is called once per frame
	void Update () {
        if (player_Controller.is_Playable) {
            Shot();
        }
	}


    //ショット
    private void Shot() {
        if (Input.GetKey(KeyCode.X)) {
            if (time < SHOT_SPAN) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                //弾の発射
                for (int i = 0; i < 2; i++) {
                    var bullet = _pool.GetObject();
                    bullet.transform.position = gameObject.transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(BULLET_SPEED * player.transform.localScale.x, 0);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.X)) {
            time = SHOT_SPAN;
        }
    }
}
