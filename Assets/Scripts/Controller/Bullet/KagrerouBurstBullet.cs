using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagrerouBurstBullet : MonoBehaviour {

    //スクリプト、コンポーネント
    private BulletPoolFunctions _bullet;
    private ObjectPoolManager pool_Manager;

    //色
    [SerializeField] private string bullet_Name;


	// Use this for initialization
	void Start () {
        //取得
        _bullet = GetComponent<BulletPoolFunctions>();
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        //ショット
        StartCoroutine("Shot");
	}
	

    //ショット
    private IEnumerator Shot() {
        yield return new WaitForSeconds(6.0f);
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.0f);
        ObjectPool pool = pool_Manager.Get_Pool(bullet_Name);
        _bullet.Set_Bullet_Pool(pool);
        float center_Angle = Random.Range(-20f, 20f);
        _bullet.Diffusion_Bullet(15, 80f, center_Angle, 8.0f);
        UsualSoundManager.Shot_Sound();
        Destroy(gameObject);
    }

}
