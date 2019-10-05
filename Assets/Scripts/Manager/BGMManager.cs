using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour {

    //BGM
    [SerializeField] private AudioSource title_BGM;
    [SerializeField] private AudioSource stage_BGM1;
    [SerializeField] private AudioSource stage_BGM2;
    [SerializeField] private AudioSource stage_BGM3;
    [SerializeField] private AudioSource stage1_Boss_BGM;
    [SerializeField] private AudioSource stage2_Boss_BGM;
    [SerializeField] private AudioSource stage3_Boss_BGM;
    [SerializeField] private AudioSource stage4_Boss_BGM;
    [SerializeField] private AudioSource stage5_Boss_BGM;
    [SerializeField] private AudioSource extra_Boss_BGM;
    [SerializeField] private AudioSource base_BGM;
    [SerializeField] private AudioSource staff_Roll_BGM;
    [SerializeField] private AudioSource ending_BGM;

    
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
        if (now_BGM != null) {
            now_BGM.Stop();
            now_BGM = null;
        }
    }

    //BGM変える
    private void Change_BGM(AudioSource bgm) {
        //BGM同じとき
        if (now_BGM == bgm) {
            return;
        }
        //変更時
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


    //曲のフェードアウト
    public void Start_Fade_Out(float speed, float time_Span) {
        if (now_BGM != null) {
            StartCoroutine(Fade_Out(speed, time_Span));
        }
    }

    private IEnumerator Fade_Out(float speed, float time_Span) {
        AudioSource audio = now_BGM;
        float default_Volume = audio.volume;
        while(audio.volume > 0) {
            audio.volume -= speed;
            yield return null;
        }
        yield return new WaitForSeconds(time_Span);
        audio.volume = default_Volume;
    }


    //シーン読み込み時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "TitleScene"       : Change_BGM(title_BGM);    break;
            case "GameOverScene"    : Stop_BGM();               break;
            case "Stage1_1Scene"    : Change_BGM(stage_BGM1);   break;
            case "Stage2_1Scene"    : Change_BGM(stage_BGM1);   break;
            case "Stage2_2Scene"    : Change_BGM(stage_BGM1);   break;
            case "Base_1Scene"      : Change_BGM(base_BGM);     break;
            case "Stage3_1Scene"    : Change_BGM(stage_BGM2);   break;
            case "Stage3_2Scene"    : Change_BGM(stage_BGM2);   break;
            case "Base_2Scene"      : Stop_BGM();               break;
            case "Stage4_1Scene"    : Change_BGM(stage_BGM3);   break;
            case "Stage4_2Scene"    : Change_BGM(stage_BGM3);   break;
            case "Stage5_1Scene"    : Change_BGM(stage_BGM3);   break;
            case "ExtraFrontScene"  : Stop_BGM();               break;
            case "EndingScene"      : Stop_BGM();               break;
            case "AfterEndingScene" : Stop_BGM();               break;
        }
    }
}
