using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2_1Scene : MonoBehaviour {
    
    //カメラ
    private GameObject main_Camera;
    //自機
    private GameObject player;
    private WriggleController player_Controller;
    
    //敵生成開始
    private bool start_Enemy_Gen = false;


    // Use this for initialization
    void Start () {
        //カメラ
        main_Camera = GameObject.FindWithTag("MainCamera");
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();

        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            GetComponent<FadeInOut>().Start_Fade_In();
        }
    }

    // Update is called once per frame
    void Update () {
        //敵生成開始
        if (!start_Enemy_Gen) {
            if (6700f < main_Camera.transform.position.x && main_Camera.transform.position.x < 6732f) {
                start_Enemy_Gen = true;
                StartCoroutine("Enemy_Gen");
            }
        }
        //シーンの遷移
        if (player.transform.position.x > 9240f) {
            SceneManager.LoadScene("Stage2_2Scene");
        }
        //スクロール時、自機の動き
        if(player.transform.position.x > 6000) {
            if (player_Controller.Get_Is_Fly() && player.transform.parent != main_Camera.transform) {
                player.transform.SetParent(main_Camera.transform);
            }
            else if(!player_Controller.Get_Is_Fly() && player.transform.parent != null) {
                player.transform.SetParent(null);
            }
        }
        
	}


    //敵生成
    private IEnumerator Enemy_Gen() {
        //ファイル読み込み
        TextFileReader text = new TextFileReader("Stage2_1_Enemy_Gen");
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
