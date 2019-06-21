using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

    //スクリプト
    private GameManager _gameManager;
    //一時停止中かどうか
    private bool is_Pause = false;
    //一時停止可能かどうか
    public bool can_Pause = true;
    //ポーズ画面
    private GameObject pause_Canvas;


    //Start
    private void Start() {
        _gameManager = GetComponent<GameManager>();
        //ポーズ画面の取得
        pause_Canvas = Resources.Load("PauseCanvas") as GameObject;
    }


    // Update is called once per frame
    void Update () {
        //一時停止の処理
        if (Input.GetButtonDown("Cancel") && can_Pause && !is_Pause && _gameManager.Is_Game_Scene()) {
            Pause_Game();
        }
        //一時停止解除
        else if (Input.GetButtonDown("Cancel") && is_Pause && _gameManager.Is_Game_Scene()) {
            Release_Pause_Game();
        }
        
    }

    //一時停止時の処理
    private void Pause_Game() {
        is_Pause = true;
        //時間止める
        Time.timeScale = 0;
        //自機の操作を止める
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(false);
        }
        //ポーズ画面を出す
        pause_Canvas = Instantiate(Resources.Load("PauseCanvas")) as GameObject;
        pause_Canvas.transform.GetChild(0).GetComponent<Button>().Select();
    }

    //一時停止解除時の処理
    public void Release_Pause_Game() {
        is_Pause = false;
        //時間動かす
        Time.timeScale = 1;
        //自機を動けるようにする
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(true);
        }
        //ポーズ画面を消す
        pause_Canvas.GetComponents<AudioSource>()[1].Play();
        pause_Canvas.GetComponent<Canvas>().enabled = false;
        Destroy(pause_Canvas, 0.5f);
    }


    //1時停止中かどうかを返すメソッド
    public bool Is_Pause() {
        if (is_Pause) {
            return true;
        }
        return false;
    }


    //can_PauseのSetter
    public void Set_Pausable(bool can_Pause) {
        this.can_Pause = can_Pause;
    }

}
