using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBattleEnemy : MonoBehaviour {

    //コンポーネント
    private Renderer _renderer;

    //start
    private void Start() {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        if (!_renderer.isVisible) {
            Destroy(gameObject);
        }
	}


    
}
