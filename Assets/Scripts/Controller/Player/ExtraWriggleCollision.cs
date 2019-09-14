using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraWriggleCollision : WriggleCollision {


    public override void Miss() {
        Invoke("Back_Title", 1.0f);
        GameObject.FindWithTag("PlayerTag").SetActive(false);
    }

    

    private void Back_Title() {
        SceneManager.LoadScene("TitleScene");
    }
}
