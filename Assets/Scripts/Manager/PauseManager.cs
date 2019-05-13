using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    //一時停止中かどうか
    private bool is_Pause = false;
    //一時停止可能かどうか
    public bool can_Pause = true;
    //一時停止になったことを検知する用
    private bool pause_Trigger = false;
    //一時停止解除を検知する用
    private bool release_Pause_Trigger = false;

	
	// Update is called once per frame
	void Update () {
        //一時停止の処理
        if (Input.GetKeyDown(KeyCode.Escape) && can_Pause && !is_Pause) {
            Time.timeScale = 0;
            is_Pause = true;
            pause_Trigger = true;
            Pause_Game();
        }
        //一時停止解除
        else if (Input.GetKeyDown(KeyCode.Escape) && is_Pause) {
            Time.timeScale =1;
            is_Pause = false;
            release_Pause_Trigger = true;
            Release_Pause_Game();
        }
        //一時停止検知用
        Pause_Trigger();
        
    }

    //一時停止時の処理
    private void Pause_Game() {
        //自機の操作を止める
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(false);
        }
    }

    //一時停止解除時の処理
    private void Release_Pause_Game() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            PlayerController _playerController = player.GetComponent<PlayerController>();
            _playerController.Set_Playable(true);
        }
    }


    //1時停止中かどうかを返すメソッド
    public bool Is_Pause() {
        if (is_Pause) {
            return true;
        }
        return false;
    }


    //一時停止を検知するよう
    public bool Pause_Trigger() {
        if (pause_Trigger) {
            pause_Trigger = false;
            return true;
        }
        return false;
    }

    //一時停止解除を検知する用
    public bool Release_Pause_Trigger() {
        if (release_Pause_Trigger) {
            release_Pause_Trigger = false;
            return true;
        }
        return false;
    }


    //can_PauseのSetter
    public void Set_Pausable(bool can_Pause) {
        this.can_Pause = can_Pause;
    }

}
