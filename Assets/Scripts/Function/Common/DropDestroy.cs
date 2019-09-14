using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDestroy : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	    if(transform.position.y < -200f) {
            Destroy(gameObject);
        }	
	}
}
