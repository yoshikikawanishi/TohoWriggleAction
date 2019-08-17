using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleGreenFairy3 : MonoBehaviour {
    
    //スクリプト
    private BulletScrollPoolFunctions _bullet;

    //オブジェクトプール
    ObjectPoolManager pool_Manager;


	// Use this for initialization
	void Start () {
        //スクリプト
        _bullet = gameObject.AddComponent<BulletScrollPoolFunctions>();
        pool_Manager = GameObject.FindWithTag("BulletPoolTag").GetComponent<ObjectPoolManager>();

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
        //全方位弾
        ObjectPool pool = pool_Manager.Get_Pool(Resources.Load("Bullet/PooledBullet/RedRiceBullet_Pool") as GameObject);
        _bullet.Set_Bullet_Pool(pool);
        _bullet.Diffusion_Bullet(16, 70, 0, 7.0f);
        UsualSoundManager.Shot_Sound();
        //左に寄ってくる
        while(transform.localPosition.x > -300f){
            transform.localPosition += new Vector3(-0.5f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        Destroy(gameObject);
    }

}
