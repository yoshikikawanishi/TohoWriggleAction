using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBurstBullet : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        StartCoroutine("Shot");
	}
	
	//ショット
    private IEnumerator Shot() {
        yield return new WaitForSeconds(1.5f);
        BulletFunctions _bullet = GetComponent<BulletFunctions>();
        _bullet.Set_Bullet(Resources.Load("Bullet/BlueBullet") as GameObject);
        float center_Angle = Random.Range(-10f, 10f);
        for (int i = 0; i < 3; i++) {
            _bullet.Diffusion_Bullet(8, 80f - i * 15f, center_Angle + i * 5f, 7.0f);
        }
        UsualSoundManager.Shot_Sound();
        Destroy(gameObject);
    }
}
