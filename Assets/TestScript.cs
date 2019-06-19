using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
        StartCoroutine("Reimu_Test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator Reimu_Test() {
        MoveBetweenTwoPoints _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        BulletPoolFunctions _bullet_Pool = gameObject.AddComponent<BulletPoolFunctions>();
        ScatterPoolBullet _scatter_Bullet = gameObject.AddComponent<ScatterPoolBullet>();
        //弾のオブジェクトプール
        ObjectPool white_Talisman_Pool = gameObject.AddComponent<ObjectPool>();
        ObjectPool red_Talisman_Pool = gameObject.AddComponent<ObjectPool>();
        ObjectPool red_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        Create_Bullet_Pools(white_Talisman_Pool, red_Talisman_Pool, red_Bullet_Pool);
        //移動
        yield return new WaitForSeconds(1.0f);
        _move.Start_Move(new Vector3(140f, -12f), 32f, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        while (true) {
            //全方位弾
            _bullet_Pool.Set_Bullet_Pool(white_Talisman_Pool);
            float center_Angle = Random.Range(-90, 90);
            _bullet_Pool.Diffusion_Bullet(24, 90f, center_Angle, 7.0f);
            yield return new WaitForSeconds(0.3f);
            center_Angle += 7f;
            _bullet_Pool.Set_Bullet_Pool(red_Talisman_Pool);
            for (int i = 1; i < 4; i++) {
                _bullet_Pool.Diffusion_Bullet(24, (90f - i * 5), (center_Angle + i * 3), 7.0f);
            }
            yield return new WaitForSeconds(0.5f);
            //弾をばらまきながら移動
            _scatter_Bullet.Set_Bullet_Pool(red_Bullet_Pool);
            _scatter_Bullet.Start_Scatter(30f, 50f, 2.0f, 9.0f);
            Vector3 next_Pos;
            if (transform.position.y < 0) {
                next_Pos = new Vector3(140f, 24f);
            }
            else {
                next_Pos = new Vector3(140f, -48f);
            }
            _move.Start_Move(next_Pos, 0, 0.02f);
            yield return new WaitUntil(_move.End_Move);
            _scatter_Bullet.Stop_Scatter();

            yield return new WaitForSeconds(3.0f);
        }
    }


    //フェーズ2オブジェクトプール用
    private void Create_Bullet_Pools(ObjectPool pool1, ObjectPool pool2, ObjectPool pool3) {
        GameObject white_Talisman_Bullet = Resources.Load("Bullet/PooledBullet/WhiteTalismanBullet") as GameObject;
        GameObject red_Talisman_Bullet = Resources.Load("Bullet/PooledBullet/RedTalismanBullet") as GameObject;
        GameObject red_Bullet = Resources.Load("Bullet/PooledBullet/RedBulletPool") as GameObject;
        pool1.CreatePool(white_Talisman_Bullet, 32);
        pool2.CreatePool(red_Talisman_Bullet, 32);
        pool3.CreatePool(red_Bullet, 30);
    }

}
