using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    //エフェクト
    [SerializeField] private GameObject effect;

    public void Destroy_Object() {
        GameObject e = Instantiate(effect);
        e.transform.position = transform.position;
        Destroy(e, 1.0f);
        Destroy(gameObject);
    }
	
}
