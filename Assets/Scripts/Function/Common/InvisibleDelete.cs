using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleDelete : MonoBehaviour {

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
