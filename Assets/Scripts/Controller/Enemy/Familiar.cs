using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;

	// Use this for initialization
	void Start () {
        //コンポーネント
        _sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Become_Invisible();
	}

    //LShiftで透明化
    private void Become_Invisible() {
        //LShiftで透明化
        if (Input.GetButtonDown("Fly")) {
            _sprite.color = new Color(1, 1, 1, 0);
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
        if (Input.GetButtonUp("Fly")) {
            _sprite.color = new Color(1, 1, 1, 1);
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
            UsualSoundManager.Familiar_Appear_Sound();
        }       
    }
}
