using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterEndingScene : MonoBehaviour {

   
    
    // Use this for initialization
	void Start () {
        GetComponent<FadeInOut>().Start_Fade_In();
    }
	

	


    //タイトルに戻る
    private IEnumerator Go_Title_Scene() {
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
