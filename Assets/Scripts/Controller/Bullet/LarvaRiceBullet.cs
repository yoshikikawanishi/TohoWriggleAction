using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaRiceBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Differ_Velocity");
	}

    private IEnumerator Differ_Velocity() {
        yield return null;
        yield return null;
        Rigidbody2D _rigid = GetComponent<Rigidbody2D>();
        for (int i = 0; i < 3; i++) {
            GameObject bullet = transform.GetChild(i).gameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = _rigid.velocity * (1 - 0.1f * i);
        }
    }
	
	
}
