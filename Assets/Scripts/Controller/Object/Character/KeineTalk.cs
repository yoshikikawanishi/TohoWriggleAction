using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineTalk : TalkCharacter {

    //会話後、アイテムを出す
    protected override IEnumerator Talk() {
        StartCoroutine(base.Talk());
        yield return new WaitUntil(End_Talk);
        if (GetComponent<KeineController>().Get_Is_Catched()) {
            GameObject item = Instantiate(Resources.Load("LifeUpItem") as GameObject);
            item.transform.position = transform.position + new Vector3(0, 32f);
        }
    }
}
