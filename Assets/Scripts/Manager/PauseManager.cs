using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;
    
    //状態
    private enum STATE {
        pause,
        confirm,
        normal,
    }
    private STATE state = STATE.normal;
    
    //一時停止可能かどうか
    public bool can_Pause = true;
    //ポーズ画面
    private GameObject pause_Canvas;
    //確認画面
    private GameObject confirm_Canvas;


    //Start
    private void Start() {
        _gameManager = GetComponent<GameManager>();
    }


    // Update is called once per frame
    void Update () {
        if (_gameManager.Is_Game_Scene()) {
            //一時停止
            if (Input.GetButtonDown("Pause") && can_Pause && state == STATE.normal) {
                Pause_Game();
            }
            //一時停止解除
            else if (Input.GetButtonDown("Pause") && state == STATE.pause) {
                Release_Pause_Game();
            }
            else if (Input.GetButtonDown("Shot/Cancel") && state == STATE.pause) {
                Release_Pause_Game();
            }
        }
    }


    //一時停止時の処理
    private void Pause_Game() {
        state = STATE.pause;
        //時間止める
        Time.timeScale = 0;
        //自機の操作を止める
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(false);
        }
        //ポーズ画面を出す
        Display_Pause_Canvas();
    }


    //一時停止解除時の処理
    public void Release_Pause_Game() {
        state = STATE.normal;
        //時間動かす
        Time.timeScale = 1;
        //自機を動けるようにする
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(true);
        }
        //ポーズ画面を消す
        Delete_Pause_Canvas();
    }


    //ポーズ画面の生成
    private void Display_Pause_Canvas() {
        if (pause_Canvas == null) {
            pause_Canvas = Instantiate(Resources.Load("PauseCanvas") as GameObject);
        }
        pause_Canvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        pause_Canvas.transform.GetChild(0).GetComponent<Button>().Select();
        UsualSoundManager.Pause_In_Sound();
    }


    //ポーズ画面の消去
    private void Delete_Pause_Canvas() {
        if (pause_Canvas == null) {
            pause_Canvas = Instantiate(Resources.Load("PauseCanvas") as GameObject);
        }
        pause_Canvas.SetActive(false);
        UsualSoundManager.Pause_Out_Sound();
    }


    //タイトルに戻る押下時の確認画面を生成
    public void Confirm_Back_Title() {
        Delete_Pause_Canvas();
        confirm_Canvas = Instantiate(Resources.Load("ConfirmCanvas") as GameObject);
        EventSystem.current.SetSelectedGameObject(null);
        confirm_Canvas.transform.GetChild(2).GetComponent<Button>().Select();
        state = STATE.confirm;
    }


    //確認画面を消す
    public void Delete_Confirm_Canvas() {
        Destroy(confirm_Canvas);
        state = STATE.pause;
        Display_Pause_Canvas();
    }


    //1時停止中かどうかを返すメソッド
    public bool Is_Pause() {
        if (state == STATE.pause || state == STATE.confirm) {
            return true;
        }
        return false;
    }


    //can_PauseのSetter
    public void Set_Pausable(bool can_Pause) {
        this.can_Pause = can_Pause;
    }

}
