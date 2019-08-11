using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarParentEnemy : Enemy {

    //消滅時
    protected override void Vanish() {
        //使い魔を消滅
        GameObject familiar_Vanish_Effect = Resources.Load("Effect/FamiliarVanishEffect") as GameObject;
        for (int i = 0; i < transform.childCount; i++) {
            GameObject effect = Instantiate(familiar_Vanish_Effect);
            effect.transform.position = transform.GetChild(i).position;
        }
        base.Vanish();
    }

}
