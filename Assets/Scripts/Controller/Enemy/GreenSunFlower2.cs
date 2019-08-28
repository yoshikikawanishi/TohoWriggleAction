using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSunFlower2 : MonoBehaviour {

    //自機
    private GameObject player;

    //コンポーネント
    private BulletFunctions _bullet;

    //行動開始
    private bool start_Action = false;

    //弾
    [SerializeField] private GameObject burst_Bullet;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        _bullet = gameObject.AddComponent<BulletFunctions>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (!start_Action) {
            if (player.transform.position.x > transform.position.x - 240f && player.transform.position.x < transform.position.x) {
                StartCoroutine("Action");
                start_Action = true;
            }
        }
	}


    //登場とショット
    private IEnumerator Action() {
        //上から降りてくる
        float speed = 4.5f;
        while (speed >= 0) {
            yield return null;
            transform.position += new Vector3(0, -speed * Time.timeScale, 0);
            speed -= 0.05f * Time.timeScale;
        }
        //ショット
        _bullet.Set_Bullet(burst_Bullet);
        while (true) {
            for (int i = 0; i < 2; i++) {
                UsualSoundManager.Shot_Sound();
                _bullet.Turn_Shoot_Bullet(150f, 225f, 10);
                _bullet.Turn_Shoot_Bullet(150f, 315f, 10);
                yield return new WaitForSeconds(0.375f);
            }
            yield return new WaitForSeconds(2.25f);
        }
    }
}
