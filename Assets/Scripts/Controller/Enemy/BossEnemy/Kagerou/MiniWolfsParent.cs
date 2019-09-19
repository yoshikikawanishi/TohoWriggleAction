using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWolfsParent : MonoBehaviour {

    [SerializeField] private GameObject wolf_Prefab;
    private List<GameObject> mini_Wolfs = new List<GameObject>();
    private List<MiniWolf> wolfs_Controller = new List<MiniWolf>();

    private GameObject kagerou;
    private KagerouController kagerou_Controller;
    private BossEnemyController boss_Controller;

    private bool is_Transformed = false;

    [SerializeField] private GameObject appear_Effect;


    // Use this for initialization
    void Start () {
        //取得
        Create_List();
        kagerou = GameObject.Find("Kagerou");
        kagerou_Controller = kagerou.GetComponent<KagerouController>();
        boss_Controller = kagerou.GetComponent<BossEnemyController>();
    }
	

	// Update is called once per frame
	void Update () {        
        //ちび影狼の内だれか倒されたら変身
        if (transform.childCount < 6 && !is_Transformed) {
            is_Transformed = true;
            Create_List();
            StartCoroutine("Do_Wolf_Attack");
        }
        //ライフが20以下になったら変身
        if (boss_Controller.life[2] < 20 && !is_Transformed) {
            is_Transformed = true;
            StartCoroutine("Do_Wolf_Attack");
        }
        //ちび影狼動き
        if (!is_Transformed) {
            for (int i = 0; i < transform.childCount; i++) {
                wolfs_Controller[i].Transition();
            }
        }
    }


    //出現
    public void Appear() {
        for(int i = 0; i < transform.childCount; i++) {
            var effect = Instantiate(appear_Effect);
            effect.transform.position = mini_Wolfs[i].transform.position;
        }
        Invoke("Appear_Wolfs", 1.5f);
    }
    private void Appear_Wolfs() {
        for(int i = 0; i < transform.childCount; i++) {
            mini_Wolfs[i].SetActive(true);
        }
    }


    //子供の取得
    private void Create_List() {
        mini_Wolfs.Clear();
        wolfs_Controller.Clear();
        for(int i = 0; i < transform.childCount; i++) {
            mini_Wolfs.Add(transform.GetChild(i).gameObject);
            wolfs_Controller.Add(mini_Wolfs[i].GetComponent<MiniWolf>());
        }
    }



    public IEnumerator Do_Wolf_Attack() {

        Create_List();
        //数を増やす
        while (mini_Wolfs.Count <= 4) {
            var wolf = Instantiate(wolf_Prefab, transform, false);
            wolf.transform.position = new Vector3(0, -116f);
            wolf.SetActive(true);
            mini_Wolfs.Add(wolf);
            wolfs_Controller.Add(wolf.GetComponent<MiniWolf>());
        }

        //無敵化
        for (int i = 0; i < mini_Wolfs.Count; i++) {
            mini_Wolfs[i].GetComponent<Enemy>().Set_Is_Invincible(true);
        }
        

        //エフェクト
        kagerou_Controller.Roar();
        for (int i = 0; i < mini_Wolfs.Count; i++) {
            wolfs_Controller[i].transform_Effect.Play();
        }

        yield return new WaitForSeconds(0.5f);

        //変身、方向転換
        for (int i = 0; i < mini_Wolfs.Count; i++) {
            wolfs_Controller[i].Transform_Wolf();
            wolfs_Controller[i].Look_Player();
        }

        yield return new WaitForSeconds(1.0f);

        //突進開始
        for (int i = 0; i < mini_Wolfs.Count; i++) {
            wolfs_Controller[i].StartCoroutine("Rush");
        }
    }



}
