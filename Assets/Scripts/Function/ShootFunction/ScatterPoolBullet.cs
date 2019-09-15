using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPoolBullet : MonoBehaviour {

    //フィールド
    ObjectPool bullet_Pool;
    private float rate;
    private float speed;
    private float scatter_Span;
    private float lifeTime;

    //弾のセット
    public void Set_Bullet_Pool(ObjectPool bullet_Pool) {
        this.bullet_Pool = bullet_Pool;
    }

    //ばらまき開始
    public void Start_Scatter(float rate, float speed, float scatter_Span, float lifeTime) {
        this.rate = rate;
        this.speed = speed;
        this.scatter_Span = scatter_Span;
        this.lifeTime = lifeTime;
        StartCoroutine("Scatter");
    }


    //ばらまく
    private IEnumerator Scatter() {
        for (float t = 0; t < scatter_Span; t += Time.deltaTime) {
            GameObject bullet = bullet_Pool.GetObject();
            float direction = Random.Range(0, 6.3f);
            bullet.transform.position = transform.position + new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0);
            bullet.transform.LookAt2D(transform, Vector2.left);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * speed;
            Delete_Bullet(bullet, lifeTime);
            UsualSoundManager.Small_Shot_Sound();
            yield return new WaitForSeconds(1 / rate);
        }
    }

    
    //ばらまき停止
    public void Stop_Scatter() {
        StopAllCoroutines();
    }


    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        bullet.GetComponent<EnemyBullet>().StartCoroutine("Delete_Pool_Bullet", lifeTime);
    }

}
