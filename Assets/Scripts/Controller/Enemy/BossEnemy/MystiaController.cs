using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaController : MonoBehaviour {

    //スクリプト
    private BossEnemyController boss_Controller;
    private MoveBetweenTwoPoints _move;
    //自機
    private GameObject player;

    //戦闘開始
    public bool start_Battle = false;

    //フェーズ毎
    private bool start_Phase1_Routine = true;
    private bool start_Phase2_Routine = true;
    private bool start_Phase3_Routine = true;


	// Use this for initialization
	void Start () {
        //スクリプトの取得
        boss_Controller = GetComponent<BossEnemyController>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        //自機
        player = GameObject.FindWithTag("PlayerTag");
    }
	

	// Update is called once per frame
	void Update () {
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: Phase1(); break;
                case 2: Phase2(); break;
                case 3: Phase3(); break;
            }
        }
	}


    //フェーズ1
    private void Phase1() {
        if (start_Phase1_Routine) {
            start_Phase1_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }

    //フェーズ1コルーチン
    private IEnumerator Phase1_Routine() {
        ObjectPool blue_Scales_Pool = gameObject.AddComponent<ObjectPool>();
        while (true) {
            _move.Start_Move(new Vector3(200f, -128f), 0, 0.016f);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(1.0f);
            //斜め上に上がりながら鳥出す
            _move.Start_Move(new Vector3(-260f, 160f), 0, 0.005f);
            while(transform.position.y < -100f) { yield return null; }
            Bird_Gen(false);
            while(transform.position.y < 100f) { yield return null; }
            Bird_Gen(true);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(1.0f);
            //後ろのほうを横切る
            transform.position = new Vector3(-250f, 90f);
            while(transform.position.x < 260f) {
                transform.position += new Vector3(3f, 0) * Time.timeScale;
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
            //突進
            transform.position = new Vector3(260f, player.transform.position.y + 64f);
            _move.Start_Move(new Vector3(-260f, transform.position.y), -80f, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            yield return new WaitForSeconds(1.0f);
            transform.position = new Vector3(-260f, player.transform.position.y + 64f);
            _move.Start_Move(new Vector3(260f, transform.position.y), -80f, 0.01f);
            yield return new WaitUntil(_move.End_Move);
            //移動
            _move.Start_Move(new Vector3(200f, -32f), 0, 0.016f);
            yield return new WaitUntil(_move.End_Move);
        }
    }
    //鳥の生成
    private void Bird_Gen(bool is_Right) {
        GameObject bird = Instantiate(Resources.Load("Enemy/MystiaBird") as GameObject);
        bird.transform.position = transform.position;
        bird.GetComponent<MystiaBird>().is_Right_Direction = is_Right;
    }

    //フェーズ2
    private void Phase2() {
        if (start_Phase2_Routine) {
            start_Phase2_Routine = false;
            StartCoroutine("Phase2_Routine");
            StopCoroutine("Phase1_Routine");
        }
    }

    //フェーズ2コルーチン
    private IEnumerator Phase2_Routine() {
        yield return null;
    }


    //フェーズ3
    private void Phase3() {
        if (start_Phase3_Routine) {
            start_Phase3_Routine = false;
            StartCoroutine("Phase3_Routine");
            StopCoroutine("Phase2_Routine");
        }
    }

    //フェーズ3コルーチン
    private IEnumerator Phase3_Routine() {
        yield return null;
    }

}
