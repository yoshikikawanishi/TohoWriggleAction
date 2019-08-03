using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : MonoBehaviour {

    //カメラ
    private GameObject main_Camera;

    //消滅時のエフェクト
    [SerializeField] private GameObject delete_Effect;
    //初期位置
    private float default_Y;
    //振幅
    [SerializeField] private float amplitude = 32;
    //速さ
    [SerializeField] private float speed = 1;
    //方向
    private int direction = 1;

    //被弾の見地
    private bool is_Hit = false;


    // Use this for initialization
    void Start() {
        main_Camera = GameObject.FindWithTag("MainCamera");
        default_Y = transform.position.y;
    }


    //FixedUpdate
    private void FixedUpdate() {
        if (Mathf.Abs(main_Camera.transform.position.x - transform.position.x) < 240f) {
            transform.position += new Vector3(0, speed * direction);
            //方向転換
            if (transform.position.y >= default_Y + amplitude / 2 && direction == 1) {
                direction = -1;
            }
            else if (transform.position.y <= default_Y - amplitude / 2 && direction == -1) {
                direction = 1;
            }
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerAttackTag" || collision.tag == "PlayerBulletTag" && !is_Hit) {
            Crash();
            is_Hit = true;
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "PlayerBodyTag" && !is_Hit) {
            Crash();
            is_Hit = true;
        }
    }


    //誘爆
    private void Crash() {
        //消す
        GameObject effect = Instantiate(delete_Effect);
        effect.transform.position = transform.position;
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject, 3.0f);
        //誘爆ブロックの取得
        List<GameObject> upper_Blocks = new List<GameObject>();
        List<GameObject> lower_Blocks = new List<GameObject>();
        foreach(RaycastHit2D hit in Physics2D.RaycastAll(transform.position, new Vector2(0, 1), 150)) {
            if(hit.collider.tag == "GroundTag") {
                upper_Blocks.Add(hit.collider.gameObject);
            }
        }
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, new Vector2(0, -1), 150)) {
            if (hit.collider.tag == "GroundTag") {
                lower_Blocks.Add(hit.collider.gameObject);
            }
        }
        //誘爆ブロックを順番に消す
        StartCoroutine(Order_Crash(upper_Blocks));
        StartCoroutine(Order_Crash(lower_Blocks));
    }


    //誘爆ブロックを順番に消す
    private IEnumerator Order_Crash(List<GameObject> blocks) {
        for (int i = 0; i < blocks.Count; i++) {
            GameObject effect = Instantiate(Resources.Load("Effect/SmokeEffect") as GameObject);
            effect.transform.position = blocks[i].transform.position;
            Destroy(effect, 0.2f);
            Destroy(blocks[i]);
            GetComponents<AudioSource>()[1].Play();
            yield return new WaitForSeconds(3f / 28f);
        }
    }

}
