﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterEndingMovie : MonoBehaviour {

	//
    public void Start_After_Ending_Movie() {
        StartCoroutine("After_Ending_Movie_Routine");
    }

    private IEnumerator After_Ending_Movie_Routine() {
        yield return new WaitForSeconds(1.5f);
        MessageDisplay _message = GetComponent<MessageDisplay>();
        _message.Start_Display("AfterEndingText", 1, 7);
        yield return new WaitUntil(_message.End_Message);
        yield return new WaitForSeconds(1.0f);
        _message.Start_Display_Auto("AfterEndingText", 8, 8, 3.5f, 0);
        yield return new WaitUntil(_message.End_Message);
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScene");
    }
}
