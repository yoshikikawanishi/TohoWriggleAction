using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotController : MonoBehaviour {

    
    //オーディオ
    private AudioSource shot_Sound;
    //オブジェクトプール
    private ObjectPool flies_Bullet_Pool;
    private ObjectPool butterFly_Bullet_Pool;
    //自機
    private PlayerController _playerController;
    //スクリプト
    private PlayerManager _playerManager;

    //ショット
    private float time = 0;
    private float shot_Span = 0.1f;
 

    //強化段階
    private int power_Grade = 0;

    
    // Use this for initialization
    void Start () {
        //オーディオの取得
        shot_Sound = GetComponents<AudioSource>()[2];
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        _playerController = gameObject.GetComponent<PlayerController>();
        //オブジェクトプール
        flies_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        GameObject flies_Bullet = Resources.Load("Bullet/PooledBullet/FliesBullet") as GameObject;
        flies_Bullet_Pool.CreatePool(flies_Bullet, 20);

        butterFly_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        GameObject butterFly_Bullet = Resources.Load("Bullet/PooledBullet/ButterFlyBullet") as GameObject;
        butterFly_Bullet_Pool.CreatePool(butterFly_Bullet, 20);
    }
	

	// Update is called once per frame
	void Update () {
        if (_playerController.Get_Playable()) {
            switch (_playerManager.option_Type) {
                case "Flies": Flies_Shot(); break;
                case "ButterFly": ButterFly_Shot(); break;
                case "Beetle": Beetle_Shot(); break;
                case "Bee": Bee_Shot(); break;
            }
        }
        //ショットの強化
        Power_Up();
	}


    //オプションがハエのとき
    private void Flies_Shot() {
        if (Input.GetKey(KeyCode.X)) {
            if (time < shot_Span) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 2;
                float bullet_Speed = 400f;
                //1段階目以降弾を増やす
                if (power_Grade >= 1) {
                    bullet_Num = 3;
                }
                //2段階目以降早くする
                if (power_Grade >= 2) {
                    bullet_Speed = 500f;
                }
                //3段階目以降
                if (power_Grade >= 3) {
                    bullet_Num = 4;
                }
                //4段階目
                if(power_Grade >= 4) {
                    bullet_Num = 6;
                    bullet_Speed = 600f;
                }
                //弾の発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = flies_Bullet_Pool.GetObject();
                    float width = 12f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, -width / 2 + width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                }             
                shot_Sound.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.X)) {
            time = shot_Span;
        }
    }


    //オプションが蝶のとき
    private void ButterFly_Shot() {
        if (Input.GetKey(KeyCode.X)) {
            if (time < shot_Span) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 2;
                float bullet_Speed = 350f;
                //1段階目以降弾を増やす
                if (power_Grade >= 1) {
                    bullet_Num = 3;
                }
                //2段階目以降早くする
                if (power_Grade >= 2) {
                    bullet_Speed = 400f;
                }
                //3段階目以降
                if (power_Grade >= 3) {
                    bullet_Num = 4;
                }
                //4段階目
                if (power_Grade >= 4) {
                    bullet_Num = 5;
                    bullet_Speed = 500f;
                }
                //弾の発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = butterFly_Bullet_Pool.GetObject();
                    float width = 12f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, -width / 2 + width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                }
                shot_Sound.Play();
            }
        }
    }



    //オプションがカブトムシのとき
    private void Beetle_Shot() {

    }


    //オプションが蜂のとき
    private void Bee_Shot() {
        if (Input.GetKey(KeyCode.X)) {
            if (time < 0.3f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 1;
                float bullet_Speed = 600f;
                //1段階目以降弾を増やす
                if (power_Grade >= 1) {
                    bullet_Speed = 700f;
                }
                //2段階目以降早くする
                if (power_Grade >= 2) {
                    bullet_Num = 2;
                    bullet_Speed = 800f;
                }
                //3段階目以降
                if (power_Grade >= 3) {
                    bullet_Num = 3;
                }
                //4段階目
                if (power_Grade >= 4) {
                    bullet_Num = 4;
                    bullet_Speed = 900f;
                }
                //弾の発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = Instantiate(Resources.Load("Bullet/BeeBullet")) as GameObject;
                    float width = 12f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, width / 2 - width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                    Destroy(bullet, 1.3f);
                }
                shot_Sound.Play();
            }
        }
    }




    //ショットの強化段階
    private void Power_Up() {
        if (_playerManager.power < 16) {
            if (power_Grade != 0) {
                power_Grade = 0;
            }
        }
        else if (_playerManager.power < 32) {
            if (power_Grade != 1) {
                power_Grade = 1;
            }
        }
        else if (_playerManager.power < 64) {
            if (power_Grade != 2) {
                power_Grade = 2;
            }
        }
        else if (_playerManager.power < 128) {
            if (power_Grade != 3) {
                power_Grade = 3;
            }
        }
        else if (_playerManager.power == 128) {
            if (power_Grade != 4) {
                power_Grade = 4;
            }
        }
    }

}
