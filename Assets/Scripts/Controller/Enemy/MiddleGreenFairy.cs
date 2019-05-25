using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleGreenFairy : Enemy {

    //種類
    [SerializeField] private int kind;
    //スクリプト
    private BulletFunctions _bullet;

    //使い魔の消滅エフェクト
    private GameObject familiar_Vanish_Effect;


	// Use this for initialization
	new void Start () {
        base.Start();
        //スクリプト
        _bullet = gameObject.AddComponent<BulletFunctions>();
        //使い魔にBulletFunctionsをアタッチ
        List<BulletFunctions> familiars_Bullet = new List<BulletFunctions>();
        for (int i = 0; i < transform.childCount; i++) {
            BulletFunctions b = transform.GetChild(i).gameObject.AddComponent<BulletFunctions>();
            familiars_Bullet.Add(b);
        }
        //使い魔のエフェクト
        familiar_Vanish_Effect = Resources.Load("Effect/FamiliarVanishEffect") as GameObject;

        //攻撃
        switch (kind) {
            case 1: StartCoroutine("Shoot_Red_Bullet");
                break;
            case 2: StartCoroutine("Shoot_Blue_Bullet");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

    }


    //赤弾妖精
    private IEnumerator Shoot_Red_Bullet() {
        yield return new WaitForSeconds(1.0f);
        //全方位弾
        _bullet.Set_Bullet(Resources.Load("Bullet/RedRiceBullet") as GameObject);
        _bullet.Diffusion_Bullet(24, 70, 0, 5.0f);
        //奇数弾
        for (int i = 0; i < 10; i++) {
            _bullet.Odd_Num_Bullet(3, 30f, 90f, 5.0f);
            yield return new WaitForSeconds(1.5f);
        }
    }


    //青弾妖精
    private IEnumerator Shoot_Blue_Bullet() {
        yield return new WaitForSeconds(1.0f);
        //全方位弾
        _bullet.Set_Bullet(Resources.Load("Bullet/BlueRiceBullet") as GameObject);
        _bullet.Diffusion_Bullet(24, 70, 0, 5.0f);
        //自機狙い弾
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < transform.childCount; j++) {
                BulletFunctions f_Bullet = transform.GetChild(j).GetComponent<BulletFunctions>();
                f_Bullet.Set_Bullet(Resources.Load("Bullet/BlueBullet") as GameObject);
                f_Bullet.Odd_Num_Bullet(1, 0, 90, 5.0f);
                f_Bullet.Odd_Num_Bullet(1, 0, -90, 5.0f);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    //消滅時
    protected override void Vanish() {
        //使い魔の消滅
        for(int i = 0; i < transform.childCount; i++) {
            GameObject effect = Instantiate(familiar_Vanish_Effect);
            effect.transform.position = transform.GetChild(i).position;
        }
        base.Vanish();

    }




}
