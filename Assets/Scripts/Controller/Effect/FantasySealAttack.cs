using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FantasySealAttack : MonoBehaviour {

	
	
    //攻撃開始
    public void Start_Attack(Color start_Color) {
        //色変更
        ParticleSystem.MainModule pm = GetComponent<ParticleSystem>().main;
        pm.startColor = start_Color;
        ParticleSystem.MainModule pm_Body = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        pm_Body.startColor = start_Color;
        //溜め
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(Play_Fantasy_Seal_Attack());
    }


    //溜めエフェクト後、攻撃発生
    private IEnumerator Play_Fantasy_Seal_Attack() {
        yield return new WaitForSeconds(1.0f);
        transform.GetChild(0).gameObject.SetActive(true);
        UsualSoundManager.Shot_Sound();
        Destroy(gameObject, 0.4f);
    }
}
