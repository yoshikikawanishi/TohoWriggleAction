using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFrame : MonoBehaviour {

    [SerializeField] private GameObject main_Canvas;
    [SerializeField] private GameObject main_Boss_Canvas;
    [SerializeField] private GameObject frame_Canvas;
    [SerializeField] private GameObject frame_Boss_Canvas;

    //狭める
    public void Start_Narrow() {
        StartCoroutine("Narrow_Frame");
    }
    private IEnumerator Narrow_Frame() {
        Vector3 scale = transform.localScale;
        while(transform.localScale.x >= 1.005f) {
            scale *= 0.98f;
            if(scale.x <= 0.1f) {
                scale -= new Vector3(0.001f, 0.001f);
            }
            transform.localScale = new Vector3(1, 1, 1) + scale;
            yield return new WaitForSeconds(0.015f);
        }
        
        transform.localScale = new Vector3(1, 1, 1);
        //キャンバス
        frame_Canvas.SetActive(true);
        frame_Boss_Canvas.SetActive(true);
        main_Canvas.SetActive(false);
        main_Boss_Canvas.SetActive(false);
    }


    //広げる
    public void Start_Spread() {
        StartCoroutine("Spread_Frame");
    }
    private IEnumerator Spread_Frame() {
        //キャンバス
        main_Canvas.SetActive(true);
        main_Boss_Canvas.SetActive(true);
        frame_Canvas.SetActive(false);
        frame_Boss_Canvas.SetActive(false);
        
        while (transform.localScale.x <= 5) {
            transform.localScale += new Vector3(0.05f, 0.05f);
            yield return new WaitForSeconds(0.015f);
        }
        transform.localScale = new Vector3(3, 3, 1);
        
    }
}
