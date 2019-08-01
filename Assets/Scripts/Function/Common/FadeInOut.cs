using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour {

    //フィールド
    public GameObject cover_Object;
    private SpriteRenderer obj_Sprite;
    public float speed = 0;


	// Use this for initialization
	void Awake () {
        obj_Sprite = cover_Object.GetComponent<SpriteRenderer>();
    }
	

	//フェードイン
    public void Start_Fade_In() {
        this.speed = speed;
        StartCoroutine("Fade_In");
    }
    private IEnumerator Fade_In() {
        while(obj_Sprite.color.a >= 0) {
            obj_Sprite.color += new Color(0, 0, 0, -speed);
            yield return null;
        }
    }


    //フェードアウト
    public void Start_Fade_Out() {
        this.speed = speed;
        StartCoroutine("Fade_Out");
    }
    private IEnumerator Fade_Out() {
        while (obj_Sprite.color.a <= 1) {
            obj_Sprite.color += new Color(0, 0, 0, speed);
            yield return null;
        }
    }

}
