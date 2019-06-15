using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2_2Scene : MonoBehaviour {

    //自機
    private GameObject player;
    private WriggleController player_Controller;
    //スクリプト
    private Stage2_2Movie _movie;
    //スクロール用
    private GameObject main_Camera;
    [SerializeField] private float right_Side;


    // Use this for initialization
    void Start () {
        //自機の取得
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        //スクリプトの取得
        _movie = GetComponent<Stage2_2Movie>();
        //オブジェクトの取得
        main_Camera = GameObject.FindWithTag("MainCamera");
        //ムービー開始
        _movie.StartCoroutine("Boss_Movie");
    }
	
	// Update is called once per frame
	void Update () {
              
        //スクロール時の自機の動き
        if (player_Controller.Get_Is_Fly()) {    
            player.transform.SetParent(main_Camera.transform);
        }
        else {
            player.transform.SetParent(null);
        }

        //右端についた時
        if(main_Camera.transform.position.x >= right_Side) {
            Exit_Reimu();
        }

        //シーン遷移
        if (player.transform.position.x > 5240f) {
            SceneManager.LoadScene("Stage2_BossScene");
        }
    }


    //敵生成
    public IEnumerator Generate_Enemy() {
        //ファイル読み込み
        TextReader text = new TextReader("Stage2_Boss_Enemy_Gen");
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        //敵生成
        for (int i = 1; i < text.rowLength; i++) {
            yield return new WaitForSeconds(float.Parse(text.textWords[i, 1]));
            GameObject enemy = Instantiate(Resources.Load("Enemy/" + text.textWords[i, 0]) as GameObject);
            Vector3 pos = new Vector3(main_Camera.transform.position.x + float.Parse(text.textWords[i, 2]), float.Parse(text.textWords[i, 3]));
            enemy.transform.position = pos;
            enemy.transform.SetParent(main_Camera.transform);
        }
    }


    //霊夢退場
    private void Exit_Reimu() {
        GameObject reimu = GameObject.Find("Reimu");
        if (reimu != null) {
            reimu.transform.position += new Vector3(1.4f, 0, 0) * Time.timeScale;
        }
    }


}
