using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBulletFunction : MonoBehaviour {

    //フィールド変数
    private ObjectPool bullet_Pool;
    private float speed;
    private float start_Angle = 0;
    private float inter_Angle;
    private float span;
    private float lifeTime;


    //弾のセット
    public void Set_Bullet_Pool(ObjectPool bullet_Pool) {
        this.bullet_Pool = bullet_Pool;
    }


    //渦巻き弾開始
    public void Start_Spiral_Bullet(float speed,float startAngle, float interAngle, float span, float lifeTime) {
        this.speed = speed;
        this.start_Angle = startAngle;
        this.inter_Angle = interAngle;
        this.span = span;
        this.lifeTime = lifeTime;
        StartCoroutine("Spiral_Bullet");
    }


    //渦巻き弾終了
    public void Stop_Spiral_Bullet() {
        StopCoroutine("Spiral_Bullet");
    }


    //渦巻き弾
    private IEnumerator Spiral_Bullet() {   
        if(bullet_Pool == null) {
            Debug.Log("Not Set SpiralBullet");
            Stop_Spiral_Bullet();
        }
        float angle = start_Angle * Mathf.PI / 180f;
        while (true) {
            //弾の生成、発射
            var bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * -16f;
            bullet.transform.LookAt2D(gameObject.transform, Vector2.right);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * speed;
            //効果音
            UsualSoundManager.Small_Shot_Sound();
            //弾の消滅
            Delete_Bullet(bullet, lifeTime);
            yield return new WaitForSeconds(span);
            //次の角度
            angle += inter_Angle * Mathf.PI / 180f;
        }
    }

    
    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        bullet.GetComponent<EnemyBullet>().StartCoroutine("Delete_Pool_Bullet", lifeTime);
    }

}
