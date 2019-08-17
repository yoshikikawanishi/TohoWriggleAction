using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kosuzu : TalkCharacter {


    //会話終了後倒れる
    protected override IEnumerator Talk() {
        StartCoroutine(base.Talk());
        yield return new WaitUntil(End_Talk);
        //セリフ変える
        base.start_ID = 3;
        base.end_ID = 3;
    }
}
