using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyPhase4ShootObj : MonoBehaviour {

    //スクリプト
    private ObjectPoolManager pool_Manager;
    private BulletPoolFunctions _bullet;
    private SpiralBulletFunction[] _spiral = new SpiralBulletFunction[4];
    private ScatterPoolBullet _scatter;

    //影ドレミー
    private GameObject shadow_Doremy;
    private ScatterPoolBullet shadow_Scatter;


    //Awake
    private void Awake() {
        //取得
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        _bullet = GetComponent<BulletPoolFunctions>();
        _spiral = GetComponents<SpiralBulletFunction>();
        _scatter = GetComponent<ScatterPoolBullet>();
        shadow_Doremy = transform.GetChild(0).gameObject;
        shadow_Scatter = shadow_Doremy.GetComponent<ScatterPoolBullet>();
    }


    //4つ角からの5Way弾用
    public void Shoot_Five_Way_Bullet(float center_Angle) {
        _bullet.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        _bullet.Some_Way_Bullet(5, 80f, center_Angle, 20f, 6.0f);
        _bullet.Some_Way_Bullet(5, 120f, center_Angle, 20f, 6.0f);
        UsualSoundManager.Shot_Sound();
    }


    //中央で渦巻き弾用
    public void Start_Spiral_Shoot() {
        for(int i = 0; i < 4; i++) {
            _spiral[i].Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
            _spiral[i].Start_Spiral_Bullet(80f, i * 90f, 8f, 0.1f, 7.0f);
        }
    }

    public void Stop_Spiral_Shoot() {
        for(int i = 0; i < 4; i++) {
            _spiral[i].Stop_Spiral_Bullet();
        }
    }


    //分裂してばらまき弾用
    public void Start_Scatter_Shoot() {
        shadow_Scatter.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        _scatter.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        shadow_Scatter.Start_Scatter(22f, 80f, 6.0f, 5.0f);
        _scatter.Start_Scatter(22f, 80f, 6.0f, 5.0f);
        UsualSoundManager.Shot_Sound();
    }

    public void Stop_Scatter_Shoot() {
        shadow_Scatter.Stop_Scatter();
        _scatter.Stop_Scatter();
    }
    
	
}
