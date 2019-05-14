using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectController : MonoBehaviour {

    private SpriteRenderer _sprite;

    private void Awake() {
        _sprite = GetComponent<SpriteRenderer>();
    }


    //OnEnable
    private void OnEnable() {
        transform.localScale = new Vector3(0, 0, 1);
        _sprite.color = new Color(1, 1, 1, 0.7f);
        StartCoroutine("Spread");
    }


    private IEnumerator Spread() {
        while(transform.localScale.x < 0.3f) {
            transform.localScale += new Vector3(0.05f, 0.05f, 0) * Time.timeScale;
            _sprite.color -= new Color(0, 0, 0, 0.05f);
            yield return null;
        }
    }

}
