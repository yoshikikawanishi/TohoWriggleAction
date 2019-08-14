using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSpark : MonoBehaviour {

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
        //発射
        while(transform.localScale.y < 1.5f) {
            transform.localScale += new Vector3(0, 0.04f);
            yield return new WaitForSeconds(0.016f);
        }
        yield return new WaitForSeconds(1.0f);
        //揺らす
        camera_Shake.Shake(4.0f, 1f, false);
        StartCoroutine(Shake(4.0f, 1f));
        //効果音
        GetComponent<AudioSource>().Play();
        //広げる
        while (transform.localScale.x < 0.3f) {
            transform.localScale += new Vector3(0.008f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        //当たり判定を濃くしながら広げる
        laser_Core.SetActive(true);
        while(transform.localScale.x <= 1.0f) {
            if (core_Sprite.color.a < 0.6f) {
                core_Sprite.color += new Color(0, 0, 0, 0.005f);
            }
            transform.localScale += new Vector3(0.008f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        yield return new WaitForSeconds(1.5f);
        //狭める
        while(transform.localScale.x >= 0) {
            if(core_Sprite.color.a >= 0) {
                core_Sprite.color += new Color(0, 0, 0, -0.02f);
            }
            else {
                laser_Core.SetActive(false);
            }
            transform.localScale += new Vector3(-0.015f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        Destroy(gameObject);
    }


    //揺らす
    private IEnumerator Shake(float duraction, float magnitude) {
        var pos = transform.localPosition;
        var elapsed = 0f;
        while (elapsed < duraction) {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = pos;
    }
}
