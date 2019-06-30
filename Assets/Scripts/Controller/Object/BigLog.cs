using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLog : MonoBehaviour {

    private Rigidbody2D _rigid;
    private Renderer _renderer;

    //エフェクト
    private AudioSource landing_Sound;

    [SerializeField] private float land_Height;
    [SerializeField] private float speed = 150f;
    
	
    // Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
        _rigid = GetComponent<Rigidbody2D>();
        landing_Sound = GetComponent<AudioSource>();
    }


    //Update
    private void Update() {
        if(transform.position.y < land_Height) {
            _rigid.velocity = new Vector2(speed, _rigid.velocity.y);
        }
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
