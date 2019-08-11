using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour {

    //BGM
    [SerializeField] private AudioSource title_BGM;
    [SerializeField] private AudioSource stage_BGM1;
    [SerializeField] private AudioSource stage1_Boss_BGM;
    [SerializeField] private AudioSource stage2_Boss_BGM;
    [SerializeField] private AudioSource stage3_Boss_BGM;
    
    //現在流れているBGM 
    private AudioSource now_BGM;


    //シングルトン用
    public static BGMManager instance;
    void Awake() {
        //シングルトン
        if (instance != null) {
            Destroy(this.gameObject);
        }
        else if (instance == null) {
            instance = this;
        }
        //シーンを遷移してもオブジェクトを消さない
        DontDestroyOnLoad(gameObject);

        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //BGM止める
    public void Stop_BGM() {
        now_BGM.Stop();
    }

    //BGM始める
    private void Change_BGM(AudioSource bgm) {
        if (now_BGM != null) {
            now_BGM.Stop();
        }
        bgm.Play();
        now_BGM = bgm;
    }

    //BGM始める(番号)
    public void Change_BGM_Index(int index) {
        if (now_BGM != null) {
            now_BGM.Stop();
        }
        AudioSource next_BGM = GetComponents<AudioSource>()[index];
        next_BGM.Play();
        now_BGM = next_BGM;
    }


    //シーン読み込み時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "Stage1_1Scene": Change_BGM(stage_BGM1); break;
            case "Stage2_1Scene": Change_BGM(stage_BGM1); break;
            case "Stage2_2Scene": Change_BGM(stage_BGM1); break;
            case "Stage3_1Scene": Change_BGM(stage_BGM1); break;
            case "Stage3_2Scene": Change_BGM(stage_BGM1); break;
        }
    }
}
