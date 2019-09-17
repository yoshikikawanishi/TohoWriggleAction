using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    //スクリプト
    private PlayerManager _playerManager;
    private SceneProgress _sceneProgress;

    //シーンに初めて訪れたかどうか
    private bool first_Visit_Frag = true;
    
    //解像度
    public int ScreenWidth = 960;
    public int ScreenHeight = 540;

    //オーディオ
    public AudioMixer audio_Mixer;

    //シングルトン用
    public static GameManager instance;


    void Awake() {
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer) {
            Screen.SetResolution(ScreenWidth, ScreenHeight, false);
        }

        //シングルトン
        if (instance != null) {
            Destroy(this.gameObject);
        }
        else if (instance == null) {
            instance = this;
        }
        //シーンを遷移してもオブジェクトを消さない
        DontDestroyOnLoad(gameObject);

        //スクリプトの取得
        _sceneProgress = GetComponent<SceneProgress>();
       
        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;

        //初めてゲームを起動したとき
        //テスト
        Debug.Log("First_Visit_Test");
        //PlayerPrefs.DeleteKey("Is_First_Open_Game");

        if (!PlayerPrefs.HasKey("Is_First_Open_Game")) {
            Do_First_Time_Setting();
        }


        /*　データ消去のテスト */
        Debug.Log("Delete_Data");
        //DeleteData();
        //DoremyHat.Delete_Data();
        /*
        ClearDataManager clear_Data = new ClearDataManager();
        clear_Data.Delete_Data();
        */

    }


    // Use this for initialization
    void Start () {
        //スクリプトの取得
        _playerManager = GetComponent<PlayerManager>();

        //オーディオの初期化
        Set_Up_Audio_Mixer();


        //ビルド前に初期化、消去すること
        Debug.Log("Check Before Build About[SaveDate, SceneProgress, DoremyHat, KeyConfig, ClearData, AudioVolume]");
        
    }


    //シーン読み込み時に呼ばれる関数
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //進行度の更新、初めて訪れたかどうかの確認
        first_Visit_Frag = _sceneProgress.Update_Is_Visited(scene.name);
    }


    //初めてゲームを起動したとき
    public void Do_First_Time_Setting() {
        Debug.Log("First_Open_Game");

        //起動済みを保存する
        PlayerPrefs.SetInt("Is_First_Open_Game", 0);
    }


    //セーブ
    public void SaveData() {
        string scene = SceneManager.GetActiveScene().name;
        GameObject player = GameObject.FindWithTag("PlayerTag");

        PlayerPrefs.SetString("Scene", scene);
        PlayerPrefs.SetFloat("PosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PosY", player.transform.position.y);
        PlayerPrefs.SetInt("Life", _playerManager.life);
        PlayerPrefs.SetInt("Score", _playerManager.score);
        PlayerPrefs.SetInt("Power", _playerManager.power);
        PlayerPrefs.SetInt("Stock", _playerManager.stock);
        PlayerPrefs.SetInt("Continue", _playerManager.continue_Count);
        PlayerPrefs.SetString("Option", _playerManager.option_Type);
      
    }


    //ロード
    public IEnumerator LoadData() {
        //データがないとき
        if (!PlayerPrefs.HasKey("Scene")) {
            PlayerPrefs.SetString("Scene", "Stage1_1Scene");
            PlayerPrefs.SetFloat("PosX", -160f);
            PlayerPrefs.SetFloat("PosY", -82f);
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Power", 0);
            PlayerPrefs.SetInt("Life", 3);
            PlayerPrefs.SetInt("Stock", 3);
            PlayerPrefs.SetInt("Continue", 0);
            PlayerPrefs.SetString("Option", "Flies");
        }
        //データの読み込み
        SceneManager.LoadScene(PlayerPrefs.GetString("Scene"));
        yield return null;
        //座標
        Vector2 pos = new Vector2(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"));
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player.transform.position = pos;
        //ステータス
        _playerManager.life = PlayerPrefs.GetInt("Life");
        _playerManager.score = PlayerPrefs.GetInt("Score");
        _playerManager.power = PlayerPrefs.GetInt("Power");
        _playerManager.stock = PlayerPrefs.GetInt("Stock");
        _playerManager.continue_Count = PlayerPrefs.GetInt("Continue");
        _playerManager.option_Type = PlayerPrefs.GetString("Option");
    }


    //データの消去
    public void DeleteData() {
        PlayerPrefs.DeleteAll();
        _sceneProgress.Delete_Progress();
        PlayerPrefs.SetInt("Is_First_Open_Game", 0);
    }


    /*-----------------ゲームシーンかどうか-------------*/
    public bool Is_Game_Scene() {
        List<string> not_Game_Scene_List = new List<string>();
        not_Game_Scene_List.Add("TitleScene");
        not_Game_Scene_List.Add("GameOverScene");
        not_Game_Scene_List.Add("ConfigScene");
        not_Game_Scene_List.Add("UnderGroundScene");
        not_Game_Scene_List.Add("EndingScene");
        not_Game_Scene_List.Add("StaffRollScene");

        string now_Scene = SceneManager.GetActiveScene().name;
        foreach (string scene in not_Game_Scene_List) {
            if (now_Scene == scene) {
                return false;
            }
        }
        return true;
    }


    //現在のシーンに初めて訪れたかどうか
    public bool Is_First_Visit() {
        if (first_Visit_Frag) {
            return true;
        }
        return false;
    }


    //引数シーンに訪れたことがあるかどうか
    public bool Is_Visited(string scene_Name) {
        return _sceneProgress.Is_Exist_Scene(scene_Name);
    }


    //シーンの遷移と自機の位置
    public IEnumerator Load_Scene(string next_Scene, Vector2 player_Pos) {
        SceneManager.LoadScene(next_Scene);
        yield return null;
        GameObject.FindWithTag("PlayerTag").transform.position = player_Pos;
    }


    //オーディオの初期化
    public void Set_Up_Audio_Mixer() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\AudioSetting.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        audio_Mixer.SetFloat("BGMvol", float.Parse(text.textWords[1, 1]));
        audio_Mixer.SetFloat("SEvol", float.Parse(text.textWords[2, 1]));
    }
}
