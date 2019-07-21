using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceDisplay : MonoBehaviour {
    
    private string fileName;
    private int id;
    private Text text;

    private bool is_End_Display = false;

    //表示開始
    public void Start_Display(string fileName, int id, Text display_Text) {
        is_End_Display = false;
        this.fileName = fileName;
        this.id = id;
        text = display_Text;
        text.text = "";
        StartCoroutine("Display_Sentence");
    }

    private IEnumerator Display_Sentence() {
        TextReader text_Words = new TextReader(fileName);
        string sentence = text_Words.textWords[id, 1];
        for (int i = 0; i < sentence.Length; i++) {
            text.text += sentence[i];
            yield return new WaitForSeconds(0.03f);
        }
        is_End_Display = true;
    }

    //表示終了の検知用
    public bool End_Display() {
        if (is_End_Display) {
            is_End_Display = false;
            return true;
        }
        return false;
    }

}
