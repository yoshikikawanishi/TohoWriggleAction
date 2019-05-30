using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAroundFamiliar : Enemy {

    //親
    private GameObject parent;
    //初期位相
    [SerializeField] private float default_Angle = 0;
    //位相
    private float angle = 0;
    private float time = 0;
    //透明かどうか
    private bool is_Visible = true;


    // Use this for initialization
    new void Start () {
        //取得        
        base.Start();
        parent = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //親の周りをまわる
        time += Time.deltaTime;
        angle = (default_Angle + time * 50f) * Mathf.PI / 180f;
        transform.position = parent.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 48f;
        Become_Invisible();
    }


    //LShiftで透明化
    private void Become_Invisible() {
        //LShiftで透明化
        if (Input.GetKey(KeyCode.LeftShift) && is_Visible) {
            is_Visible = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            is_Visible = true;
        }
        if (is_Visible && _sprite.color.a != 1) {
            _sprite.color = base.default_Color;
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        }
        else if (!is_Visible && _sprite.color.a != 0) {
            _sprite.color = new Color(1, 1, 1, 0);
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
    }

}
