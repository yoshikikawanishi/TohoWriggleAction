using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_BossMovie : MonoBehaviour {

    //オブジェクト
    private GameObject player;
    private GameObject kagerou;
    private GameObject larva;

    //コンポーネント
    private MessageDisplay _message;

    //スクリプト
    private PauseManager pause_Manager;

    //定数
    private float LARVA_HEIGHT = 60f;


    //Awake
    private void Awake() {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        kagerou = GameObject.Find("Kagerou");
        larva = GameObject.Find("Larva");
        _message = GetComponent<MessageDisplay>();
        pause_Manager = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
    }


    // Use this for initialization
    void Start () {
	}
	

    //ムービー初期設定
    private void Start_Setting() {
        pause_Manager.Set_Pausable(false);
        player.GetComponent<WriggleController>().Set_Playable(false);
    }


    //戦闘開始
    private void Start_Battle() {
        pause_Manager.Set_Pausable(true);
        player.GetComponent<WriggleController>().Set_Playable(true);
        kagerou.GetComponent<KagerouController>().Start_Battle();
        GameObject.FindWithTag("BGMTag").GetComponent<BGMManager>().Change_BGM_Index(8);
    }

	
    //初回時ボス前ムービー
    public IEnumerator Before_Movie_First() {
        //初期設定
        Start_Setting();

        //ラルバ登場、自機登場
        Appear_Larva();
        yield return new WaitForSeconds(1.5f);
        player.GetComponent<WriggleController>().Change_Parameter("IdleBool");

        //ラルバ警告会話
        _message.Start_Display("KagerouText", 1, 1);
        yield return new WaitUntil(_message.End_Message);

        //影狼咆哮
        Roar();
        yield return new WaitForSeconds(1.0f);

        //ラルバ逃げる会話
        _message.Start_Display("KagerouText", 2, 2);
        yield return new WaitUntil(_message.End_Message);

        //ラルバ逃げる、影狼登場
        StartCoroutine(Appear_Kagerou());
        yield return new WaitForSeconds(2.0f);

        //影狼会話
        _message.Start_Display("KagerouText", 3, 3);
        yield return new WaitUntil(_message.End_Message);

        Start_Battle();
    }


    //ラルバ登場
    private void Appear_Larva() {
        //取得
        MoveBetweenTwoPoints larva_Move = larva.GetComponent<MoveBetweenTwoPoints>();
        MoveBetweenTwoPoints player_Move = player.AddComponent<MoveBetweenTwoPoints>();
        //移動
        larva.GetComponent<LarvaController>().Change_Parameter("DashBool");
        larva_Move.Start_Move(new Vector3(160f, LARVA_HEIGHT), 0, 0.02f);
        player_Move.Start_Move(new Vector3(-150f, -114f), 0, 0.015f);
        player.GetComponent<WriggleController>().Change_Parameter("DashBool");
    }


    //影狼咆哮
    private void Roar() {

    }


    //ラルバ逃げる、影狼登場
    private IEnumerator Appear_Kagerou() {
        //取得
        MoveBetweenTwoPoints larva_Move = larva.GetComponent<MoveBetweenTwoPoints>();
        MoveBetweenTwoPoints kagerou_Move = kagerou.GetComponent<MoveBetweenTwoPoints>();
        //移動
        larva_Move.Start_Move(new Vector3(-260f, LARVA_HEIGHT), 0, 0.02f);
        yield return new WaitUntil(larva_Move.End_Move);
        Destroy(larva);
        kagerou_Move.Start_Move(new Vector3(100, -114f), 0, 0.04f);
    }
    

    //2回目以降ボス前ムービー
    private IEnumerator Before_Movie_Second() {
        //初期設定
        Start_Setting();

        //ラルバ消す
        Destroy(larva);
        //自機移動
        player.transform.position = new Vector3(-150f, -114f);
        //影狼登場
        MoveBetweenTwoPoints kagerou_Move = kagerou.GetComponent<MoveBetweenTwoPoints>();
        kagerou_Move.Start_Move(new Vector3(100f, -114f), 0, 0.04f);
        yield return new WaitUntil(kagerou_Move.End_Move);
        
        //影狼会話
        _message.Start_Display("KagerouText", 3, 3);
        yield return new WaitUntil(_message.End_Message);
        
        //戦闘開始
        Start_Battle();
    }



}
