using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleRedFairy : Enemy {

    //スクリプト
    private BulletScrollFunctions _bullet_Scroll;
    private BulletScrollPoolFunctions _bullet_Scroll_Pool;
    

    // Use this for initialization
    new void Start () {
        base.Start();
        //スクリプト
        _bullet_Scroll = gameObject.AddComponent<BulletScrollFunctions>();
        _bullet_Scroll_Pool = gameObject.AddComponent<BulletScrollPoolFunctions>();

        UsualSoundManager.Small_Shot_Sound();
        //行動
        StartCoroutine("Action");
    }
	
    //行動
    private IEnumerator Action() {
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0) {
            yield return null;
            transform.position += new Vector3(0, -speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        yield return new WaitForSeconds(1.0f);
        //使い魔突進
        GameObject familiar = transform.GetChild(0).gameObject;
        familiar.SetActive(true);
        _bullet_Scroll.Set_Bullet(familiar);
        _bullet_Scroll.Odd_Num_Bullet(3, 10f, 100f, 8.0f);
        Destroy(familiar);
        yield return new WaitForSeconds(12f / 7f);
        //ショット
        GameObject bullet = Resources.Load("Bullet/PooledBullet/BlueBulletPool") as GameObject;
        ObjectPoolManager pool_Manager = GameObject.FindWithTag("BulletPoolTag").GetComponent<ObjectPoolManager>();
        _bullet_Scroll_Pool.Set_Bullet_Pool(pool_Manager.Get_Pool(bullet));
        for(int i = 0; i < 2; i++) {
            for(int j = 0; j < 7; j++) {
                _bullet_Scroll_Pool.Odd_Num_Bullet(3, 25f, 50f+j*10f, 5.0f);
            }
            yield return new WaitForSeconds(12f / 7f);
        }
    }


    //消滅時
    protected override void Vanish() {
        //使い魔の消滅
        GameObject familiar_Vanish_Effect = Resources.Load("Effect/FamiliarVanishEffect") as GameObject;
        for (int i = 0; i < transform.childCount; i++) {
            GameObject effect = Instantiate(familiar_Vanish_Effect);
            effect.transform.position = transform.GetChild(i).position;
        }
        base.Vanish();
    }
}
