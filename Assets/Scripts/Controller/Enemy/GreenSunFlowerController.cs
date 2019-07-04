using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSunFlowerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("SunFlowerFairy_Routine");
    }
	

    //登場後の流れ
    private IEnumerator SunFlowerFairy_Routine() {
        UsualSoundManager.Small_Shot_Sound();
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0){
            yield return null;
            transform.position += new Vector3(0, -speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        yield return new WaitForSeconds(1.5f);
        //弾の発射
        var bullet = Resources.Load("Bullet/YellowBullet") as GameObject;
        BulletFunctions b = gameObject.AddComponent<BulletFunctions>();
        b.Set_Bullet(bullet);
        //V字型に発射
        b.Diffusion_Bullet(10, 100f, 0, 5.0f);
        b.Diffusion_Bullet(10, 90f, 5f, 5.0f);
        b.Diffusion_Bullet(10, 90f, -5f, 5.0f);
        //効果音
        UsualSoundManager.Shot_Sound();
        yield return new WaitForSeconds(1.5f);
        //上にはける
        while (transform.position.y < 210f) {
            transform.position += new Vector3(0, speed * Time.timeScale, 0);
            speed += 0.05f * Time.timeScale;
            yield return null;
        }
        //消す
        Destroy(gameObject);
    }
   
}
