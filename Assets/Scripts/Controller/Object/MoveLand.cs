using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLand : MonoBehaviour {

    //コンポーネント
    private MoveBetweenTwoPoints _move;
    //スクリプト
    private WriggleController player_Controller;

    //時間
    private float span = 3.0f;
    private float time = 1.5f;

    //移動
    private Vector2 default_Pos;
    private Vector2 next_Pos;
    [SerializeField]
    private List<Vector2> positions = new List<Vector2>();

    private int count = 1;


	// Use this for initialization
	void Start () {
        _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<WriggleController>();
        default_Pos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
	    if(time < span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            next_Pos = default_Pos + positions[count % positions.Count];
            _move.Start_Move(next_Pos, 0, 0.01f);
            count++;
        }
	}


    //OnTriggerEnter
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            if (collision.transform.parent != transform) {
                collision.transform.parent.SetParent(gameObject.transform);
            }
            if (player_Controller.Get_Is_Fly()) {
                collision.transform.parent.SetParent(null);
            }
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(null);
        }
    }
}
