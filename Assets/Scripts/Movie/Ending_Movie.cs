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
        _message.Start_Display("EndingText", 7, 9);
        yield return new WaitUntil(_message.End_Message);

        reimu.GetComponent<MoveBetweenTwoPoints>().Start_Move(player.transform.position, -64f, 0.01f);
        reimu.GetComponent<Animator>().SetBool("DashBool", true);

        //フェードアウト、終了設定
        fade_In_Obj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(2.0f);
        _pause.Set_Pausable(true);
     
        SceneManager.LoadScene("StaffRollScene");
    }


    
}
