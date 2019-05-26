using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairBallController : MonoBehaviour {

    //生成位置
    private float default_Height;


	// Use this for initialization
	void Start () {
        //生成位置
        default_Height = transform.position.y;
        //消滅
        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(default_Height < 0) {
            transform.position += new Vector3(-3, 2, 0);
        }
        else {
            transform.position += new Vector3(-3, -2, 0);
        }
	}
}
