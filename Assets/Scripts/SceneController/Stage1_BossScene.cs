using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1_BossScene : MonoBehaviour {

    //自機
    private GameObject player;
    //ラルバ
    private GameObject larva;

    //スクリプト
    private GameManager _gameManager;
    private MessageDisplay _message;
    private PlayerController _playerController;
    private BossEnemyController __BossEnemyController;
    private PauseManager _pause;

    //初めてかどうか
    private bool is_First_Visit = true;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        larva = GameObject.Find("Larva");
        _gameManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        _message = GetComponent<MessageDisplay>();
        _playerController = player.GetComponent<PlayerController>();
        __BossEnemyController = larva.GetComponent<BossEnemyController>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();

        //初めてのボス戦かどうか
        if (_gameManager.Is_First_Visit("Stage1_BossScene")) {
            is_First_Visit = true;
        }
        else {
            is_First_Visit = false;
        }

        //ボス前ムービー
        StartCoroutine("Before_Movie");
	}
	
	// Update is called once per frame
	void Update () {
        //クリア後
        if (__BossEnemyController.Clear_Trigger()) {
            StartCoroutine("Clear_Movie");
        }
	}


    //ボス前ムービー
    private IEnumerator Before_Movie() {
        //一時停止不可
        _pause.Set_Pausable(false);
        //自機の移動
        _playerController.Set_Playable(false);
        _playerController.Change_Parameter("DashBool");
        for(float t = 0; t < 1.5f; t += Time.deltaTime) {
            player.transform.position += new Vector3(2f, 0, 0);
            yield return null;
        }
        _playerController.Change_Parameter("IdleBool");
        //ラルバの登場
        //メッセージ
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }
        //移動
        larva.GetComponent<LarvaController>().Change_Parameter("DashBool");
        MoveBetweenTwoPoints larva_Move = larva.GetComponent<MoveBetweenTwoPoints>();
        larva_Move.Start_Move(new Vector3(110f, -32f), -32f, 0.01f);
        yield return new WaitUntil(larva_Move.End_Move);
        larva.GetComponent<LarvaController>().Change_Parameter("IdleBool");
        //メッセージ表示
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 2, 3);
            yield return new WaitUntil(_message.End_Message);
        }
        //戦闘開始
        _pause.Set_Pausable(true);
        _playerController.Set_Playable(true);
        larva.GetComponent<LarvaController>().start_Battle = true;

    }


    //クリア後
    private IEnumerator Clear_Movie() {
        //ラルバ
        larva.GetComponent<LarvaController>().Change_Parameter("IdleBool");
        larva.GetComponent<LarvaController>().StopAllCoroutines();
        Destroy(GameObject.Find("LarvaBackDesigns"));
        yield return new WaitForSeconds(4.0f);
        //一時停止不可
        _pause.Set_Pausable(false);
        //メッセージ表示
        _message.Start_Display("LarvaText", 4, 4);
        yield return new WaitUntil(_message.End_Message);
        yield return new WaitForSeconds(1.5f);
        _pause.Set_Pausable(true);
        //シーン遷移
        SceneManager.LoadScene("Stage2_1Scene");
    }

}
