using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFairyController : MonoBehaviour {

    //種類分け
    [SerializeField] int kind_Num = 0;


	// Use this for initialization
	void Start () {
		switch (kind_Num) {
            case 1: SunFlowerEnemy_Start(); break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (kind_Num) {
            case 1: break;
        }
	}


    //SunFlowerEnemy
    private void SunFlowerEnemy_Start() {
        StartCoroutine("SunFlowerEnemy_Routine");
    }
    private IEnumerator SunFlowerEnemy_Routine() {
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0){
            yield return null;
            transform.position += new Vector3(0, -speed, 0);
            speed -= 0.05f;
        }
        yield return new WaitForSeconds(1.5f);
        //弾の発射
        var bullet = Resources.Load("Bullet/YellowBullet") as GameObject;
        BulletFunctions b = gameObject.AddComponent<BulletFunctions>();
        b.Set_Bullet(bullet);
        //V字型に発射
        b.Diffusion_Bullet(10, 100f, 0, 5.0f);
        b.Diffusion_Bullet(10, 90f, 20f, 5.0f);
        b.Diffusion_Bullet(10, 90f, -20f, 5.0f);
        yield return new WaitForSeconds(1.5f);
        //上にはける
        while (transform.position.y < 210f) {
            transform.position += new Vector3(0, speed, 0);
            speed += 0.05f;
            yield return null;
        }
        //消す
        Destroy(gameObject);
    }
    

}
