using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFunctions : MonoBehaviour {

    //弾
    private GameObject bullet;

    //弾のセット
    public void Set_Bullet(GameObject bullet) {
        this.bullet = bullet;
    }


    //自機狙い奇数弾
    public void Odd_Num_Bullet(int num, float angle, float speed, float lifeTime) {
        int center = num / 2;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            for (int i = 0; i < num; i++) {
                //弾の生成
                GameObject odd_Bullet = Instantiate(bullet);
                odd_Bullet.transform.position = transform.position;
                //弾の方向転換
                odd_Bullet.transform.LookAt2D(player.transform, Vector2.right);
                float r = (i - center) * angle;
                odd_Bullet.transform.Rotate(0, 0, r);
                //弾の発射
                odd_Bullet.GetComponent<Rigidbody2D>().velocity = odd_Bullet.transform.right * speed;
                //弾の消去
                if (lifeTime > 0) {
                    Destroy(odd_Bullet, lifeTime);
                }
            }
        }
        else {
            Debug.Log("Can't Find Player");
        }
    }

    //自機外し偶数弾
    public void Even_Num_Bullet(int num, float angle, float speed, float lifeTime) {
        float center = num / 2 - 0.5f;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            for (int i = 0; i < num; i++) {
                //弾の生成
                GameObject even_Bullet = Instantiate(bullet);
                even_Bullet.transform.position = transform.position;
                //方向転換
                even_Bullet.transform.LookAt2D(player.transform, Vector2.right);
                float r = (i - center) * angle;
                even_Bullet.transform.Rotate(0, 0, r);
                //弾の発射
                even_Bullet.GetComponent<Rigidbody2D>().velocity = even_Bullet.transform.right * speed;
                //弾の消去
                if (lifeTime > 0) {
                    Destroy(even_Bullet, lifeTime);
                }
            }
        }
        else {
            Debug.Log("Can't Find Player");
        }
    }



    //円形拡散弾
    public void Diffusion_Bullet(int num, float speed, float center_Angle, float lifeTime) {
        for (int i = 0; i < num; i++) {
            //弾を円形に生成
            GameObject diff_Bullet = Instantiate(bullet) as GameObject;
            float noise = Random.Range(-0.01f, 0.01f);
            Vector3 circle = new Vector2(Mathf.Cos(i * 2 * Mathf.PI / num + center_Angle + noise), Mathf.Sin(i * 2 * Mathf.PI / num + center_Angle + noise)) * 10;
            diff_Bullet.transform.position = transform.position + circle;
            //弾の方向転換
            diff_Bullet.transform.LookAt2D(transform.position, Vector2.right);
            //弾の発射
            diff_Bullet.GetComponent<Rigidbody2D>().velocity =  diff_Bullet.transform.right * -speed;
            //弾の消去
            if (lifeTime > 0) {
                Destroy(diff_Bullet, lifeTime);
            }
        }
    }


    //nWay弾
    public void Some_Way_Bullet(int num, float speed, float interAngle, float lifeTime) {
        float center;
        //偶数数wayの場合
        if (num % 2 == 0) {
            center = num / 2 - 0.5f;
        }
        //奇数wayの場合
        else {
            center = num / 2;
        }
        for (int i = 0; i < num; i++) {
            //弾の生成
            GameObject some_Bullet = Instantiate(bullet);
            float angle = interAngle * (i - center);
            Vector3 circle = new Vector2(-Mathf.Cos(2 * Mathf.PI * angle / 360f), Mathf.Sin(2 * Mathf.PI * angle / 360f)) * 0.5f;
            some_Bullet.transform.position = transform.position + circle;
            //方向転換
            some_Bullet.transform.LookAt2D(gameObject.transform, Vector2.right);
            //弾の発射
            some_Bullet.GetComponent<Rigidbody2D>().velocity = -speed * bullet.transform.right;
            //弾の消去
            if (lifeTime > 0) {
                Destroy(some_Bullet, lifeTime);
            }
        }
    }



}