using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFunctions : MonoBehaviour {

    //弾
    public GameObject bullet;

    //弾のセット
    public void Set_Bullet(GameObject bullet) {
        this.bullet = bullet;
    }


    //弾がセットされているか
    private bool Is_Set_Bullet() {
        if(bullet == null) {
            Debug.Log("Not Set Bullet");
            return false;
        }
        return true;
    }


    //弾の生成、発射
    public void Shoot_Bullet(Vector2 velocity, float lifeTime) {
        if (!Is_Set_Bullet()) {
            return;
        }
        var shoot_Bullet = Instantiate(bullet) as GameObject;
        shoot_Bullet.transform.position = transform.position;
        shoot_Bullet.GetComponent<Rigidbody2D>().velocity = velocity;
        if (lifeTime > 0) {
            Destroy(shoot_Bullet, lifeTime);
        }
    }

    //弾の生成、方向転換して発射
    public void Turn_Shoot_Bullet(float speed, float angle, float lifeTime) {
        if (!Is_Set_Bullet()) {
            return;
        }
        var turn_Bullet = Instantiate(bullet) as GameObject;
        turn_Bullet.transform.position = transform.position + new Vector3(Mathf.Cos(angle * Mathf.PI / 180f), Mathf.Sin(angle * Mathf.PI / 180), 0);
        turn_Bullet.transform.LookAt2D(transform, Vector2.right);
        turn_Bullet.GetComponent<Rigidbody2D>().velocity = turn_Bullet.transform.right * -speed;
        if (lifeTime > 0) {
            Destroy(turn_Bullet, lifeTime);
        }
    }

    //自機狙い奇数弾
    public void Odd_Num_Bullet(int num, float angle, float speed, float lifeTime) {
        if (!Is_Set_Bullet()) {
            return;
        }
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
        if (!Is_Set_Bullet()) {
            return;
        }
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



    //全方位弾
    public void Diffusion_Bullet(int num, float speed, float centerAngle, float lifeTime) {
        if (!Is_Set_Bullet()) {
            return;
        }
        for (int i = 0; i < num; i++) {
            //弾を円形に生成,発射
            float noise = Random.Range(-0.01f, 0.01f);
            float angle = i * 360f / num + centerAngle + noise;
            Turn_Shoot_Bullet(speed, angle, lifeTime);
        }
    }


    //nWay弾
    public void Some_Way_Bullet(int num, float speed, float centerAngle, float interAngle, float lifeTime) {
        if (!Is_Set_Bullet()) {
            return;
        }
        float center;
        //偶数wayの場合
        if (num % 2 == 0) {
            center = num / 2 - 0.5f;
        }
        //奇数wayの場合
        else {
            center = num / 2;
        }
        for (int i = 0; i < num; i++) {
            //弾の生成、発射
            float angle = centerAngle + interAngle * (i - center) + 180f;
            Turn_Shoot_Bullet(speed, angle, lifeTime);
        }
    }

}