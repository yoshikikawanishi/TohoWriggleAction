using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosureStar : MonoBehaviour {

    private SpriteRenderer _sprite;

	private enum STATE {
        up,
        right,
        down,
        left,
    }

    [SerializeField] private STATE default_State;
    private STATE now_State;

    private Vector3 direction;
    
    private float speed = 0.5f;
    private float size = 111f;


    //Start
    private void Awake() {
        now_State = default_State;
    }


    // Update is called once per frame
    void Update () {
        //移動
        Transition();
        //状態の遷移
        Change_State();
	}


    //状態の遷移
    private void Change_State() {
        //上から右
        if (transform.localPosition.x >= size && transform.localPosition.y >= size && now_State == STATE.up) {
            transform.localPosition = new Vector3(size, size) + new Vector3(0.5f, -0.5f);
            now_State = STATE.right;
        }
        //右から下
        else if (transform.localPosition.x >= size && transform.localPosition.y <= -size && now_State == STATE.right) {
            transform.localPosition = new Vector3(size, -size) + new Vector3(-0.5f, -0.5f);
            now_State = STATE.down;
        }
        //下から左
        else if (transform.localPosition.x <= -size && transform.localPosition.y <= -size && now_State == STATE.down) {
            transform.localPosition = new Vector3(-size, -size) + new Vector3(-0.5f, 0.5f);
            now_State = STATE.left;
        }
        //左から上
        else if (transform.localPosition.x <= -size && transform.localPosition.y >= size && now_State == STATE.left) {
            transform.localPosition = new Vector3(-size, size) + new Vector3(0.5f, 0.5f);
            now_State = STATE.up;
        }
    }


    //移動
    private void Transition() {
        switch (now_State) {
            case STATE.up: direction = new Vector2(1, 0); break;
            case STATE.right: direction = new Vector2(0, -1); break;
            case STATE.down: direction = new Vector2(-1, 0); break;
            case STATE.left: direction = new Vector2(0, 1); break;
        }
        transform.localPosition += direction * speed * Time.timeScale;
    }


    //出現
    public IEnumerator Appear() {
        _sprite = GetComponent<SpriteRenderer>();
        StopCoroutine(Disappear());
        _sprite.color = new Color(1, 1, 1, 0);
        while(_sprite.color.a <= 1) {
            _sprite.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.015f);
        }
        _sprite.color = new Color(1, 1, 1, 1);
    }

    //消滅
    public IEnumerator Disappear() {
        GetComponent<CircleCollider2D>().enabled = false;
        _sprite = GetComponent<SpriteRenderer>();
        StopCoroutine(Appear());
        _sprite.color = new Color(1, 1, 1, 1);
        while (_sprite.color.a >= 0) {
            _sprite.color += new Color(0, 0, 0, -0.01f);
            yield return new WaitForSeconds(0.015f);
        }
        _sprite.color = new Color(1, 1, 1, 0);
        
    }
}
