using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniWolfsParent : MonoBehaviour {

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
            for (int i = 0; i < transform.childCount; i++) {
                wolfs_Controller[i].StartCoroutine("Transform_Wolf");
            }
        }
        //ライフが半分以下になったら変身
        if (boss_Controller.life[1] < boss_Controller.LIFE[1] / 2 && !is_Transformed) {
            is_Transformed = true;
            for (int i = 0; i < transform.childCount; i++) {
                wolfs_Controller[i].StartCoroutine("Transform_Wolf");
            }
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
        
        while (true) {
            //狼に変身
            yield return new WaitForSeconds(1.5f);
            kagerou_Controller.Roar();
            for (int i = 0; i < mini_Wolfs.Count; i++) {
                //エフェクト
                wolfs_Controller[i].transform_Effect.Play();
                yield return new WaitForSeconds(0.5f);

                //突進
                wolfs_Controller[i].StartCoroutine("Rush");
                while (!wolfs_Controller[i].Is_Hit_Wall()) {
                    yield return new WaitForSeconds(0.016f);
                }
                kagerou_Controller.Shake_Camera(0.25f, 4.0f);
            }
            //全員壁に衝突するまで待つ
            yield return new WaitUntil(Is_Stop_All_Wolfs);
        }
    }


    //全員壁に衝突したとき真
    private bool Is_Stop_All_Wolfs() {
        for(int i = 0; i < mini_Wolfs.Count; i++) {
            if (!wolfs_Controller[i].Is_Hit_Wall()) {
                return false;
            }
        }
        return true;
    }
}
