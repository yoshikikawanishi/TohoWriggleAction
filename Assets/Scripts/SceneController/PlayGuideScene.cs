using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayGuideScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerManager pm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>();
        pm.life = 3;
        pm.stock = 3;
        Display_Play_Guide_Text();
	}


    //操作方法の表示
    private void Display_Play_Guide_Text() {
        KeyConfig k = InputManager.Instance.keyConfig;

        Text text = GameObject.Find("ControlleGuideText2").GetComponent<Text>();

        text.text   =  k.GetKeyCode("Jump")[0].ToString()   +
        "\n\n\n"    +  k.GetKeyCode("Shot")[0].ToString() +
        "\n\n\n"    +  "↓ " + k.GetKeyCode("Shot")[0].ToString() +
        "\n\n\n"    +  k.GetKeyCode("Fly")[0].ToString()    +
        "\n\n\n"    +  k.GetKeyCode("Pause")[0].ToString();

        text = GameObject.Find("ControlleGuideText3").GetComponent<Text>();

        text.text   = k.GetKeyCode("Jump")[1].ToString() +
        "\n\n\n"    + k.GetKeyCode("Shot")[1].ToString() +
        "\n\n\n"    + "↓ " + k.GetKeyCode("Shot")[1].ToString() +
        "\n\n\n"    + k.GetKeyCode("Fly")[1].ToString() +
        "\n\n\n"    + k.GetKeyCode("Pause")[1].ToString();
    }


}
