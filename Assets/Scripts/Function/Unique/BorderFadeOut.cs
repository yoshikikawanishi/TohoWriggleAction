using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFadeOut : MonoBehaviour {

    [SerializeField] private GameObject border_Sprites;
    public float max_Width = 32f;
    private float speed;


    public void Start_Fade_Out(float speed) {
        this.speed = speed;
        StartCoroutine("Fade_Out");
    }

    private IEnumerator Fade_Out() {
        //子供の取得
        int count = border_Sprites.transform.childCount;
        GameObject[] border = new GameObject[count];
        for (int i = 0; i < count; i++) {
            border[i] = border_Sprites.transform.GetChild(i).gameObject;
        }

        float width = 0;
        float length = border[0].transform.localScale.x;

        //縮小する
        while (width <= max_Width) {
            for (int i = 0; i < count; i++) {
                border[i].transform.localScale = new Vector3(length, width, 1);
            }
            width += speed;
            yield return null;
        }
    }
}
