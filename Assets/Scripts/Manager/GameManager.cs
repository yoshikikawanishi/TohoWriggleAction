using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    

    //スクリプト
    private PlayerManager _playerManager;

    //進行度
    private Dictionary<string, bool> progress_Dic = new Dictionary<string, bool>();

    //シングルトン用
    public static GameManager instance;
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

        //進行度の初期設定
        Progress_Dic_Setting();

        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    // Use this for initialization
    void Start () {
        //スクリプトの取得
        _playerManager = GetComponent<PlayerManager>();
        //キー
        KeyConfig keyConfig = new KeyConfig();
        keyConfig.Create_InputManager();

        /*　データ消去のテスト */
        //DeleteData();

        /* コンフィグテスト */
        /*
        KeyConfig keyConfig = new KeyConfig();
        keyConfig.Change_Button("Jump/Submit", "z", false);
        keyConfig.Create_InputManager();
        */
    }


    //シーン読み込み時に呼ばれる関数
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //進行度の更新
        StartCoroutine("Update_Progress", scene.name);
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
        PlayerPrefs.SetInt("Progress", Get_Progress_Num());
    }


    //ロード
    public IEnumerator LoadData() {
        //データがないとき
        if (!PlayerPrefs.HasKey("Scene")) {
            PlayerPrefs.SetString("Scene", "Stage1_1Scene");
            PlayerPrefs.SetFloat("PosX", -160f);
            PlayerPrefs.SetFloat("PosY", -78f);
            PlayerPrefs.SetInt("Score", 0);
            PlayerPrefs.SetInt("Power", 0);
            PlayerPrefs.SetInt("Life", 3);
            PlayerPrefs.SetInt("Stock", 3);
            PlayerPrefs.SetInt("Continue", 0);
            PlayerPrefs.SetString("Option", "Flies");
            PlayerPrefs.SetInt("Progress", 0);
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
        Set_Progress(PlayerPrefs.GetInt("Progress"));
    }


    //データの消去
    public void DeleteData() {
        PlayerPrefs.DeleteAll();
    }


    /*-----------------ゲームシーンかどうか-------------*/
    public bool Is_Game_Scene() {
        List<string> not_Game_Scene_List = new List<string>();
        not_Game_Scene_List.Add("TitleScene");
        not_Game_Scene_List.Add("GameOverScene");
        not_Game_Scene_List.Add("ConfigScene");

        string now_Scene = SceneManager.GetActiveScene().name;
        foreach (string scene in not_Game_Scene_List) {
            if (now_Scene == scene) {
                return false;
            }
        }
        return true;
    }


    /*-----------------進行度の初期設定-----------------*/
    private void Progress_Dic_Setting() {
        progress_Dic.Add("Stage1_1Scene", false);
        progress_Dic.Add("Stage1_BossScene", false);
        progress_Dic.Add("Stage2_1Scene", false);
        progress_Dic.Add("Stage2_2Scene", false);
        progress_Dic.Add("Stage2_BossScene", false);
        progress_Dic.Add("Base_1Scene", false);
    }


    //進行度の更新
    private IEnumerator Update_Progress(string loaded_Scene) {
        //ロードされたシーンのスタートで、更新前の進行度を取得できるようにする
        yield return null;
        yield return null;
        yield return null;
        //更新
        if (progress_Dic.ContainsKey(loaded_Scene)) {
            if (!progress_Dic[loaded_Scene]) {
                progress_Dic[loaded_Scene] = true;
                PlayerPrefs.SetInt("Progress", Get_Progress_Num());
            }
        }
    }


    //引数番目までの進行度をtrueにする
    private void Set_Progress(int progress_Num) {
        List<string> keyList = new List<string>(progress_Dic.Keys);
        foreach (string key in keyList) {
            if (progress_Num > 0) {
                progress_Dic[key] = true;
            }
            else {
                break;
            }
            progress_Num--;
        }
    }


    //進行度の取得用
    public bool Is_First_Visit(string scene_Name) {
        if (progress_Dic[scene_Name]) {
            return false;
        }
        return true;
    }


    //何番目のシーンまで進んだか
    public int Get_Progress_Num() {
        int progress_Num = 0;
        foreach (bool value in progress_Dic.Values) {
            if (!value) {
                break;
            }
            progress_Num++;
        }
        return progress_Num;
    }
}
