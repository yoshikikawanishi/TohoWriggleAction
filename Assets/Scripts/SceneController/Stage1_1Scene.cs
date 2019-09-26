using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1_1Scene : MonoBehaviour {

    //自機
    private GameObject player;
    //カメラ
    private GameObject main_Camera;

    //SoulEnemyを生成するトリガーのx座標
    [SerializeField] private float soul_Gen_Line = 0;
    private bool start_Soul_Gen = false;


	// Use this for initialization
	void Start () {
        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");
        //カメラの取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        //初回時フェードイン
        FadeInOut f = GetComponent<FadeInOut>();
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            f.Start_Fade_In();
        }
	}
	
	// Update is called once per frame
	void Update () {
        //敵生成
        Enemy_Gen();
        //シーンの遷移
        Scene_Change();
	}


    //敵の生成
    private void Enemy_Gen() {
        //カメラが指定のX座標についた時生成開始
        if(main_Camera.transform.position.x > soul_Gen_Line && !start_Soul_Gen) {
            start_Soul_Gen = true;
            StartCoroutine("Enemy_Gen_Routine");
        }
        //カメラがステージ右端についた時生成の中止
        if(6000f - main_Camera.transform.position.x < 32f) {
            StopCoroutine("Enemy_Gen_Routine");
        }
    }


    private IEnumerator Enemy_Gen_Routine() {
        while (true) {
            //SoulEnemyの生成
            var sl = Resources.Load("Enemy/SoulEnemy") as GameObject;
            for (int i = 0; i < 6; i++) {
                for (int j = 0; j < 4; j++) {                    
                    var soul = Instantiate(sl);
                    float noise = Random.Range(-18f, 18f);
                    if (i % 2 == 0) {
                        soul.transform.position = new Vector3(main_Camera.transform.position.x + 216f, 132f - j * 64f + noise, 0);
                    }
                    else {
                        soul.transform.position = new Vector3(main_Camera.transform.position.x + 216f, -132f + j * 64f + noise, 0);
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(24f/7f - 0.4f);
            }

            //SunFlowerEnemyの生成
            var sf = Resources.Load("Enemy/GreenSunFlowerFairy") as GameObject;
            var sun = Instantiate(sf);
            sun.transform.position = new Vector3(main_Camera.transform.position.x + 200f, 216f, 0);
            yield return new WaitForSeconds(24f/7f);
            GameObject[] suns = new GameObject[2];
            suns[0] = Instantiate(sf);
            suns[1] = Instantiate(sf);
            suns[0].transform.position = new Vector3(main_Camera.transform.position.x + 200f, 264f, 0);
            suns[1].transform.position = new Vector3(main_Camera.transform.position.x + 200f, 160f, 0);

            yield return new WaitForSeconds(48f/7f);

            //YinBallの生成
            var y = Resources.Load("Enemy/YinBall") as GameObject;
            for (int i = 0; i < 25; i++) {
                var yin = Instantiate(y);
                Vector3 pos = new Vector3(Random.Range(-64f, 64f), Random.Range(195f, 200f), 0);
                yin.transform.position = new Vector3(main_Camera.transform.position.x, 0, 0) + pos;
                yield return new WaitForSeconds(3f/7f);
            }

            yield return new WaitForSeconds(72f/7f - (3f*25f)/7f);
        }
    }


    //シーンの遷移
    private void Scene_Change() {
        if(player.transform.position.x > 6232f) {
            SceneManager.LoadScene("Stage1_BossScene");
        }
    }

}
