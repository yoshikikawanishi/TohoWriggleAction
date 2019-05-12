using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongRail : MonoBehaviour {

    //スピード
    [SerializeField] private float speed = 1f;
    //ループするかどうか
    [SerializeField] private bool is_Loop = true;
    //進む順番
    [SerializeField] private Vector2[] directions;


	// Use this for initialization
	void Start () {
        StartCoroutine("Move");	
	}
	
	
    //移動
    private IEnumerator Move() {
        for (int i = 0; i < directions.Length; i++) {
            for (int j = 0; j < 32; j++) {
                Vector3 velocity = directions[i];
                transform.position += velocity;
                yield return new WaitForSeconds(1f / (32f * speed));
            }
        }
        //ループ
        if (is_Loop) {
            StartCoroutine("Move");
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(gameObject.transform);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(null);
        }
    }


}
