using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosureStarsParent : MonoBehaviour {

    //移動
    private Vector2 start_Pos;
    [SerializeField] private Vector2 end_Pos;
    private float distance_Ratio = 0;


    //OnEnable
    private void OnEnable() {
        start_Pos = GameObject.FindWithTag("PlayerTag").transform.position;
        transform.position = start_Pos;
        distance_Ratio = 0;
        foreach(Transform child in gameObject.transform) {
            child.GetComponent<EnclosureStar>().StartCoroutine("Appear");
        }
    }
    

	// Update is called once per frame
	void Update () {
		if(distance_Ratio <= 1) {
            transform.position = Vector2.Lerp(start_Pos, end_Pos, distance_Ratio);
            distance_Ratio += 0.007f * Time.timeScale;
        }
	}


    //消滅
    public void Disappear() {
        Destroy(gameObject, 1.5f);
        foreach (Transform child in gameObject.transform) {            
            child.GetComponent<EnclosureStar>().StartCoroutine("Disappear");
        }
    }
}
