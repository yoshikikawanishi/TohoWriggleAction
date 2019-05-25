using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAroundFamiliar : Enemy {

    //親
    private GameObject parent;
    //初期位相
    [SerializeField] private float default_Angle = 0;
    //位相
    private float angle = 0;
    private float time = 0;


    // Use this for initialization
    new void Start () {
        base.Start();
        parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //親の周りをまわる
        time += Time.deltaTime;
        angle = (default_Angle + time * 50f) * Mathf.PI / 180f;
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 48f;
	} 
}
