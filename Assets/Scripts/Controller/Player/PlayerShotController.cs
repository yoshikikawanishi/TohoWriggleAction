using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour {

    //オプション番号
    [SerializeField] private int option_Num = 0;

    //オーディオ
    private AudioSource shot_Sound;
    //オブジェクトプール
    private ObjectPool _pool;
    //自機
    private GameObject player;
    private PlayerController player_Controller;
    //スクリプト
    private PlayerManager _playerManager;

    //ショット
    private float BULLET_SPEED = 500.0f;
    private float time = 0;
    private float SHOT_SPAN = 0.1f;
    //パワーショット
    private float time2 = 0;
    private float SHOT_SPAN2 = 0.1f;

    //強化段階
    private int power_Grade = 0;

    
    // Use this for initialization
    void Start () {
        //オーディオの取得
        shot_Sound = GetComponent<AudioSource>();
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        //オブジェクトプール
        _pool = GetComponent<ObjectPool>();
        GameObject player_Bullet = Resources.Load("Bullet/PooledBullet/PlayerBullet") as GameObject;
        _pool.CreatePool(player_Bullet, 20);

        //自機
        player = transform.parent.gameObject;
        player_Controller = GetComponentInParent<PlayerController>();
        
    }
	

	// Update is called once per frame
	void Update () {
        if (player_Controller.Get_Playable()) {
            Shot();
            if(power_Grade == 4) {
                Power_Shot();
            }
        }
        //ショットの強化
        Power_Up();
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
                var bullet = _pool.GetObject();
                bullet.transform.position = gameObject.transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(BULLET_SPEED * player.transform.localScale.x, 0);
                //1段階目以降弾を増やす
                if (power_Grade >= 1) {
                    if (option_Num == 0) { 
                        var bullet2 = _pool.GetObject();
                        bullet2.transform.position = gameObject.transform.position + new Vector3(0, -10f, 0);
                        bullet2.GetComponent<Rigidbody2D>().velocity = new Vector2(BULLET_SPEED * player.transform.localScale.x, 0);
                    }
                }
                //3段階目以降斜めに発射
                if (power_Grade >= 3) {
                    var bullet3 = _pool.GetObject();
                    bullet3.transform.position = gameObject.transform.position;
                    if (option_Num == 0) {
                        bullet3.GetComponent<Rigidbody2D>().velocity = new Vector2(700f * player.transform.localScale.x, -100f);
                    }
                    else {
                        bullet3.GetComponent<Rigidbody2D>().velocity = new Vector2(700f * player.transform.localScale.x, 100f);
                    }
                }
                shot_Sound.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.X)) {
            time = SHOT_SPAN;
        }
    }


    //パワーショット
    private void Power_Shot() {
        if (Input.GetKey(KeyCode.X) && option_Num == 0) {
            if (time2 < SHOT_SPAN2) {
                time2 += Time.deltaTime;
            }
            else {
                time2 = 0;
                var power_Bullet = Instantiate(Resources.Load("Bullet/PlayerShot2")) as GameObject;
                power_Bullet.transform.position = transform.position;
                power_Bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(500f * player.transform.localScale.x, 0);
                Destroy(power_Bullet, 0.4f);
            }
        }
        else if (Input.GetKeyUp(KeyCode.X)) {
            time = SHOT_SPAN2;
        }
    }


    //ショットの強化(弾速とスパン)
    private void Power_Up() {
        if (_playerManager.power < 16) {
            if (power_Grade != 0) {
                power_Grade = 0; BULLET_SPEED = 500.0f; SHOT_SPAN = 0.1f;
            }
        }
        else if (_playerManager.power < 32) {
            if (power_Grade != 1) {
                power_Grade = 1; BULLET_SPEED = 550.0f; SHOT_SPAN = 0.09f;
            }
        }
        else if (_playerManager.power < 64) {
            if (power_Grade != 2) {
                power_Grade = 2; BULLET_SPEED = 700.0f; SHOT_SPAN = 0.08f;
            }
        }
        else if (_playerManager.power < 128) {
            if (power_Grade != 3) {
                power_Grade = 3; BULLET_SPEED = 800.0f; SHOT_SPAN = 0.08f;
            }
        }
        else if (_playerManager.power == 128) {
            if (power_Grade != 4) {
                power_Grade = 4; BULLET_SPEED = 900.0f; SHOT_SPAN = 0.07f;
            }
        }
    }

}
