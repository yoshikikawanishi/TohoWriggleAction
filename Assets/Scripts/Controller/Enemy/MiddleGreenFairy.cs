using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleGreenFairy : Enemy {

    //種類
    [SerializeField] private int kind;
    //スクリプト
    private BulletScrollFunctions _bullet;

    //使い魔の消滅エフェクト
    private GameObject familiar_Vanish_Effect;


	// Use this for initialization
	new void Start () {
        base.Start();
        //スクリプト
        _bullet = gameObject.AddComponent<BulletScrollFunctions>();

        //使い魔にBulletFunctionsをアタッチ
        List<BulletScrollFunctions> familiars_Bullet = new List<BulletScrollFunctions>();
        for (int i = 0; i < transform.childCount; i++) {
            BulletScrollFunctions b = transform.GetChild(i).gameObject.AddComponent<BulletScrollFunctions>();
            familiars_Bullet.Add(b);
        }
        //使い魔のエフェクト
        familiar_Vanish_Effect = Resources.Load("Effect/FamiliarVanishEffect") as GameObject;

        UsualSoundManager.Small_Shot_Sound();

        //行動
        switch (kind) {
            case 1:
                StartCoroutine("Shoot_Red_Bullet");
                break;
            case 2:
                StartCoroutine("Shoot_Blue_Bullet");
                break;
        }
    }


    //赤弾妖精
    private IEnumerator Shoot_Red_Bullet() {
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0) {
            yield return null;
            transform.position += new Vector3(0, -speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        yield return new WaitForSeconds(1.0f);
        //全方位弾
        _bullet.Set_Bullet(Resources.Load("Bullet/RedRiceBullet") as GameObject);
        _bullet.Diffusion_Bullet(16, 70, 0, 7.0f);
        UsualSoundManager.Shot_Sound();
        //奇数弾
        for (int i = 0; i < 4; i++) {
            _bullet.Odd_Num_Bullet(3, 30f, 90f, 7.0f);
            yield return new WaitForSeconds(12f/7f);
        }
    }


    //青弾妖精
    private IEnumerator Shoot_Blue_Bullet() {
        //下から登場
        float speed = 4.5f;
        while (speed >= 0) {
            yield return null;
            transform.position += new Vector3(0, speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        yield return new WaitForSeconds(1.0f);
        //全方位弾
        _bullet.Set_Bullet(Resources.Load("Bullet/BlueRiceBullet") as GameObject);
        _bullet.Diffusion_Bullet(16, 70, 0, 7.0f);
        UsualSoundManager.Shot_Sound();
        //自機狙い弾
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < transform.childCount; j++) {
                BulletScrollFunctions f_Bullet = transform.GetChild(j).GetComponent<BulletScrollFunctions>();
                f_Bullet.Set_Bullet(Resources.Load("Bullet/BlueBullet") as GameObject);
                f_Bullet.Odd_Num_Bullet(1, 0, 90, 7.0f);
                f_Bullet.Odd_Num_Bullet(1, 0, -90, 7.0f);
            }
            yield return new WaitForSeconds(12f/7f);
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
