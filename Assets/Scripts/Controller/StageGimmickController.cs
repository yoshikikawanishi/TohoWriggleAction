using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGimmickController : MonoBehaviour {

    //種類分け
    [SerializeField] private int kind_Num = 0;

    //1面動く床用
    //位相
    private float phase = 0;
    //初期位置
    private Vector3 default_Pos;


	// Use this for initialization
	void Start () {
        switch (kind_Num) {
            case 1: Stage1_MoveGround1_Start(); break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    //FixedUpdate
    private void FixedUpdate() {
        switch (kind_Num) {
            case 1: Stage1_MoveGround1(); break;    //1面動く床
        }
    }


    //1面動く床用
    private void Stage1_MoveGround1_Start() {
        default_Pos = transform.position;
    }
    private void Stage1_MoveGround1() {
        transform.position = default_Pos + new Vector3(Mathf.Sin(phase) * 64f, 0, 0);
        phase += 0.01f;     
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //1面動く床用
        if (collision.tag == "PlayerFootTag" && kind_Num == 1) {
            collision.transform.parent.SetParent(transform);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        //1面動く床用
        if(collision.tag == "PlayerFootTag" && kind_Num == 1) {
            collision.transform.parent.SetParent(null);
        }
    }

}
