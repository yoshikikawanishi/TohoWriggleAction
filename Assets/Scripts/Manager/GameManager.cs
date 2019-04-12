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

    }
	

	// Update is called once per frame
	void Update () {
		
	}


    //セーブ
    public void SaveData() {
        Debug.Log("Save");
        string scene = SceneManager.GetActiveScene().name;
        GameObject player = GameObject.FindWithTag("PlayerTag");

        PlayerPrefs.SetString("Scene", scene);
        PlayerPrefs.SetFloat("PosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PosY", player.transform.position.y);
        PlayerPrefs.SetInt("Score", _playerManager.score);
        PlayerPrefs.SetInt("Power", _playerManager.power);
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
            _playerManager.score = PlayerPrefs.GetInt("Score");
            _playerManager.power = PlayerPrefs.GetInt("Power");
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
        PlayerPrefs.SetString("Scene", "");
        PlayerPrefs.SetFloat("PosX", 0);
        PlayerPrefs.SetFloat("PosY", 0);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Power", 0);
    }

}
