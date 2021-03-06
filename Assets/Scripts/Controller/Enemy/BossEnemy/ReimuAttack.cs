﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//霊夢の攻撃をするクラス
public class ReimuAttack : MonoBehaviour {

    private bool start_Phase1_Routine = true;
    private bool start_Phase2_Routine = true;

    //ゆかりの手
    [SerializeField] private GameObject[] yukari_Hand = new GameObject[2];


    //フェーズ1
    public void Phase1() {
        if (start_Phase1_Routine) {
            start_Phase1_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }


    //フェーズ1のルーチン
    private IEnumerator Phase1_Routine() {
        MoveBetweenTwoPoints _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        BulletFunctions _bullet = gameObject.AddComponent<BulletFunctions>();
        ReimuController _controller = GetComponent<ReimuController>();
        while (true) {
            //移動
            _controller.Change_Parameter("DashBool");
            _move.Start_Move(new Vector3(180f, -64f), 64f, 0.014f);
            yield return new WaitUntil(_move.End_Move);
            _controller.Change_Parameter("AttackBool");
            transform.localScale = new Vector3(-1, 1, 1);
            //バックデザイン
            transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            //ホーミング弾
            GameObject homing_Bullet = Resources.Load("Bullet/ReimuHomingBullet") as GameObject;
            for (int i = 0; i < 4; i++) {
                _bullet.Set_Bullet(homing_Bullet);
                _bullet.Some_Way_Bullet(4, 150f, 0, 30f, 4.0f);
                UsualSoundManager.Shot_Sound();
                _move.Start_Random_Move(32f, 0.03f);
                yield return new WaitUntil(_move.End_Move);
            }
            //陰陽玉
            _move.Start_Move(new Vector3(150f, 0, 0), 0, 0.02f);
            _move.End_Move();
            //溜め
            _controller.Change_Parameter("AvoidBool");
            GameObject effect = Instantiate(Resources.Load("Effect/PowerChargeEffects") as GameObject);
            effect.transform.position = transform.position;
            effect.transform.SetParent(transform);
            Destroy(effect, 3.0f);
            yield return new WaitForSeconds(3.0f);
            //発射
            _controller.Change_Parameter("AttackBool");
            GameObject yin_Bullet = Resources.Load("Bullet/BigYinBallBullet") as GameObject;
            _bullet.Set_Bullet(yin_Bullet);
            _bullet.Odd_Num_Bullet(1, 0, 150f, 0);
            UsualSoundManager.Shot_Sound();
            UsualSoundManager.Small_Shot_Sound();
            GameObject spread_Effect = Instantiate(Resources.Load("Effect/PowerSpreadEffect") as GameObject);
            spread_Effect.transform.position = transform.position;
            Destroy(spread_Effect, 3.0f);
            yield return new WaitForSeconds(2.0f);
        }
    }

    //フェーズ2
    public void Phase2() {
        if (start_Phase2_Routine) {
            start_Phase2_Routine = false;
            StopAllCoroutines();
            StartCoroutine("Phase2_Routine");
        }
    }

    //フェーズ2のルーチン
    private IEnumerator Phase2_Routine() {
        //初期設定
        ReimuController _controller = GetComponent<ReimuController>();
        MoveBetweenTwoPoints _move = GetComponent<MoveBetweenTwoPoints>();
        BulletPoolFunctions _bullet_Pool = gameObject.AddComponent<BulletPoolFunctions>();
        ScatterPoolBullet _scatter_Bullet = gameObject.AddComponent<ScatterPoolBullet>();
        //バックデザイン
        transform.GetChild(1).gameObject.SetActive(false);
        //弾のオブジェクトプール
        ObjectPool white_Talisman_Pool = gameObject.AddComponent<ObjectPool>();
        ObjectPool red_Talisman_Pool = gameObject.AddComponent<ObjectPool>();
        ObjectPool red_Bullet_Pool = gameObject.AddComponent<ObjectPool>();
        Create_Bullet_Pools(white_Talisman_Pool, red_Talisman_Pool, red_Bullet_Pool);
        //移動
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(1.0f);
        _move.Start_Move(new Vector3(140f, -12f), 32f, 0.02f);
        yield return new WaitUntil(_move.End_Move);
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        //バックデザイン
        transform.GetChild(1).localScale = new Vector3(0, 0, 1);
        transform.GetChild(1).gameObject.SetActive(true);
        //紫
        StartCoroutine(Yukari_Hand());
        while (true) {
            //全方位弾
            _controller.Change_Parameter("AttackBool");
            _bullet_Pool.Set_Bullet_Pool(white_Talisman_Pool);
            float center_Angle = Random.Range(-90, 90);
            _bullet_Pool.Diffusion_Bullet(24, 90f, center_Angle, 7.0f);
            UsualSoundManager.Shot_Sound();
            yield return new WaitForSeconds(0.3f);
            center_Angle += 7f;
            _bullet_Pool.Set_Bullet_Pool(red_Talisman_Pool);
            for (int i = 1; i < 4; i++) {
                _bullet_Pool.Diffusion_Bullet(24, (90f - i * 5), (center_Angle + i * 3), 7.0f);
            }
            UsualSoundManager.Shot_Sound();
            yield return new WaitForSeconds(0.5f);
            //弾をばらまきながら移動
            _controller.Change_Parameter("DashBool");
            _scatter_Bullet.Set_Bullet_Pool(red_Bullet_Pool);
            _scatter_Bullet.Start_Scatter(75f, 50f, 2.0f, 9.0f);
            UsualSoundManager.Small_Shot_Sound();
            Vector3 next_Pos;
            if (transform.position.y < 0) {
                next_Pos = new Vector3(140f, 24f);
            }
            else {
                next_Pos = new Vector3(140f, -48f);
            }
            _move.Start_Move(next_Pos, 0, 0.02f);
            yield return new WaitUntil(_move.End_Move);
            _scatter_Bullet.Stop_Scatter();
            _controller.Change_Parameter("AvoidBool");

            yield return new WaitForSeconds(3.0f);
        }
    }


    //フェーズ2オブジェクトプール用
    private void Create_Bullet_Pools(ObjectPool pool1, ObjectPool pool2, ObjectPool pool3) {
        GameObject white_Talisman_Bullet = Resources.Load("Bullet/PooledBullet/WhiteTalismanBullet") as GameObject;
        GameObject red_Talisman_Bullet = Resources.Load("Bullet/PooledBullet/RedTalismanBullet") as GameObject;
        GameObject red_Bullet = Resources.Load("Bullet/PooledBullet/RedBulletPool") as GameObject;
        pool1.CreatePool(white_Talisman_Bullet, 32);
        pool2.CreatePool(red_Talisman_Bullet, 32);
        pool3.CreatePool(red_Bullet, 30);
    }


    //フェーズ2紫
    private IEnumerator Yukari_Hand() {
        //初期設定

        Vector2[] yukari_Default_Pos = new Vector2[2];
        
        for (int i = 0; i < 2; i++) {
            yukari_Default_Pos[i] = yukari_Hand[i].transform.position;
        }

        yield return new WaitForSeconds(2.0f);
        while (true) {
            //地面動かす
            StartCoroutine(Yukari_Hand_Move(yukari_Default_Pos[0] + new Vector2(0, 48f), 0));
            yield return new WaitForSeconds(5.0f);
            //画面端動かす
            StartCoroutine(Yukari_Hand_Move(yukari_Default_Pos[1] + new Vector2(32, -48f), 1));
            yield return new WaitForSeconds(5.0f);
            //地面戻す
            StartCoroutine(Yukari_Hand_Move(yukari_Default_Pos[0], 0));
            yield return new WaitForSeconds(5.0f);
            //画面端戻す
            StartCoroutine(Yukari_Hand_Move(yukari_Default_Pos[1], 1));
            yield return new WaitForSeconds(5.0f);

        }
    }

    //紫の手の移動
    private IEnumerator Yukari_Hand_Move(Vector2 next_Pos, int number) {
        SpriteRenderer yukari_Sprite = yukari_Hand[number].GetComponent<SpriteRenderer>();
        MoveBetweenTwoPoints yukari_Move = yukari_Hand[number].GetComponent<MoveBetweenTwoPoints>();
        //出現
        while (yukari_Sprite.color.a <= 1) {
            yukari_Sprite.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.015f);
        }
        //移動
        yukari_Move.Start_Move(next_Pos, 0, 0.01f);
        yield return new WaitUntil(yukari_Move.End_Move);
        //消える
        while (yukari_Sprite.color.a >= 0) {
            yukari_Sprite.color += new Color(0, 0, 0, -0.01f);
            yield return new WaitForSeconds(0.015f);
        }

    }

}
