using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremySpiralShoot : MonoBehaviour {

    //スクリプト
    private SpiralBulletFunction[] _spiral = new SpiralBulletFunction[4];
    private BulletPoolFunctions _bullet;
    private ObjectPoolManager pool_Manager;
    

    //Awake
    private void Awake() {
        //取得
        _spiral = GetComponents<SpiralBulletFunction>();
        _bullet = GetComponent<BulletPoolFunctions>();
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
    }
	

	//渦巻き弾開始
    public void Start_Spiral_Shoot() {
        //拡散弾
        StartCoroutine("Shoot_Small_Bullet");
        //渦巻き弾
        for (int i = 0; i < _spiral.Length; i++) {
            _spiral[i].Set_Bullet_Pool(pool_Manager.Get_Pool("RedMiddleBullet"));
        }
        _spiral[0].Start_Spiral_Bullet(100f, 180, 14f, 0.1f, 6.0f);
        _spiral[1].Start_Spiral_Bullet(100f, 135f, 8f, 0.1f, 6.0f);
        _spiral[2].Start_Spiral_Bullet(100f, -135, -8f, 0.1f, 6.0f);
        _spiral[3].Start_Spiral_Bullet(100f, -180f, -14f, 0.1f, 6.0f);
    }

    //拡散弾
    private IEnumerator Shoot_Small_Bullet() {
        _bullet.Set_Bullet_Pool(pool_Manager.Get_Pool("SmallBullet"));
        for(int i = 0; i < 10; i++) {
            yield return new WaitForSeconds(0.5f);
            _bullet.Odd_Num_Bullet(18, 20f, 160f, 5.0f);
        }
    }


    //渦巻き弾終了
    public void Stop_Spiral_Shoot() {
        for(int i = 0; i < _spiral.Length; i++) {
            _spiral[i].Stop_Spiral_Bullet();
        }
        StopCoroutine("Shoot_Small_Bullet");
    }
}
