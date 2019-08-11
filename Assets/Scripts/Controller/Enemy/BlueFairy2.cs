using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFairy2 : MonoBehaviour {


    //オブジェクトプール
    ObjectPoolManager pool_Manager;


    // Use this for initialization
    void Start () {
        pool_Manager = GameObject.FindWithTag("BulletPoolTag").GetComponent<ObjectPoolManager>();

        //動き
        StartCoroutine("Action");
    }


    //動き
    private IEnumerator Action() {
        //移動
        float speed = 4.0f;
        for (float t = 0; t < 1.5f; t += Time.deltaTime) {
            transform.position += new Vector3(-speed, 0);
            //減速
            if (speed > 1.5f) {
                speed -= 0.05f;
            }
            yield return new WaitForSeconds(0.016f);
        }
        //ショット
        ObjectPool pool = pool_Manager.Get_Pool(Resources.Load("Bullet/PooledBullet/BlueScalesBulletPool") as GameObject);
        BulletScrollPoolFunctions _bullet = gameObject.AddComponent<BulletScrollPoolFunctions>();
        _bullet.Set_Bullet_Pool(pool);
        _bullet.Odd_Num_Bullet(1, 0, 100f, 6.0f);
        _bullet.Even_Num_Bullet(2, 40f, 70f, 6.0f);
        UsualSoundManager.Shot_Sound();
        //はける
        float escape_Acc = 0.05f;
        Vector3 escape_Speed = new Vector3(-3f, 0);
        if (transform.position.y < 0) { escape_Acc = -0.05f; }
        while(Mathf.Abs(transform.position.y) < 210f) {
            transform.position += escape_Speed * Time.timeScale;
            escape_Speed += new Vector3(0, escape_Acc);
            yield return new WaitForSeconds(0.016f);
        }
        Destroy(gameObject);
    }
}
