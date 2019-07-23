using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundBackGround : MonoBehaviour {

    private SpriteRenderer back_Sprite;

    [SerializeField] private Sprite default_Texture;
    [SerializeField] private Sprite laugh_Texture;


	// Use this for initialization
	void Awake () {
        back_Sprite = GameObject.Find("BackGround").GetComponent<SpriteRenderer>();
	}
	

	//背景差し替え
    public void Change_Back_Ground() {
        back_Sprite.sprite = laugh_Texture;
    } 

    public void Restore_Back_Ground() {
        back_Sprite.sprite = default_Texture;
    }
}
