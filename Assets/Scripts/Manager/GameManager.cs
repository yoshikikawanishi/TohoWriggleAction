using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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
    }


    //スクリプト
    private PlayerManager _playerManager;


	// Use this for initialization
	void Start () {
        //スクリプトの取得
        _playerManager = GetComponent<PlayerManager>();

        /*　データ消去のテスト */
        DeleteData();

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
        //シーン遷移
        string scene = PlayerPrefs.GetString("Scene");
        if (scene != "") {
            SceneManager.LoadScene(scene);
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
        //データがないとき
        else {
            Debug.Log("No Data");
            scene = "Stage1_1Scene";
            SceneManager.LoadScene(scene);
        }
    }


    //データの消去
    public void DeleteData() {
        PlayerPrefs.SetString("Scene", "Stage1_1Scene");
        PlayerPrefs.SetFloat("PosX", -160f);
        PlayerPrefs.SetFloat("PosY", -78f);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Power", 0);
        PlayerPrefs.SetInt("Stock", 3);
        PlayerPrefs.SetInt("Continue", 0);
        PlayerPrefs.SetString("Option", "Flies");
    }

}
