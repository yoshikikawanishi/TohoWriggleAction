using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharacter : MonoBehaviour {

    
    //スクリプト
    protected MessageDisplay _message;

    //吹き出し
    private GameObject mark_Up_Baloon;
    [SerializeField] private Vector3 baloon_Pos;

    //読み込むテキストファイル
    public string fileName;
    public int start_ID;
    public int end_ID;

    //会話中か
    protected bool is_Talking = false;
    //会話終了検知用
    protected bool end_Talk = false;


    //Start
    protected void Start() {
        //スクリプト
        _message = gameObject.AddComponent<MessageDisplay>();
        //吹き出し
        mark_Up_Baloon = Instantiate(Resources.Load("Object/MarkUpBaloon") as GameObject);
        mark_Up_Baloon.transform.position = transform.position + baloon_Pos;
        mark_Up_Baloon.transform.SetParent(gameObject.transform);
        mark_Up_Baloon.SetActive(false);
    }


    //OnTriggerStay
    protected void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            if (!is_Talking && Input.GetAxisRaw("Vertical") > 0) {
                StartCoroutine("Talk");
            }
        }
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            mark_Up_Baloon.SetActive(true);            
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            mark_Up_Baloon.SetActive(false);
        }
    }


    //会話
    virtual protected IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        //自機を止める
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        if (player_Controller.Get_Playable()) {
            player_Controller.Set_Playable(false);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player_Controller.Change_Parameter("IdleBool");
            //ポーズ禁止
            PauseManager _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
            _pause.Set_Pausable(false);
            //停止
            Time.timeScale = 0;
            //メッセージ
            _message.Start_Display(fileName, start_ID, end_ID);
            yield return new WaitUntil(_message.End_Message);
            //終了
            Time.timeScale = 1;
            yield return new WaitForSeconds(0.1f);
            player_Controller.Set_Playable(true);
            _pause.Set_Pausable(true);
        }
        end_Talk = true;
        is_Talking = false;
    }


    //会話終了検知用
    public bool End_Talk() {
        if (end_Talk) {
            end_Talk = false;
            return true;
        }
        return false;
    }


    //ステータス代入
    public void Set_Status(string file_Name, int start_ID, int end_ID, Vector2 baloon_Pos) {
        this.fileName = file_Name;
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        this.baloon_Pos = baloon_Pos;
    }
}
