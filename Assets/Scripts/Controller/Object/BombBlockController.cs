using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlockController : MonoBehaviour {

    //誘爆ブロック
    private List<GameObject> caused_Blocks = new List<GameObject>();
    [SerializeField] List<Vector2> directions = new List<Vector2>();

    private bool is_Crashed = false;


    //Start
    private void Start() {
        //誘爆ブロックの生成
        caused_Blocks.Add(transform.GetChild(1).gameObject);
        caused_Blocks[0].transform.position = transform.position + (Vector3)directions[0] * 16;
        for(int i = 1; i < directions.Count; i++) {
            caused_Blocks.Add(Instantiate(caused_Blocks[i - 1]));
            caused_Blocks[i].transform.position = caused_Blocks[i - 1].transform.position;
            caused_Blocks[i].transform.position += (Vector3)directions[i] * 16;
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerAttackTag" || collision.tag == "PlayerBulletTag" || collision.tag == "BeetleBulletTag") {
            if (!is_Crashed) {
                is_Crashed = true;
                Crash();
            }
        }
    }


    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "DamageGroundTag") {
            if (!is_Crashed) {
                is_Crashed = true;
                Crash();
            }
        }
    }


    //爆発
    private void Crash() {
        if (transform.childCount != 0) {
            //エフェクト出す
            Effect(gameObject);
            //誘爆
            StartCoroutine("Caused_Explode");
            //消滅
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
    }


    //誘爆
    private IEnumerator Caused_Explode() {
        for(int i = 0; i < caused_Blocks.Count; i++) { 
            yield return new WaitForSeconds(0.1f);
            Effect(caused_Blocks[i]);
            Destroy(caused_Blocks[i]);
        }
    }


    //エフェクト
    private void Effect(GameObject block) {
        GameObject effect = block.transform.GetChild(0).gameObject;
        effect.transform.SetParent(null);
        AudioClip clip = effect.GetComponent<AudioSource>().clip;
        effect.GetComponent<AudioSource>().PlayOneShot(clip);
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(effect, 1.0f);
    }
}
