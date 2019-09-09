using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriggleShot : MonoBehaviour {

    
    //オーディオ
    private AudioSource shot_Sound;
    //オブジェクトプール
    private ObjectPool flies_Bullet_Pool;
    private ObjectPool butterFly_Bullet_Pool;
    private ObjectPool bee_Bullet_Pool;
    //自機
    private PlayerController _playerController;
    //スクリプト
    private PlayerManager _playerManager;

    //オプションのアニメーション
    private Animator[] options_Anim = new Animator[2];

    //ショット
    private float time = 0;

    //強化段階
    private int power_Grade = 0;

    //オプションのタイプ
    private string option_Type;

    //Beetle用
    private float beetle_Shot_Span = 0.4f;

    
    // Use this for initialization
    void Start () {
        //オーディオの取得
        shot_Sound = GetComponents<AudioSource>()[2];
        //スクリプトの取得
        _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        _playerController = gameObject.GetComponent<PlayerController>();
        //オブジェクトプール
        flies_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        butterFly_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        bee_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        GameObject flies_Bullet = Resources.Load("Bullet/PooledBullet/FliesBullet") as GameObject;
        GameObject butterFly_Bullet = Resources.Load("Bullet/PooledBullet/ButterFlyBullet") as GameObject;
        GameObject bee_Bullet = Resources.Load("Bullet/PooledBullet/BeeBullet") as GameObject;
        flies_Bullet_Pool.CreatePool(flies_Bullet, 20);
        butterFly_Bullet_Pool.CreatePool(butterFly_Bullet, 20);
        bee_Bullet_Pool.CreatePool(bee_Bullet, 20);

        //オプションの取得
        options_Anim[0] = transform.GetChild(4).gameObject.GetComponent<Animator>();
        options_Anim[1] = transform.GetChild(5).gameObject.GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update () {
        //オプションタイプの変更
        Change_Option();
        //ショットの強化
        Power_Up();

        //ショット
        if (_playerController.Get_Playable()) {
            switch (this.option_Type) {
                case "Flies": Flies_Shot(); break;
                case "ButterFly": ButterFly_Shot(); break;
                case "Beetle": Beetle_Shot(); break;
                case "Bee": Bee_Shot(); break;
            }
        }
       
	}


    //オプションタイプの変更
    private void Change_Option() {
        if (_playerManager.option_Type != this.option_Type) {
            this.option_Type = _playerManager.option_Type;
            Change_Option_Anim(option_Type + "Bool");
        }
    }

    //オプションのアニメーションの変更
    private void Change_Option_Anim(string change_Bool) {
        for(int i = 0; i < 2; i++) {
            options_Anim[i].SetBool("FliesBool", false);
            options_Anim[i].SetBool("ButterFlyBool", false);
            options_Anim[i].SetBool("BeetleBool", false);
            options_Anim[i].SetBool("BeeBool", false);

            options_Anim[i].SetBool(change_Bool, true);
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


    //オプションがハエのとき
    private void Flies_Shot() {
        if (InputManager.Instance.GetKey(MBLDefine.Key.Shot)) {
            if (time < 0.15f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 2;
                float bullet_Speed = 400f;
                //1段階目以降
                if (power_Grade >= 1) {
                    bullet_Num = 3;
                }
                //2段階目以降
                if (power_Grade >= 2) {
                    bullet_Speed = 500f;
                }
                //3段階目以降
                if (power_Grade >= 3) {
                    bullet_Num = 4;
                }
                //4段階目
                if(power_Grade >= 4) {
                    bullet_Num = 8;
                    bullet_Speed = 600f;
                }
                //弾の生成、発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = flies_Bullet_Pool.GetObject();
                    float width = 12f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, -width / 2 + width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                }             
                shot_Sound.Play();
            }
        }
        else if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Shot)) {
            time = 0.15f;
        }
    }


    //オプションが蝶のとき
    private void ButterFly_Shot() {
        if (InputManager.Instance.GetKey(MBLDefine.Key.Shot)) {
            if (time < 0.12f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 2;
                float bullet_Speed = 400f;
                //1段階目以降
                if (power_Grade >= 1) {
                    bullet_Num = 3;
                }
                //2段階目以降
                if (power_Grade >= 2) {
                    bullet_Speed = 450f;
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
                //弾の生成,発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = butterFly_Bullet_Pool.GetObject();
                    float width = 8f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, -width / 2 + width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                }
                shot_Sound.Play();
            }
        }
        else if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Shot)) {
            time = 0.1f;
        }
    }



    //オプションがカブトムシのとき
    private void Beetle_Shot() {
        if(time < beetle_Shot_Span) {
            time += Time.deltaTime;
        }
        else if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Shot)) {
            time = 0;
            beetle_Shot_Span = 0.5f;
            int bullet_Num = 1;
            Vector2 bullet_Speed = new Vector2(150f * transform.localScale.x, 300f);
            //1段階目以降
            if (power_Grade >= 1) {
                beetle_Shot_Span = 0.5f;
            }
            //2段階目以降
            if (power_Grade >= 2) {
                beetle_Shot_Span = 0.4f;
            }
            //3段階目以降
            if (power_Grade >= 3) {
                bullet_Num = 2;
                beetle_Shot_Span = 0.35f;
            }
            //4段階目
            if (power_Grade >= 4) {
                bullet_Num = 3;
                beetle_Shot_Span = 0.3f;
            }
            //弾の発射
            for (int i = 0; i < bullet_Num; i++) {
                bullet_Speed += GetComponent<Rigidbody2D>().velocity / 2;
                var bullet = Instantiate(Resources.Load("Bullet/BeetleBullet")) as GameObject;
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = bullet_Speed + new Vector2(i * 50f, 0);
            }
            shot_Sound.Play();
        }
    }


    //オプションが蜂のとき
    private void Bee_Shot() {
        if (InputManager.Instance.GetKey(MBLDefine.Key.Shot)) {
            if (time < 0.3f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                int bullet_Num = 1;
                float bullet_Speed = 700f;
                //1段階目以降弾を増やす
                if (power_Grade >= 1) {
                    bullet_Speed = 800f;
                }
                //2段階目以降早くする
                if (power_Grade >= 2) {
                    bullet_Num = 2;
                    bullet_Speed = 900f;
                }
                //3段階目以降
                if (power_Grade >= 3) {
                    bullet_Num = 3;
                }
                //4段階目
                if (power_Grade >= 4) {
                    bullet_Num = 5;
                    bullet_Speed = 1000f;
                }
                //弾の発射
                for (int i = 0; i < bullet_Num; i++) {
                    var bullet = bee_Bullet_Pool.GetObject();
                    float width = 12f + bullet_Num * 2;
                    bullet.transform.position = transform.position + new Vector3(0, width / 2 - width * 2 / bullet_Num * i);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                    //中心の弾は2重にする
                    if (i == bullet_Num / 2 && bullet_Num != 2) {
                        var bullet2 = bee_Bullet_Pool.GetObject();
                        bullet2.transform.position = transform.position + new Vector3(0, width / 2 - width * 2 / bullet_Num * i);
                        bullet2.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
                    }
                }
                shot_Sound.Play();
            }          
        }
        else if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Shot)) {
            time = 0.3f;
        }
    }




    

}
