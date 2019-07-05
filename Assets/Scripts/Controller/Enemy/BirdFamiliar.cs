using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFamiliar : MonoBehaviour {

    //コンポーネント
    private AudioSource appear_Sound;
    
    
    // Use this for initialization
    void Start () {
        //コンポーネント
        appear_Sound = GetComponents<AudioSource>()[1];
        //出現
        appear_Sound.Play();
        //攻撃
        StartCoroutine("Shot");
        //消滅
        Destroy(gameObject, 7.0f);
	}
	

    //FixedUpDate
    private void FixedUpdate() {
        //行動
        transform.position += transform.right * -1.5f * Time.timeScale;
    }


    //攻撃
    private IEnumerator Shot() {
        yield return new WaitForSeconds(3f / 7f);
        GameObject bullet = Resources.Load("Bullet/BirdFamiliarBullet") as GameObject;
        for (int i = 0; i < 15; i++) {
            GameObject b = Instantiate(bullet);
            b.transform.position = transform.position;
            UsualSoundManager.Small_Shot_Sound();
            b.transform.eulerAngles = transform.eulerAngles;
            yield return new WaitForSeconds(3f / 7f);
        }
    }
}
