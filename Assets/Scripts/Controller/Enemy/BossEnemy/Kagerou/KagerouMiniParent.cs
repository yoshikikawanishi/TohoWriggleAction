using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWolfParent : MonoBehaviour {

    private List<GameObject> mini_Wolfs = new List<GameObject>();
    private List<MiniWolf> wolfs_Controller = new List<MiniWolf>();

    private GameObject kagerou;


	// Use this for initialization
	void Start () {
        kagerou = GameObject.Find("Kagerou");
	}


    public IEnumerator Do_Wolf_Attack() {
        //子供の取得
        for(int i = 0; i < transform.childCount; i++) {
            mini_Wolfs[i] = transform.GetChild(i).gameObject;
            wolfs_Controller[i] = mini_Wolfs[i].GetComponent<MiniWolf>();
        }
        while (true) {
            //狼に変身
            for(int i = 0; i < transform.childCount; i++) {
                yield return new WaitForSeconds(1.5f);
                //エフェクト
                kagerou.GetComponent<KagerouController>().Roar();
                //wolfs_Controller.transform_Effect.Play();
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
