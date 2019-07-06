using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : MonoBehaviour {

    //カメラ
    private GameObject main_Camera;
    
    //初期位置
    private float default_Y;
    //振幅
    [SerializeField] private float amplitude = 32;
    //速さ
    [SerializeField] private float speed = 1;
    //方向
    private int direction = 1;


	// Use this for initialization
	void Start () {
        main_Camera = GameObject.FindWithTag("MainCamera");
        default_Y = transform.position.y;
    }


    //FixedUpdate
    private void FixedUpdate() {
        if(Mathf.Abs(main_Camera.transform.position.x - transform.position.x) < 240f) {
            transform.position += new Vector3(0, speed * direction);
            //方向転換
            if (transform.position.y >= default_Y + amplitude / 2 && direction == 1) {
                direction = -1;
            }
            else if(transform.position.y <= default_Y - amplitude / 2 && direction == -1) {
                direction = 1;
            }
        }
    }
}
