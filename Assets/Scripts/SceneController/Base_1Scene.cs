using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base_1Scene : MonoBehaviour {

    //自機
    private GameObject player;

    [SerializeField] private GameObject fade_Out_Cover;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        GameManager gm = GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>();
        if (gm.Is_First_Visit()) {
            GetComponent<FadeInOut>().Start_Fade_In();
        }
	}
	
	// Update is called once per frame
	void Update () {
		//右端に行ったらシーン遷移
        if(player.transform.position.x > 800f) {
            StartCoroutine(Change_Scene());
        }
	}

    private IEnumerator Change_Scene() {
        fade_Out_Cover.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<FadeInOut>().Start_Fade_Out();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Stage3_1Scene");
    }
}
