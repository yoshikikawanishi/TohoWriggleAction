using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAroundFamiliar : Enemy {
    
    //親
    private GameObject parent;
    
    //速度
    [SerializeField] private float speed = 50f;
    //半径
    [SerializeField] private float radius = 48f;
    //初期位相
    [SerializeField] private float default_Angle = 0;
    //位相
    private float angle = 0;
    private float time = 0;


    // Use this for initialization
    new void Start () {
        //取得        
        base.Start();
        parent = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //親の周りをまわる
        time += Time.deltaTime;
        angle = (default_Angle + time * speed) * Mathf.PI / 180f;
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }


   

}
