using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour {

    //コンポーネント
    private Enemy _enemy;
    private AudioSource repel_Sound;

    //弾
    [SerializeField] private GameObject bullet;

    //行動開始
    private bool start_Action = false;
    //無敵かどうか
    private bool is_Invincible = true;

    //Start
    private void Start() {
        //取得
        _enemy = GetComponent<Enemy>();
        repel_Sound = GetComponents<AudioSource>()[1];
        //無敵化
        _enemy.Set_Is_Invincible(true);
    }

    //OnBecomeVisible
    private void OnBecameVisible() {
        if (!start_Action) {
            start_Action = true;
            StartCoroutine("Shot");
        }
    }


    //OnBecomeInvisible
    private void OnBecameInvisible() {
        if (start_Action) {
            start_Action = false;
            StopCoroutine("Shot");
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (is_Invincible) {
            if (collision.tag == "PlayerBulletTag" || collision.tag == "PlayerAttackTag" || collision.tag == "BeetleBulletTag") {
                repel_Sound.Play();
            }
        }
    }


    //行動
    private IEnumerator Shot() {
        //初期設定
        Animator _anim = GetComponent<Animator>();
        BulletFunctions _bullet = gameObject.AddComponent<BulletFunctions>();
        _bullet.Set_Bullet(bullet);

        yield return new WaitForSeconds(1.0f);

        while (true) {
            _anim.SetBool("ShotBool", true);
            _enemy.Set_Is_Invincible(false);
            is_Invincible = false;

            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < 3; i++) {
                _bullet.Shoot_Bullet(new Vector2(-100f * transform.localScale.x, 0), 0);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);

            _enemy.Set_Is_Invincible(true);
            is_Invincible = true;
            _anim.SetBool("ShotBool", false);

            yield return new WaitForSeconds(1.8f);
        }
    }
}
