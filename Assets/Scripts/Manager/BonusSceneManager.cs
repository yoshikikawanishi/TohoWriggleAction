using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSceneManager : MonoBehaviour {

    //Awake
    private void Awake() {
        /*--------------  鍵  ------------------*/
        if (!PlayerPrefs.HasKey("Stage2_BonusScene")) {
            PlayerPrefs.SetInt("Stage2_BonusScene", 1);
        }
        if (!PlayerPrefs.HasKey("Stage3_BonusScene")) {
            PlayerPrefs.SetInt("Stage3_BonusScene", 1);
        }


        //テストプレイ中鍵開ける
        Debug.Log("Open Bonus Scene in TestPlay");
        PlayerPrefs.SetInt("Stage2_BonusScene", 1);
        PlayerPrefs.SetInt("Stage3_BonusScene", 1);
    }


    //シーン遷移
    public void Change_Scene(string next_Scene, Vector2 next_Pos) {
        SceneManager.LoadScene(next_Scene);
        if (PlayerPrefs.HasKey(next_Scene)) {
            PlayerPrefs.SetInt(next_Scene, 0);
        }
        StartCoroutine(Player_Pos(next_Pos));
    }

    private IEnumerator Player_Pos(Vector2 next_Pos) {
        yield return null;
        GameObject.FindWithTag("PlayerTag").transform.position = next_Pos;
        
    }
    
}
