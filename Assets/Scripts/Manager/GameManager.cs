using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //スクリプト
    private PlayerManager _playerManager;
    private SceneProgress _sceneProgress;

    //シーンに初めて訪れたかどうか
    private bool first_Visit_Frag = true;

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

        //スクリプトの取得
        _sceneProgress = GetComponent<SceneProgress>();
       
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
        //進行度の更新、初めて訪れたかどうかの確認
        first_Visit_Frag = _sceneProgress.Update_Is_Visited(scene.name);
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
            PlayerPrefs.SetFloat("PosY", -78f);
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
    }


    /*-----------------ゲームシーンかどうか-------------*/
    public bool Is_Game_Scene() {
        List<string> not_Game_Scene_List = new List<string>();
        not_Game_Scene_List.Add("TitleScene");
        not_Game_Scene_List.Add("GameOverScene");
        not_Game_Scene_List.Add("ConfigScene");
        not_Game_Scene_List.Add("UnderGroundScene");

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
}
