using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceDisplay : MonoBehaviour {
    
    private string fileName;
    private int id;
    private Text text;

    public AudioClip audio_Clip;
    private float sound_Volume = 0.07f;
    private AudioSource _audio;

    private bool is_End_Display = false;


    //Awake
    private void Awake() {
        audio_Clip = Resources.Load("Message") as AudioClip;    
    }

    //表示開始
    public void Start_Display(string fileName, int id, Text display_Text) {
        is_End_Display = false;
        this.fileName = fileName;
        this.id = id;
        text = display_Text;
        text.text = "";
        if(_audio == null) {
            _audio = gameObject.AddComponent<AudioSource>();
            _audio.volume = sound_Volume;
        }
        StartCoroutine("Display_Sentence");
    }

    private IEnumerator Display_Sentence() {
        TextFileReader text_Words = new TextFileReader(fileName);
        string sentence = text_Words.textWords[id, 1];
        for (int i = 0; i < sentence.Length; i++) {
            if (sentence[i] == '/') {
                text.text += "\n";
            }
            else {
                text.text += sentence[i];
            }
            _audio.PlayOneShot(audio_Clip);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitUntil(Wait_Input);
        is_End_Display = true;
    }


    //入力待ち
    private bool Wait_Input() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            return true;
        }
        return false;
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
