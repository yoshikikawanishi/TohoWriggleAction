using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouMini : MonoBehaviour {

    private List<GameObject> mini_Kagerous = new List<GameObject>();
    private BossEnemyController boss_Controller;

    private float default_X;
    private float move_Speed = 2f;
    private float move_Length = 80f;

    private int mini_Kagerous_Num = 6;

    private bool is_Transformed = false;


    private void Awake() {
        //初期位置
        default_X = transform.position.x;
        transform.position = new Vector3(default_X + Random.Range(-32, 32), transform.position.y);
    }

    // Use this for initialization
    void Start () {
        //取得
        boss_Controller = GameObject.Find("Kagerou").GetComponent<BossEnemyController>();
        //兄弟の取得
        for(int i = 0; i < mini_Kagerous_Num; i++) {
            mini_Kagerous[i] = transform.parent.GetChild(i).gameObject;
        }
    }


    //OnEnable
    private void OnEnable() {
        //ショット
        StartCoroutine("Shot");
    }


    // Update is called once per frame
    void Update () {
        //移動
        Transition();
        //ちび影狼の内だれか倒されたら変身
        if(mini_Kagerous.Count < mini_Kagerous_Num && !is_Transformed) {
            is_Transformed = true;
            Transform_Wolf();
        }
        //ライフが半分以下になったら変身
        if(boss_Controller.life[1] < boss_Controller.LIFE[1] && !is_Transformed) {
            is_Transformed = true;
            Transform_Wolf();
        }
	}


    //移動
    private void Transition() {
        if(transform.localScale.x == 1) {
            transform.position += new Vector3(-move_Speed, 0) * Time.timeScale;
            if(transform.position.x < default_X - move_Length) {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if(transform.localScale.x == -1) {
            transform.position += new Vector3(move_Speed, 0) * Time.timeScale;
            if (transform.position.x > default_X + move_Length) {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }


    //ショット
    private IEnumerator Shot() {
        BulletFunctions _bullet = GetComponent<BulletFunctions>();
        _bullet.Set_Bullet(Resources.Load("Bullet/PurpleBullet") as GameObject);
        float t = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(t);
        while (true) {
            _bullet.Odd_Num_Bullet(1, 0, 100f, 5.0f);
            yield return new WaitForSeconds(2.0f);
        }
    }


    //狼に変身
    public void Transform_Wolf() {
        Debug.Log("Transform Wolf");
    }
    
}
