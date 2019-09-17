using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyPhase3ShootObj : MonoBehaviour {

    //スクリプト
    private ObjectPoolManager pool_Manager;
    private BulletPoolFunctions _bullet_Pool;
    private BulletFunctions _bullet;

    //自機
    private GameObject player;

    //弾
    [SerializeField] private GameObject bounce_Bullet;


	// Use this for initialization
	void Start () {
        //取得
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        _bullet_Pool = GetComponent<BulletPoolFunctions>();
        _bullet = GetComponent<BulletFunctions>();
        player = GameObject.FindWithTag("PlayerTag");
    }


    //跳ねる球
    public void Shoot_Bounce_Bullet() {
        _bullet.Set_Bullet(bounce_Bullet);
        _bullet.Even_Num_Bullet(2, 20f, 200f, 6.0f);
        UsualSoundManager.Shot_Sound();
    }


    //全方位弾
    public void Shoot_Diffusion_Bullet() {
        _bullet_Pool.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        float center_Angle = Random.Range(0, 15f);
        for (int i = 0; i < 5; i++) {
            _bullet_Pool.Diffusion_Bullet(18, 150f + i * 15f, center_Angle, 5.0f);
        }
        UsualSoundManager.Shot_Sound();
    }


    //爆撃
    public void Start_Drop_Bullet(int num, float span) {
        _bullet_Pool.Set_Bullet_Pool(pool_Manager.Get_Pool("DoremyBullet"));
        StartCoroutine(Drop_Bullet(num, span));
    }

    private IEnumerator Drop_Bullet(int num, float span) {
        for (int i = 0; i < num; i++) {
            //弾の生成
            GameObject bullet = pool_Manager.Get_Pool("DoremyBullet").GetObject();
            bullet.transform.position = transform.position;
            UsualSoundManager.Shot_Sound();
            //初速
            float angle = - (Mathf.PI / num) * i;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 100f;
            //ホーミング
            StartCoroutine(Controlle_Drop_Bullet(bullet));
            yield return new WaitForSeconds(span);
        }
    }

    //爆撃用の弾のコントロール
    private IEnumerator Controlle_Drop_Bullet(GameObject bullet) {
        Rigidbody2D bullet_Rigid = bullet.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(0.5f);
        for (float t = 0; t < 0.4f; t += 0.016f) {
            bullet.transform.LookAt2D(player.transform, Vector2.right);
            bullet_Rigid.velocity += (Vector2)bullet.transform.right * 15f;
            yield return new WaitForSeconds(0.016f);
        }
    }


    //中止
    public void Stop_Shoot() {
        StopAllCoroutines();
    }
	
	
}
