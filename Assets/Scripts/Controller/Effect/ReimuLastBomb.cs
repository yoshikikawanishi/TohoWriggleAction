using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuLastBomb : MonoBehaviour {

    private SpriteRenderer _sprite;

	// Use this for initialization
	void Start () {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        //徐々に濃くする
        if(_sprite.color.a < 0.8f) {
            _sprite.color += new Color(0, 0, 0, 0.02f);
        }
        //徐々に青くする
        _sprite.color += new Color(-0.001f, -0.001f, 0, 0);
        //大きさを揺らめかせる
        transform.localScale = new Vector3(1.5f + Mathf.Sin(Time.time*3) * 0.3f, 1.5f + Mathf.Sin(Time.time*3) * 0.3f, 1);
	}
}
