using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;
    private Renderer _renderer;

    private Color default_Color;


	// Use this for initialization
	void Start () {
        //コンポーネント
        _sprite = GetComponent<SpriteRenderer>();
        _renderer = GetComponent<Renderer>();

        default_Color = _sprite.color;
    }

    // Update is called once per frame
    void Update() {
        if (_renderer.isVisible) {
            Become_Invisible();
        }
    }

    //LShiftで透明化
    private void Become_Invisible() {
        //LShiftで透明化
        if (InputManager.Instance.GetKey(MBLDefine.Key.Fly) && _sprite.color.a != 0) {
            _sprite.color = new Color(1, 1, 1, 0);
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
        if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Fly)) {
            _sprite.color = default_Color;
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
            UsualSoundManager.Familiar_Appear_Sound();
        }       
    }
}
