using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleGreenFairy : Enemy {

    //使い魔
    private List<GameObject> familiars = new List<GameObject>();
    //使い魔の消滅エフェクト
    private GameObject familiar_Vanish_Effect;


	// Use this for initialization
	new void Start () {
        //コンポーネントの取得
        base.Start();
        //使い魔の取得
        for(int i = 0; i < transform.childCount; i++) {
            familiars.Add(transform.GetChild(i).gameObject);
            Debug.Log(familiars);
        }
        //使い魔の消滅エフェクト
        familiar_Vanish_Effect = Resources.Load("Effect/FamiliarVanishEffect") as GameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //消滅時の処理
    override protected void Vanish() {
        //本体の消滅
        base.Vanish();
        //子供の消去とエフェクトボム
        for(int i = 0; i < familiars.Count; i++) {
            GameObject bomb = Instantiate(familiar_Vanish_Effect);
            bomb.transform.position = transform.GetChild(0).position;
            Destroy(bomb, 1.0f);
            Destroy(familiars[i]);
        }
    }
}
