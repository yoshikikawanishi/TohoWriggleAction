using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSceneManager : MonoBehaviour {

    //元のシーンの情報
    private string origin_Scene;
    private Vector3 back_Pos;
    
    //start
    private void Start() {
        /*鍵を開ける*/
        PlayerPrefs.SetInt("Stage2_Bonus", 0);
    }


    //ボーナスシーンに入る
    public void Enter_Bonus_Scene(string bonus_Scene_Name, Vector3 back_Pos) {
        //元のシーンの情報を取得
        origin_Scene = SceneManager.GetActiveScene().name;
        this.back_Pos = back_Pos;
        //遷移
        SceneManager.LoadScene(bonus_Scene_Name);
    }


    //元のシーンに戻る
    public IEnumerator Exit_Bonus_Scene() {
        SceneManager.LoadScene(origin_Scene);
        yield return null;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null){
            player.transform.position = back_Pos;
        }
    }
}
