using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAroundParent : MonoBehaviour {

    //親
    private GameObject parent;
    //初期位相
    [SerializeField] private float default_Angle;
    //半径
    [SerializeField] private float radius = 48f;
    //位相
    private float angle = 0;
    private float time = 0;

    // Use this for initialization
    void Start () {
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
