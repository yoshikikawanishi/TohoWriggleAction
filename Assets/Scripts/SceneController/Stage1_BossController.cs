using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1_BossController : MonoBehaviour {

    //自機
    private GameObject player;
    //ラルバ
    private GameObject larva;

    //スクリプト
    private MessageDisplay _message;
    private PlayerController _playerController;
    private BossFunction _bossFunction;
    private PauseManager _pause;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        larva = GameObject.Find("Larva");
        _message = GetComponent<MessageDisplay>();
        _playerController = player.GetComponent<PlayerController>();
        _bossFunction = larva.GetComponent<BossFunction>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();

        //ボス前ムービー
        StartCoroutine("Before_Movie");
	}
	
	// Update is called once per frame
	void Update () {
        //クリア後
        if (_bossFunction.Clear_Trigger()) {
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
        _message.Start_Display("LarvaAppearText");
        yield return new WaitUntil(_message.End_Message);
        larva.GetComponent<LarvaController>().Change_Parameter("DashBool");
        MoveBetweenTwoPoints larva_Move = larva.GetComponent<MoveBetweenTwoPoints>();
        larva_Move.Set_Status(-16f, 0.01f);
        larva_Move.StartCoroutine("Move_Two_Points",new Vector3(110f, 16f));
        yield return new WaitUntil(larva_Move.End_Move);
        larva.GetComponent<LarvaController>().Change_Parameter("IdleBool");
        //メッセージ表示
        _message.Start_Display("LarvaText");
        yield return new WaitUntil(_message.End_Message);
        //戦闘開始
        _pause.Set_Pausable(true);
        _playerController.Set_Playable(true);
        _bossFunction.Set_Now_Phase(1);

    }


    //クリア後
    private IEnumerator Clear_Movie() {
        //ラルバ
        larva.GetComponent<LarvaController>().Change_Parameter("IdleBool");
        larva.GetComponent<LarvaController>().StopAllCoroutines();
        yield return new WaitForSeconds(2.0f);
        //一時停止不可
        _pause.Set_Pausable(false);
        //スコア加算
        PlayerManager _playerManager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        for(int i = 0; i < 50; i++) {
            _playerManager.Get_Score();
            yield return new WaitForSeconds(0.01f);
        }
        //メッセージ表示
        _message.Start_Display("LarvaClearText");
        yield return new WaitUntil(_message.End_Message);
        yield return new WaitForSeconds(1.5f);
        _pause.Set_Pausable(true);
        //シーン遷移
        SceneManager.LoadScene("Stage2_1Scene");
    }

}
