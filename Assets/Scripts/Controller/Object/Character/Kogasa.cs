using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kogasa : TalkCharacter {

    //コンポーネント
    private Animator _anim;


	// Use this for initialization
	new void Start () {
        base.Start();
        //取得	
        _anim = GetComponent<Animator>();
	}


    //会話の始め、終わりにアニメーション変更
    protected override IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        _anim.SetBool("TurnBool", true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(base.Talk());
        yield return new WaitUntil(End_Talk);
        _anim.SetBool("TurnBool", false);
        //セリフ変える
        base.start_ID = 3;
        base.end_ID = 3;
    }
}
