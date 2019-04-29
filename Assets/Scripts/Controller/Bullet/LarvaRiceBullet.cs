using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaRiceBullet : MonoBehaviour {

    GameObject[] rice_Bullets = new GameObject[3];


	// Use this for initialization
	void Start () {
        rice_Bullets[0] = transform.GetChild(0).gameObject;
        rice_Bullets[1] = transform.GetChild(1).gameObject;
        rice_Bullets[2] = transform.GetChild(2).gameObject;
        StartCoroutine("Differ_Velocity");
	}

    private IEnumerator Differ_Velocity() {
        yield return null;
        Rigidbody2D _rigid = GetComponent<Rigidbody2D>();
        for (int i = 0; i < 3; i++) {
            if (rice_Bullets[i] != null) {
                rice_Bullets[i].GetComponent<Rigidbody2D>().velocity = _rigid.velocity * (1 - 0.1f * i);
            }
        }
    }
	
	
}
