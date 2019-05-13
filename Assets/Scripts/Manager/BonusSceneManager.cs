using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSceneManager : MonoBehaviour {

    //元のシーンの情報
    private string origin_Scene;
    private Vector3 origin_Pos;

    //start
    private void Start() {
        /*鍵を開ける*/
        PlayerPrefs.SetInt("Stage2_Bonus", 0);
    }


    //ボーナスシーンに入る
    public void Enter_Bonus_Scene() {
        origin_Scene = SceneManager.GetActiveScene().name;
        origin_Pos = GameObject.FindWithTag("PlayerTag").transform.position + new Vector3(64f, 0);
        SceneManager.LoadScene("BonusScene");
    }


    //元のシーンに戻る
    public IEnumerator Exit_Bonus_Scene() {
        SceneManager.LoadScene(origin_Scene);
        yield return null;
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null){
            player.transform.position = origin_Pos;
        }
    }
}
