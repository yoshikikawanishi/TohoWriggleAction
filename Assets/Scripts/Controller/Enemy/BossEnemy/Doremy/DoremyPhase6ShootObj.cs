using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyPhase6ShootObj : MonoBehaviour {

    //スクリプト
    private ObjectPoolManager pool_Manager;

    //フィールド変数
    private ObjectPool bullet_Pool;
    private float start_Speed = 1f;
    private float start_Angle = 0;
    private float inter_Angle = 5f;
    private float span = 0.08f;
    private float lifeTime = 10f;
    private Color bullet_Color;


    //Awake
    private void Awake() {
        //取得
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();

        bullet_Pool = pool_Manager.Get_Pool("EllipseBullet");
    }

	
	//渦巻き弾開始
    public void Start_Spiral_Shoot(Color bullet_Color, float inter_Angle) {
        this.bullet_Color = bullet_Color;
        this.inter_Angle = inter_Angle;
        StartCoroutine("Shoot_Spiral_Bullet");
    }


    //渦巻き弾
    private IEnumerator Shoot_Spiral_Bullet() {
        float angle = start_Angle * Mathf.PI / 180f;
        while (true) {
            //弾の生成、発射
            var bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * -16f;
            bullet.transform.LookAt2D(gameObject.transform, Vector2.right);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * start_Speed;
            bullet.GetComponent<SpriteRenderer>().color = bullet_Color;
            //効果音
            UsualSoundManager.Small_Shot_Sound();
            //弾の加速
            StartCoroutine(Accelerate(bullet, 0.5f, 2.0f));
            //弾の消滅
            Delete_Bullet(bullet, lifeTime);
            yield return new WaitForSeconds(span);
            //次の角度
            angle += inter_Angle * Mathf.PI / 180f;
        }
    }


    //加速用
    private IEnumerator Accelerate(GameObject bullet, float acc, float time_Span) {
        yield return new WaitForSeconds(1.5f);
        Rigidbody2D bullet_Rigid = bullet.GetComponent<Rigidbody2D>();
        if(bullet_Rigid == null) {
            yield break;
        }
        for (float t = 0; t < time_Span; t += Time.deltaTime) {
            bullet_Rigid.velocity += (Vector2)bullet.transform.right * acc * Time.timeScale;
            yield return null;
        }
        
    }

    //弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        bullet.GetComponent<EnemyBullet>().StartCoroutine("Delete_Pool_Bullet", lifeTime);
    }


    //中止
    public void Stop_Shoot() {
        StopCoroutine("Shoot_Spiral_Bullet");
        if (gameObject != null) {
            Destroy(gameObject, 5.0f);
        }
    }
}
