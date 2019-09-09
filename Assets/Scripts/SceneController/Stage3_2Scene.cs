using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3_2Scene : MonoBehaviour {

    //自機、カメラ
    private GameObject player;
    private WriggleController player_Controller;
    private GameObject main_Camera;

    //敵生成開始
    private bool start_Enemy_Gen = false;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        main_Camera = GameObject.FindWithTag("MainCamera");
	}

    // Update is called once per frame
    void Update() {
        //スクロール時の時期の動き
        if (main_Camera.transform.position.x > 5800f && player != null) {
            if (player_Controller.Get_Is_Fly() && player.transform.parent != main_Camera.transform) {
                player.transform.SetParent(main_Camera.transform);
            }
            else if (!player_Controller.Get_Is_Fly() && player.transform.parent != null) {
                player.transform.SetParent(null);
            }
        }
        //適生成
        if (!start_Enemy_Gen) {
            if (main_Camera.transform.position.x > 5800f && main_Camera.transform.position.x < 6000f) {
                start_Enemy_Gen = true;
                StartCoroutine("Enemy_Gen");
            }
        }
        //シーン遷移
        if (player.transform.position.x > 10500f) {
            SceneManager.LoadScene("Stage3_BossScene");
        }
    }


    //敵生成
    private IEnumerator Enemy_Gen() {
        //ファイル読み込み
        TextFileReader text = new TextFileReader("Stage3_2_Enemy_Gen");
        //敵生成
        for (int i = 1; i < text.rowLength; i++) {
            yield return new WaitForSeconds(float.Parse(text.textWords[i, 1]));
            GameObject enemy = Instantiate(Resources.Load("Enemy/" + text.textWords[i, 0]) as GameObject);
            enemy.transform.SetParent(main_Camera.transform);
            Vector3 pos = new Vector3(main_Camera.transform.position.x + float.Parse(text.textWords[i, 2]), float.Parse(text.textWords[i, 3]));
            enemy.transform.position = pos;
        }
    }
}
