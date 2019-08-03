using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLand : MonoBehaviour {

    //コンポーネント
    private MoveBetweenTwoPoints _move;

    //時間
    private float[] span;
    private float time = 0;

    //移動
    private Vector2 default_Pos;
    private Vector2 next_Pos;
    [SerializeField]
    private List<Vector2> positions = new List<Vector2>();

    private int count = 1;


	// Use this for initialization
	void Start () {
        _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        default_Pos = transform.position;
        span = GameObject.FindWithTag("ScriptsTag").GetComponent<Stage4_1Scene>().bgm_Match_Span;
    }
	
	// Update is called once per frame
	void Update () {
	    if(time < span[count%span.Length]) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            next_Pos = default_Pos + positions[count % positions.Count];
            _move.Start_Move(next_Pos, 0, 0.02f);
            count++;
        }
	}


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(gameObject.transform);
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            collision.transform.parent.SetParent(null);
        }
    }
}
