using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMasterSpark : MonoBehaviour {

    //コア
    private GameObject laser_Core;
    private SpriteRenderer core_Sprite;

    //画面を揺らす
    private CameraShake camera_Shake;


    // Use this for initialization
    void Start () {
        //取得
        laser_Core = transform.GetChild(4).gameObject;
        core_Sprite = laser_Core.GetComponent<SpriteRenderer>();
        laser_Core.SetActive(false);
        camera_Shake = GetComponent<CameraShake>();
        //動き
        StartCoroutine("Action");
    }


    //動き
    private IEnumerator Action() {
        GetComponents<AudioSource>()[1].Play();
        //発射
        while (transform.localScale.y < 1.8f) {
            transform.localScale += new Vector3(0, 0.04f);
            yield return new WaitForSeconds(0.016f);
        }
        yield return new WaitForSeconds(0.3f);
        //揺らす
        camera_Shake.Shake(2.0f, 1f, true);
        //効果音
        GetComponents<AudioSource>()[0].Play();
        //広げる
        while (transform.localScale.x < 0.15f) {
            transform.localScale += new Vector3(0.008f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        //当たり判定を濃くしながら広げる
        laser_Core.SetActive(true);
        while (transform.localScale.x <= 0.5f) {
            if (core_Sprite.color.a < 0.6f) {
                core_Sprite.color += new Color(0, 0, 0, 0.01f);
            }
            transform.localScale += new Vector3(0.008f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        yield return new WaitForSeconds(0.7f);
        //狭める
        while (transform.localScale.x >= 0) {
            if (core_Sprite.color.a >= 0) {
                core_Sprite.color += new Color(0, 0, 0, -0.02f);
            }
            else {
                laser_Core.SetActive(false);
            }
            transform.localScale += new Vector3(-0.01f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        Destroy(gameObject);
    }

}
