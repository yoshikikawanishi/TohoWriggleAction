using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoreKing : MonoBehaviour {

    //ショット用
    private GameObject[] shoot_Obj = new GameObject[3];

    //スクリプト
    private SpiralBulletFunction _spiral;
    private ObjectPoolManager pool_Manager;
    

    //Awake
    private void Awake() {
        //取得
        for(int i = 0; i < 3; i++) {
            shoot_Obj[i] = transform.GetChild(i).gameObject;
        }
        _spiral = shoot_Obj[2].GetComponent<SpiralBulletFunction>();
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
    }


    //下部ショットオブジェクト
    public void Shoot_In_Under_Obj() {
        BulletPoolFunctions _bullet = shoot_Obj[0].GetComponent<BulletPoolFunctions>();
        _bullet.Set_Bullet_Pool(pool_Manager.Get_Pool("RingBullet"));
        for (int i = 0; i < 5; i++) {
            _bullet.Odd_Num_Bullet(3, 5, 100f - i * 10f, 7.0f);
        }
        UsualSoundManager.Shot_Sound();
    }

   
    //上部ショットオブジェクト
    public void Shoot_In_Upper_Obj() {
        StartCoroutine("Upper_Obj_Shoot_Routine");
    }

    private IEnumerator Upper_Obj_Shoot_Routine() {
        ObjectPool pool = pool_Manager.Get_Pool("RedMiddleBullet");
        for(int i = 0; i < 3; i++) {
            GameObject bullet = pool.GetObject();
            bullet.transform.position = shoot_Obj[1].transform.position + new Vector3(i * 32f, 0);
            bullet.GetComponent<Rigidbody2D>().gravityScale = 32f;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-200f + i * 15f, 180f);
            UsualSoundManager.Shot_Sound();
            yield return new WaitForSeconds(0.5f);
        }
    }


    //本体渦巻き弾
    public void Start_Spiral_Shoot() {
        _spiral.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        _spiral.Start_Spiral_Bullet(120f, 45f, 12f, 0.1f, 6.0f);
        Invoke("Stop_Spiral_Shoot", 3.0f);
    }

    //渦巻き弾中止
    public void Stop_Spiral_Shoot() {
        if (_spiral == null) {
            _spiral = GetComponent<SpiralBulletFunction>();
            return;
        }
        _spiral.Stop_Spiral_Bullet();
    }

    //ビーム
    public void Put_Out_Beam() {
        var beam = Instantiate(Resources.Load("Bullet/MiniMasterSpark") as GameObject);
        beam.transform.SetParent(transform);
        beam.transform.localPosition = new Vector3(20, -100f);
        beam.transform.Rotate(new Vector3(0, 0, 1), -90);       
    }


    //中止
    public void Stop_Shoot() {
        StopAllCoroutines();
        Stop_Spiral_Shoot();
        gameObject.SetActive(false);
    }
	
	
}
