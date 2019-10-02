using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_Movie : MonoBehaviour {
    
    //スクリプト
    private MessageDisplay _message;
    private PauseManager _pause;

    //オブジェクト
    private GameObject player;
    private WriggleController player_Controller;
    private GameObject reimu;
    private GameObject kagerou;

    [SerializeField] private GameObject fade_In_Obj;
    [SerializeField] private GameObject boss_Cavas;


    // Use this for initialization
    void Awake () {
        //取得
        _message = GetComponent<MessageDisplay>();
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        reimu = GameObject.Find("Reimu");
        kagerou = GameObject.Find("Kagerou");
    }
	
	
    //エンディングムービー
    public IEnumerator Start_Ending_Movie() {
        //初期設定
        player_Controller.Change_Parameter("SquatBool");
        player_Controller.Set_Playable(false);
        _pause.Set_Pausable(false);
        boss_Cavas.SetActive(false);
        //フェードイン
        GetComponent<FadeInOut>().Start_Fade_In();
        yield return new WaitForSeconds(2.0f);

        //セリフ1
        _message.Start_Display("EndingText", 1, 3);
        yield return new WaitUntil(_message.End_Message);

        //霊夢登場
        kagerou.transform.localScale = new Vector3(-1, 1, 1);
        reimu.GetComponent<MoveBetweenTwoPoints>().Start_Move(new Vector3(140f, 48f), 0, 0.015f);
        yield return new WaitForSeconds(1.0f);

        //セリフ2
        _message.Start_Display("EndingText", 4, 6);
        yield return new WaitUntil(_message.End_Message);
        yield return new WaitForSeconds(1.0f);
        //BGM開始
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Change_BGM_Index(12);
        _message.Start_Display_Auto("EndingText", 7, 7, 1.5f, 0.07f);
        yield return new WaitUntil(_message.End_Message);
        _message.Start_Display_Auto("EndingText", 8, 8, 1.0f, 0.005f);
        yield return new WaitUntil(_message.End_Message);
        _message.Start_Display("EndingText", 9, 9);

        //霊夢戦開始、スタッフロール開始
        player_Controller.Set_Playable(true);
        reimu.GetComponent<EndingReimuController>().start_Battle_Trigger = true;
        GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().life = 9;        
        boss_Cavas.SetActive(true);
        GetComponent<StaffRollManager>().Start_Staff_Roll();
    }


    //クリア後ムービー
    public IEnumerator Play_Clear_Movie() {
        //自機無敵化
        player.layer = LayerMask.NameToLayer("InvincibleLayer");
        //霊夢止める
        reimu.GetComponent<EndingReimuAttaack>().Stop_Reimu_Attack();
        //フェードアウト、終了設定
        fade_In_Obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<FadeInOut>().speed = 0.01f;
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(2.0f);
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Start_Fade_Out(0.005f, 2.1f);
        yield return new WaitForSeconds(2.0f);
        _pause.Set_Pausable(true);

        SceneManager.LoadScene("AfterEndingScene");
    }



}
