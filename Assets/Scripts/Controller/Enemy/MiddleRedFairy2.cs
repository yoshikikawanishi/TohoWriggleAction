using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleRedFairy2 : MonoBehaviour {

    //スクリプト
    private BulletScrollPoolFunctions _bullet;

    //オブジェクトプール
    ObjectPoolManager pool_Manager;

    //カメラ
    private GameObject main_Camera;


    // Use this for initialization
    void Start () {
        //スクリプト
        _bullet = gameObject.AddComponent<BulletScrollPoolFunctions>();
        pool_Manager = GameObject.FindWithTag("BulletPoolTag").GetComponent<ObjectPoolManager>();
        //カメラ
        main_Camera = GameObject.FindWithTag("MainCamera");

        UsualSoundManager.Small_Shot_Sound();

        //動き
        StartCoroutine("Action");
    }
	

    //動き
    private IEnumerator Action() {
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0) {
            yield return null;
            transform.position += new Vector3(0, -speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        //ショット
        ObjectPool pool = pool_Manager.Get_Pool(Resources.Load("Bullet/PooledBullet/RedRiceBullet_Pool") as GameObject);
        _bullet.Set_Bullet_Pool(pool);
        for (int i = 0; i < 10; i++) {
            Familiar_Shot();
            _bullet.Odd_Num_Bullet(1, 0, 100f, 6.0f);
            UsualSoundManager.Small_Shot_Sound();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(4.0f);
        //上にはける
        while (transform.position.y < 210f) {
            transform.position += new Vector3(0, speed * Time.timeScale, 0);
            speed += 0.05f * Time.timeScale;
            yield return null;
        }
        gameObject.SetActive(false);
    }


    //使い魔の青弾
    private void Familiar_Shot() {
        for(int i = 0; i < transform.childCount; i++) {
            GameObject familiar = transform.GetChild(i).gameObject;
            //使い魔へのベクトル
            Vector2 vector = (familiar.transform.position - transform.position).normalized;
            //直行ベクトル
            Vector2 shot_Vector = new Vector2(-vector.y, vector.x);
            //ショット
            ObjectPool pool = pool_Manager.Get_Pool(Resources.Load("Bullet/PooledBullet/BlueBulletPool") as GameObject);
            GameObject bullet = pool.GetObject();
            bullet.transform.position = familiar.transform.position;
            bullet.transform.SetParent(main_Camera.transform);
            bullet.GetComponent<Rigidbody2D>().velocity = shot_Vector * 70f;
            Delete_Bullet(bullet, 7.0f);
        }
    }


    //オブジェクトプールの弾の消去
    private void Delete_Bullet(GameObject bullet, float lifeTime) {
        bullet.GetComponent<EnemyBullet>().StartCoroutine("Delete_Pool_Bullet", lifeTime);
    }

}
