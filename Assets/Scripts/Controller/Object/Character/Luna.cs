using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luna : TalkCharacter {

    //コンポーネント
    private Animator _anim;
    private Renderer _renderer;

    float time = 0;
    //転んだかどうか
    private bool is_Falled = false;


	// Use this for initialization
	new void Start () {
        base.Start();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();
    }

    //Update
    private void Update() {
        if (!is_Falled && _renderer.isVisible) {
            if (time < 2.0f) {
                time += Time.deltaTime;
                transform.position += new Vector3(0.3f, 0) * transform.localScale.x * Time.timeScale;
            }
            else {
                time = 0;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
    }

    //会話の始めにアニメーション変更
    protected override IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        _anim.SetBool("FallBool", true);
        is_Falled = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(base.Talk());
        yield return new WaitUntil(End_Talk);
        //会話できなくする
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
