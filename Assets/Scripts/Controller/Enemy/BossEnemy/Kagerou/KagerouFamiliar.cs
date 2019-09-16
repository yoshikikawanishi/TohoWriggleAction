using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouFamiliar : Enemy {


    protected override void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPと回復アイテムの生成
        Put_Out_Item();
        gameObject.SetActive(false);
    }

    private void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        is_Vanished = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

}
