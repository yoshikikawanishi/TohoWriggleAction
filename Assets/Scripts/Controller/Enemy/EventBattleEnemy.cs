using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBattleEnemy : MonoBehaviour {

    //カメラ
    private GameObject main_Camera;

    //出現エフェクト
    [SerializeField] private GameObject appear_Effect;


    //start
    private void Start() {
        main_Camera = GameObject.FindWithTag("MainCamera");
        //出現エフェクト
        GameObject effect = Instantiate(appear_Effect, transform.position, new Quaternion(0,0,0,0));
        Destroy(effect, 2.5f);
        Invoke("Appear", 1.9f);
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update () {
        //画面外に出たら消す
        if (Mathf.Abs(transform.position.x - main_Camera.transform.position.x) > 240f) {
            Destroy(gameObject);
        }
	}


    //出現
    private void Appear() {
        gameObject.SetActive(true);
    }

    
    
}
