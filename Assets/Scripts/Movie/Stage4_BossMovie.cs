using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage4_BossMovie : MonoBehaviour {

    //スクリプト
    private GameManager game_Manager;
    private PauseManager pause_Manager;
    private MessageDisplay _message;
    private BGMManager bgm_Manager;

    //ムービーの進行度
    private int progress_Num = 1;

    //初回かどうか
    private bool is_First_Visit = false;


    //Awake
    private void Awake() {
        //取得
        game_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        pause_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        bgm_Manager = GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>();
        _message = GetComponent<MessageDisplay>();
    }


    //ボス戦前ムービー
    private IEnumerator Before_Boss_Movie() {
        //初期設定
        progress_Num = 0;
        is_First_Visit = game_Manager.Is_First_Visit();
        pause_Manager.Set_Pausable(false);
        StartCoroutine("Player_Timeline");
        StartCoroutine("Marisa_Timeline");

        yield return new WaitForSeconds(1.2f);
        progress_Num = 1;

        //登場会話
        if (is_First_Visit) {
            _message.Start_Display("MarisaText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }
        progress_Num = 2;

        yield return new WaitForSeconds(1.5f);

        //戦闘前会話
        if (is_First_Visit) {
            _message.Start_Display("MarisaText", 2, 4);
            yield return new WaitUntil(_message.End_Message);
        }
        progress_Num = 3;

        //終了設定、戦闘開始
        pause_Manager.Set_Pausable(true);
        bgm_Manager.Change_BGM_Index(7);
    }


    //自機
    private IEnumerator Player_Timeline() {
        //初期設定
        GameObject player = GameObject.FindWithTag("PlayerTag");
        WriggleController player_Controller = player.GetComponent<WriggleController>();
        player_Controller.Set_Playable(false);
        player_Controller.Change_Parameter("DashBool");
        //横に移動
        while(progress_Num == 0) {
            player.transform.position += new Vector3(1.5f, 0);
            yield return null;
        }
        player_Controller.Change_Parameter("IdleBool");

        //戦闘前会話
        while(progress_Num <= 2) { yield return null; }

        //終了設定
        player_Controller.Set_Playable(true);
    }


    //魔理沙
    private IEnumerator Marisa_Timeline() {
        //初期設定
        GameObject marisa = GameObject.Find("Marisa");
        MoveBetweenTwoPoints marisa_Move = marisa.GetComponent<MoveBetweenTwoPoints>();
        MarisaController marisa_Controller = marisa.GetComponent<MarisaController>();
        
        //登場前会話
        while (progress_Num <= 1) { yield return null; }

        //登場
        marisa_Controller.Change_Parameter("DashBool1", 1);
        marisa_Move.Start_Move(new Vector3(160f, 16f), -64f, 0.01f);
        yield return new WaitUntil(marisa_Move.End_Move);
        marisa_Controller.Change_Parameter("IdleBool", 1);

        //戦闘前会話
        while (progress_Num <= 2) { yield return null; }

        //終了設定、戦闘開始
        marisa_Controller.start_Battle = true;
    }


    //クリア時
    public void Start_Clear_Movie() {
        StartCoroutine("Clear_Movie");
    }
    private IEnumerator Clear_Movie() {
        yield return new WaitForSeconds(2.5f);
        //セリフ
        _message.Start_Display("MarisaText", 5, 6);
        yield return new WaitUntil(_message.End_Message);
        //魔理沙退場
        GameObject marisa = GameObject.Find("Marisa");
        MoveBetweenTwoPoints marisa_Move = marisa.GetComponent<MoveBetweenTwoPoints>();
        marisa_Move.Start_Move(new Vector3(260f, 100f), 0, 0.02f);
        yield return new WaitForSeconds(1.5f);
        //セリフ
        _message.Start_Display("MarisaText", 7, 7);
        yield return new WaitUntil(_message.End_Message);
        //フェードアウト
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Stage5_1Scene");
    }
    
}
