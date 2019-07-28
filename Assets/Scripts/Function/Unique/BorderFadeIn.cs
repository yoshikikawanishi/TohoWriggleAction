using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BorderFadeIn : MonoBehaviour {

    [SerializeField] private GameObject border_Sprites;
    private float speed;


    public void Start_Fade_In(float speed) {
        this.speed = speed;
        StartCoroutine("Fade_In");
    }

    private IEnumerator Fade_In() {
        //子供の取得
        int count = border_Sprites.transform.childCount;
        GameObject[] border = new GameObject[count];
        for(int i = 0; i < count; i++) {
            border[i] = border_Sprites.transform.GetChild(i).gameObject;
        }
        
        float width = border[0].transform.localScale.y;
        float length = border[0].transform.localScale.x;
        
        //縮小する
        while (width >= 0) {
            for(int i = 0; i < count; i++) {
                border[i].transform.localScale = new Vector3(length, width, 1);
            }
            width -= speed;
            yield return null;
        }
        border_Sprites.SetActive(false);
    }


}
