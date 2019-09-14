using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGround : MonoBehaviour {

    //自機
    private GameObject player;
    private Rigidbody2D player_Rigid;
    private WriggleController player_Controller;

    //動き始め
    private bool start_Action = false;
    //落下終了
    private bool end_Drop = false;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        player_Rigid = player.GetComponent<Rigidbody2D>();
        player_Controller = player.GetComponent<WriggleController>();
	}
	

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        //落下開始
        if (collision.tag == "PlayerFootTag") {
            if (!start_Action && player_Rigid.velocity.y < 7f && !player_Controller.Get_Is_Fly()) {
                start_Action = true;
                StartCoroutine("Drop");
            }
            if (player.transform.parent != gameObject.transform) {
                player.transform.SetParent(gameObject.transform);
            }
        }
        //落下終了
        else if (collision.tag == "GroundTag" || collision.tag == "DamageGroundTag") {
            end_Drop = true;
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (player.transform.parent == gameObject.transform) {
            player.transform.SetParent(null);
        }
    }


    //落下
    private IEnumerator Drop() {
        float g = 9.8f;
        float t = 0.0f;
        float y = transform.position.y;
        while (!end_Drop) {
            //自由落下
            y = transform.position.y - (0.5f * g * t * t) * Time.timeScale;
            transform.position = new Vector3(transform.position.x, y);
            t += 0.015f;
            yield return new WaitForSeconds(0.015f);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(transform.GetChild(0).gameObject, 0.5f);

    }

}
