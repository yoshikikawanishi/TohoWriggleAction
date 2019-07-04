using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFamiliarBullet : MonoBehaviour {


	// Use this for initialization
	void Start () {
        StartCoroutine("Move");
	}


    //移動
    private IEnumerator Move() {
        yield return new WaitForSeconds(48f / 7f);
        for(float t = 0; t < 7.0f; t += Time.deltaTime) {
            transform.position += transform.right * -1.6f;
            yield return new WaitForSeconds(0.016f);
        }
        Destroy(gameObject);
    }
}
