using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLog : MonoBehaviour {

    private Renderer _renderer;
    //エフェクト
    private AudioSource landing_Sound;

    
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
        landing_Sound = GetComponent<AudioSource>();
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "GroundTag") {
            if (_renderer.isVisible) {
                landing_Sound.Play();
            }
        }
    }

}
