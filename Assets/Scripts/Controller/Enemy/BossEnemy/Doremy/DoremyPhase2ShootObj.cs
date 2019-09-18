using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyPhase2ShootObj : MonoBehaviour {

    //弾
    [SerializeField] private GameObject ring_Bullet_Prefab;
    [SerializeField] private GameObject nightmare_Bullet_Prefab;

    private GameObject nightmare_Bullet;
    private GameObject[] ring_Bullets = new GameObject[4];

    private BulletPoolFunctions _bullet;
    private ObjectPoolManager pool_Manager;

    
	// Use this for initialization
	void Start () {
        //取得
        _bullet = GetComponent<BulletPoolFunctions>();
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
	}


    //リング弾を４つ展開、水平に発射
    public void Shoot_Ring_Bullet() {
        StartCoroutine("Ring_Bullet_Routine");
    }

    private IEnumerator Ring_Bullet_Routine() {
        int num = 4;

        for (int i = 0; i < num; i++) {
            //生成
            ring_Bullets[i] = Instantiate(ring_Bullet_Prefab);
            ring_Bullets[i].transform.position = transform.position;
            //展開
            MoveBetweenTwoPoints bullet_Move = ring_Bullets[i].AddComponent<MoveBetweenTwoPoints>();
            Vector2 next_Pos = new Vector2(transform.position.x + 32f, transform.position.y + (-64f + i * 48f));
            bullet_Move.Start_Move(next_Pos, 0, 0.05f);
        }
        UsualSoundManager.Shot_Sound();

        yield return new WaitForSeconds(1.0f);
        
        //リジッドボディの取得
        Rigidbody2D[] bullet_Rigid = new Rigidbody2D[num];
        for(int i = 0; i < num; i++) {
            bullet_Rigid[i] = ring_Bullets[i].GetComponent<Rigidbody2D>();
        }
        //発射
        UsualSoundManager.Shot_Sound();
        for(float v = 0; v < 400f; v += 8.0f) {
            for(int i = 0; i < num; i++) {
                bullet_Rigid[i].velocity = new Vector2(-v, 0);
            }
            yield return null;
        }
        //消滅
        for(int i = 0; i < num; i++) {
            Destroy(ring_Bullets[i], 5.0f);
        }
    }


    //ナイトメア弾出す
    public GameObject Shoot_Nightmare_Bullet() {
        nightmare_Bullet = Instantiate(nightmare_Bullet_Prefab);
        nightmare_Bullet.transform.position = transform.position;
        return nightmare_Bullet;
    }


    //全方位弾
    public void Start_Diffusion_Shoot() {
        StartCoroutine("Shoot_Diffusion_Bullet");
    }

    private IEnumerator Shoot_Diffusion_Bullet() {
        _bullet.Set_Bullet_Pool(pool_Manager.Get_Pool("SmallBullet"));
        float center_Angle = 0;
        while (true) {
            yield return new WaitForSeconds(3.0f);
            _bullet.Diffusion_Bullet(18, 100f, center_Angle, 6.0f);
            UsualSoundManager.Shot_Sound();
            center_Angle += 10f;
        }
    }

    public void Stop_Diffusion_Shoot() {
        StopCoroutine("Shoot_Diffusion_Bullet");
    }


    //中止
    public void Stop_Shoot() {
        StopCoroutine("Ring_Bullet_Routine");
        if(nightmare_Bullet != null) {
            Destroy(nightmare_Bullet);
        }
        Stop_Diffusion_Shoot();
        for(int i = 0; i < 4; i++) {
            if(ring_Bullets[i] != null) {
                Destroy(ring_Bullets[i]);
            }
        }
    }
	
	
}
