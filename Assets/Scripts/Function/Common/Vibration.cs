using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour {

    //初期位置
    private float default_Y;
    [SerializeField] private float amplitude;
    [SerializeField] private float anguler_Velocity;
    float time = 0;

	// Use this for initialization
	void Start () {
        default_Y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, default_Y + Mathf.Sin(anguler_Velocity * time) * amplitude);
	}
}
