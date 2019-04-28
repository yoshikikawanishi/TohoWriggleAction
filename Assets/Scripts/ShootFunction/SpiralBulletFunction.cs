using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBulletFunction : BulletFunctions {

    //フィールド変数
    private float time = 0;
    private float angle = 0;
    private float start_Angle = 0;


    //弾のセット
    public void Set_Bullet(GameObject bullet, float startAngle) {
        base.Set_Bullet(bullet);
        this.start_Angle = startAngle;
    }


    //うずまき弾
    public void Spiral_Bullet(float speed, float interAngle, float span, float lifeTime) {
        if(time < span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            //弾の生成
            var bullet = Instantiate(base.bullet) as GameObject;
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(Mathf.Cos(start_Angle +angle), Mathf.Sin(start_Angle + angle));
            //弾の発射
            bullet.transform.LookAt2D(transform, Vector2.right);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * speed;
            //弾の寿命
            Destroy(bullet, lifeTime);
            //角度
            angle += interAngle;
        }


    }

}
