using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_2Movie : MonoBehaviour {

    //自機
    private GameObject player;
    private WriggleController player_Controller;
    //慧音
    private GameObject keine;
    private KeineController keine_Controller;
    //カメラ
    private GameObject main_Camera;
    //スクリプト
    private PauseManager _pause;

    //慧音落下速度
    private float keine_Drop_Speed = 8f;


    //Awake
    private void Awake() {
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<WriggleController>();
        keine = GameObject.Find("Keine");
        keine_Controller = keine.GetComponent<KeineController>();
        main_Camera = GameObject.FindWithTag("MainCamera");
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
    }


    // Use this for initialization
    void Start () {
		
	}


    //受け止めろ！イベント
    public IEnumerator Catch_Event() {
        //初期設定
        main_Camera.GetComponent<CameraController>().enabled = false;
        GameObject scene_Background = main_Camera.transform.GetChild(0).gameObject;
        GameObject event_Background = main_Camera.transform.GetChild(1).gameObject;
        scene_Background.transform.SetParent(null);
        event_Background.transform.SetParent(null);
        player_Controller.Set_Playable(false);
        _pause.Set_Pausable(false);
        //カメラを上げる
        float rais_Speed = 0;
        while(main_Camera.transform.position.y <= 1530f) {
            //加速
            if (rais_Speed < 6f) {
                rais_Speed += 0.02f;
            }
            main_Camera.transform.position += new Vector3(0, rais_Speed, 0);
            yield return null;
        }
        main_Camera.transform.position = new Vector3(0, 1530f, -10);

        yield return new WaitForSeconds(1.0f);
        
        //慧音落ちてくる
        while (keine.transform.position.y > main_Camera.transform.position.y) {
            keine.transform.position += new Vector3(0, -keine_Drop_Speed, 0);
            yield return null;
        }

        //背景差し替え
        event_Background.SetActive(true);
        
        //カメラ下げる
        float drop_Speed = keine_Drop_Speed;
        keine.transform.SetParent(main_Camera.transform);
        while(main_Camera.transform.position.y > 800f) {
            main_Camera.transform.position += new Vector3(0, -keine_Drop_Speed, 0);
            yield return null;
        }

        keine.transform.SetParent(null); //慧音止める

        while (main_Camera.transform.position.y >= 0) {
            main_Camera.transform.position += new Vector3(0, -keine_Drop_Speed, 0);
            //減速
            if(drop_Speed > 0.2f) {
                drop_Speed -= 0.05f;
            }
            yield return null;
        }
        main_Camera.transform.position = new Vector3(0, 0, -10);

        //慧音落とす、受け止める
        player_Controller.Set_Playable(true);
        while(keine.transform.position.y > -100f) {
            keine.transform.position += new Vector3(0, -3f, 0) * Time.timeScale;
            //時間の進みを遅くする
            if(keine.transform.position.y < 130f && Time.timeScale == 1.0f) {
                Time.timeScale = 0.4f;
            }
            if (keine_Controller.Get_Is_Catched()) {
                break;
            }
            yield return null;
        }
        
        //明転
        SpriteRenderer white_Sprite = GameObject.Find("WhiteSprite").GetComponent<SpriteRenderer>();
        while(white_Sprite.color.a <= 1) {
            white_Sprite.color += new Color(0, 0, 0, 0.05f);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);

        //終了設定
        GameObject.Find("WhiteSprite").SetActive(false);
        _pause.Set_Pausable(true);
        main_Camera.GetComponent<CameraController>().enabled = true;
        Time.timeScale = 1.0f;
        event_Background.SetActive(false);
        scene_Background.SetActive(true);
        scene_Background.transform.SetParent(main_Camera.transform);
        After_Movie();
    }

    //終了設定
    private void After_Movie() {
        //慧音
        keine_Controller.enabled = false;
        TalkCharacter keine_Talk = keine.AddComponent<TalkCharacter>();
        if (keine_Controller.Get_Is_Catched()) {
            keine_Talk.Set_Status("KeineText", 1, 1, new Vector2(32f, 32f));
        }
        else {
            keine_Talk.Set_Status("KeineText", 2, 2, new Vector2(32f, 32f));
        }
        keine.transform.position = new Vector3(220f, -80f);
        //自機
        player.transform.position = new Vector3(-226f, -80f);
    }

}
