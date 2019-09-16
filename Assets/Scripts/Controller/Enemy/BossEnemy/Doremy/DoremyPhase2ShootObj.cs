using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyPhase2ShootObj : MonoBehaviour {

    //弾
    [SerializeField] private GameObject ring_Bullet_Prefab;
    [SerializeField] private GameObject nightmare_Bullet_Prefab;

    private GameObject nightmare_Bullet;

    
	// Use this for initialization
	void Start () {
		
	}


    //リング弾を４つ展開、水平に発射
    public void Shoot_Ring_Bullet() {
        StartCoroutine("Ring_Bullet_Routine");
    }

    private IEnumerator Ring_Bullet_Routine() {
        int num = 4;
        GameObject[] ring_Bullets = new GameObject[num];

        for (int i = 0; i < num; i++) {
            //生成
            ring_Bullets[i] = Instantiate(ring_Bullet_Prefab);
            ring_Bullets[i].transform.position = transform.position;
            //展開
            MoveBetweenTwoPoints bullet_Move = ring_Bullets[i].AddComponent<MoveBetweenTwoPoints>();
            Vector2 next_Pos = new Vector2(transform.position.x + 16f, transform.position.y + (-64f + i * 48f));
            bullet_Move.Start_Move(next_Pos, 0, 0.05f);
        }

        yield return new WaitForSeconds(1.5f);
        
        //リジッドボディの取得
        Rigidbody2D[] bullet_Rigid = new Rigidbody2D[num];
        for(int i = 0; i < num; i++) {
            bullet_Rigid[i] = ring_Bullets[i].GetComponent<Rigidbody2D>();
        }

        //発射
        for(float v = 0; v < 400f; v += 8.0f) {
            for(int i = 0; i < num; i++) {
                bullet_Rigid[i].velocity = new Vector2(-v, 0);
            }
            yield return null;
        }
    }


    //ナイトメア弾出す
    public GameObject Shoot_Nightmare_Bullet() {
        nightmare_Bullet = Instantiate(nightmare_Bullet_Prefab);
        nightmare_Bullet.transform.position = transform.position;
        return nightmare_Bullet;
    }


    //中止
    public void Stop_Shoot() {
        StopCoroutine("Ring_Bullet_Routine");
        if(nightmare_Bullet != null) {
            Destroy(nightmare_Bullet);
        }
    }
	
	
}
