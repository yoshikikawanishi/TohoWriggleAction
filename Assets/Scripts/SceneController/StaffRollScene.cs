using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaffRollScene : MonoBehaviour {

    //フィールド
    private StaffRollManager staff_Roll;

    
    // Use this for initialization
	void Start () {
        GetComponent<FadeInOut>().Start_Fade_In();

        staff_Roll = GetComponent<StaffRollManager>();
        staff_Roll.Start_Staff_Roll();
    }
	

	// Update is called once per frame
	void Update () {
        if (staff_Roll.Is_End_Staff_Roll()) {
            StartCoroutine("Go_Title_Scene");
        }
	}


    //タイトルに戻る
    private IEnumerator Go_Title_Scene() {
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
