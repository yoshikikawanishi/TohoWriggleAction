using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage4_1Scene : MonoBehaviour {

    //時間
    public float[] bgm_Match_Span = { 4.0f, 4.0f, 4.0f, 4.0f };

    //コンポーネント
    private EnemyGenerator _enemy_Gen;
    //カメラ、自機
    private GameObject player;
    private GameObject main_Camera;

    //イベント戦の壁ブロック
    private GameObject[] wall_Blocks = new GameObject[8];
    //イベント戦の敵置き場
    [SerializeField] private GameObject event_Enemy_Parent;

    //イベント開始
    private bool start_Event_Battle1 = false;
    private bool start_Event_Battle2 = false;
    private bool start_Event_Battle3 = false;


	// Use this for initialization
	void Start () {
        //取得
        _enemy_Gen = GetComponent<EnemyGenerator>();
        main_Camera = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("PlayerTag");
	}
	

	// Update is called once per frame
	void Update () {
		//イベント戦開始
        if(main_Camera.transform.position.x >= 8608f && !start_Event_Battle1) {
            start_Event_Battle1 = true;
            StartCoroutine("Event_Battle1_Routine");
        }
        if (main_Camera.transform.position.x >= 9344f && !start_Event_Battle2) {
            start_Event_Battle2 = true;
            StartCoroutine("Event_Battle2_Routine");
        }
        if(main_Camera.transform.position.x >= 10080f && !start_Event_Battle3) {
            start_Event_Battle3 = true;
            StartCoroutine("Event_Battle3_Routine");
        }
        //シーン遷移
        if(player.transform.position.x > 10740f) {
            SceneManager.LoadScene("Stage4_2Scene");
        }
    }


    //イベント戦１
    private IEnumerator Event_Battle1_Routine() {
        //壁の生成
        StartCoroutine(Generate_Blocks_Wall(8408f, 8808f));
        //カメラの固定
        main_Camera.GetComponent<CameraController>().enabled = false;
        main_Camera.transform.position = new Vector3(8608f, 0, -10);
        yield return new WaitForSeconds(0.5f);
        //敵の生成ウェーブ1
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 1, 2, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ2
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 3, 6, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ3
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 7, 14, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //イベント終了、壁の破壊
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Delete_Blocks_Wall());
        //カメラを自機の位置に滑らかに動かす
        StartCoroutine("Approach_Camera");
    }


    //イベント戦２
    private IEnumerator Event_Battle2_Routine() {
        //壁の生成
        StartCoroutine(Generate_Blocks_Wall(9144f, 9544f));
        //カメラの固定
        main_Camera.GetComponent<CameraController>().enabled = false;
        main_Camera.transform.position = new Vector3(9344f, 0, -10);
        yield return new WaitForSeconds(0.5f);
        //敵の生成ウェーブ1
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 16, 19, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ2
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 20, 23, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ3
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 24, 29, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //イベント終了、壁の破壊
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Delete_Blocks_Wall());
        //カメラを自機の位置に滑らかに動かす
        StartCoroutine("Approach_Camera");
        yield return null;
    }


    //イベント戦3
    private IEnumerator Event_Battle3_Routine() {
        //壁の生成
        StartCoroutine(Generate_Blocks_Wall(9880f, 10280f));
        //カメラの固定
        main_Camera.GetComponent<CameraController>().enabled = false;
        main_Camera.transform.position = new Vector3(10080f, 0, -10);
        yield return new WaitForSeconds(0.5f);
        //敵の生成ウェーブ1
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 31, 34, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ2
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 35, 40, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //ウェーブ3
        _enemy_Gen.Start_Enemy_Gen("Stage4_Event_Battle", 42, 53, event_Enemy_Parent);
        yield return new WaitUntil(_enemy_Gen.End_Generate);
        while (event_Enemy_Parent.transform.childCount != 0) {
            yield return null;
        }
        //イベント終了、壁の破壊
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Delete_Blocks_Wall());
        //カメラを自機の位置に滑らかに動かす
        StartCoroutine("Approach_Camera");
        yield return null;
    }


    //壁の生成
    private IEnumerator Generate_Blocks_Wall(float left_Side, float right_Side) {
        for(int i = 0; i < 4; i++) {
            wall_Blocks[i] = Instantiate(Resources.Load("Object/HardBlock") as GameObject);
            wall_Blocks[i].transform.position = new Vector3(left_Side, -120f + i * 16);
            wall_Blocks[i+4] = Instantiate(Resources.Load("Object/HardBlock") as GameObject);
            wall_Blocks[i+4].transform.position = new Vector3(right_Side, -120f + i * 16);
            yield return new WaitForSeconds(0.1f);
        }
    }


    //壁の破壊
    private IEnumerator Delete_Blocks_Wall() {
        for(int i = 0; i < 4; i++) {
            wall_Blocks[i].GetComponent<ObjectDestroyer>().Destroy_Object();
            wall_Blocks[i+4].GetComponent<ObjectDestroyer>().Destroy_Object();
            yield return new WaitForSeconds(0.1f);
        }
    }


    //カメラを自機に滑らかに近づける
    private IEnumerator Approach_Camera() {
        float difference = 0;
        do {
            difference = main_Camera.transform.position.x - (player.transform.position.x + 64f);
            main_Camera.transform.position += new Vector3(-difference / Mathf.Abs(difference) * 5f, 0);
            yield return null;
        } while (Mathf.Abs(difference) > 3f);
        main_Camera.GetComponent<CameraController>().enabled = true;
    }

}
