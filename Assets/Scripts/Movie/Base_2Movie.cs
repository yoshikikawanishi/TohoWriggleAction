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
    //UI
    private GameObject catch_Canvas;
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
        catch_Canvas = GameObject.Find("CatchCanvas");
        _pause = GameObject.FindWithTag("CommonScriptsTag").GetComponent<PauseManager>();
    }


    // Use this for initialization
    void Start () {
        catch_Canvas.SetActive(false);
	}


    //受け止めろ！イベント
    public IEnumerator Catch_Event() {
        //2回目以降
        if (!GameObject.FindWithTag("CommonScriptsTag").GetComponent<GameManager>().Is_First_Visit()) {
            GetComponent<BorderFadeIn>().Start_Fade_In(1f);
            Debug.Log("not first visit");
            StopAllCoroutines();
            yield return null;
        }

        //初期設定
        main_Camera.GetComponent<CameraController>().enabled = false;
        GameObject scene_Background = main_Camera.transform.GetChild(0).gameObject;
        GameObject event_Background = main_Camera.transform.GetChild(1).gameObject;
        scene_Background.transform.SetParent(null);
        event_Background.transform.SetParent(null);
        player_Controller.Set_Playable(false);
        _pause.Set_Pausable(false);

        //リグル移動
        StartCoroutine("Wriggle_Timeline");

        yield return new WaitForSeconds(1.0f);

        //フェードイン
        GetComponent<BorderFadeIn>().Start_Fade_In(0.01f);
        yield return new WaitForSeconds(1.5f);

        //カメラを上げる
        float rais_Speed = 0;
        while(main_Camera.transform.position.y <= 1530f) {
            //加速
            if (rais_Speed < 8f) {
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
            main_Camera.transform.position += new Vector3(0, -drop_Speed, 0);
            yield return null;
        }

        keine.transform.SetParent(null); //慧音止める

        while (main_Camera.transform.position.y >= 0) {
            main_Camera.transform.position += new Vector3(0, -drop_Speed, 0);
            //減速
            if(drop_Speed > 0.4f) {
                drop_Speed -= 0.04f;
            }
            yield return null;
        }
        main_Camera.transform.position = new Vector3(0, 0, -10);

        //UI表示
        StartCoroutine("Catch_UI");

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
        yield return new WaitForSeconds(1.0f);

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


    //リグル移動
    private IEnumerator Wriggle_Timeline() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player.GetComponent<WriggleController>().Change_Parameter("DashBool");
        for (float t = 0; t < 1.5f; t += 0.016f) {
            player.transform.position += new Vector3(1f, 0);
            yield return new WaitForSeconds(0.016f);
        }
        player.GetComponent<WriggleController>().Change_Parameter("IdleBool");
    }

    //受け止めろ!表示
    private IEnumerator Catch_UI() {
        for (int i = 0; i < 3; i++) {
            catch_Canvas.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            catch_Canvas.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
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
