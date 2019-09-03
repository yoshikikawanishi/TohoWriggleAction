using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kisume : TalkCharacter {

    //コンポーネント
    private Animator _anim;


	// Use this for initialization
	new void Start () {
        base.Start();
        _anim = GetComponent<Animator>();
    }

    //会話の始めと終わりにアニメーション変更
    protected override IEnumerator Talk() {
        //ヤマメの店に行ったことがあるかどうか
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_Visited("UnderGroundScene")) {
            Set_Status("KisumeText", 1, 6, new Vector2(32f, 48f));
        }
        else {
            Set_Status("KisumeText", 8, 12, new Vector2(32f, 48f));
        }
        is_Talking = true;
        end_Talk = false;
        _anim.SetBool("AppearBool", true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(base.Talk());
        yield return new WaitUntil(End_Talk);
        _anim.SetBool("AppearBool", false);
    }
}
