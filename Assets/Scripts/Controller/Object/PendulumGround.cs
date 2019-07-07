using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumGround : MonoBehaviour {

    //自機
    private GameObject player;
    private Rigidbody2D player_Rigid;
    private WriggleController player_Controller;

    //動き始め
    private bool start_Action = false;

    //移動距離
    [SerializeField] private float move_Distance = 192f;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        player_Rigid = player.GetComponent<Rigidbody2D>();
        player_Controller = player.GetComponent<WriggleController>();
	}
	

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            if (!start_Action && player_Rigid.velocity.y < 7f && !player_Controller.Get_Is_Fly()) {
                start_Action = true;
                Pendulum_Move();
            }
            StopCoroutine("Drop_Player");
            player.transform.SetParent(gameObject.transform);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (player.transform.parent == gameObject.transform) {
            StartCoroutine("Drop_Player");
        }
    }

    private IEnumerator Drop_Player() {
        yield return new WaitForSeconds(0.1f);
        player.transform.SetParent(null);
    }

    //動く
    private void Pendulum_Move() {
        MoveBetweenTwoPoints _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        Vector3 next_Pos = transform.position + new Vector3(move_Distance, 0);
        _move.Start_Move(next_Pos, -64f, 0.016f);
    }

}
