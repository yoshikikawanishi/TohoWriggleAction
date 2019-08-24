using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1000f) ;
        Destroy(gameObject, 4f);
        UsualSoundManager.Laser_Sound();
    }

    
}
