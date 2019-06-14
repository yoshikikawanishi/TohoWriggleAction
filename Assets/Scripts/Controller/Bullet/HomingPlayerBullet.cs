using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingPlayerBullet : MonoBehaviour {

    //自機
    private GameObject player;
    //コンポーネント
    private Rigidbody2D _rigid;


	// Use this for initialization
	void Start () {
        //自機
        player = GameObject.FindWithTag("PlayerTag");
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
        StartCoroutine("Homing_Bullet");
    }
	
	
    //初めの数秒はホーミング
    private IEnumerator Homing_Bullet() {
        yield return new WaitForSeconds(0.4f);
        //初速
        if (player != null) {
            transform.LookAt2D(player.transform, Vector2.right);
            _rigid.velocity = transform.right * 100f;
        }
        //ホーミング
        for (float t = 0; t < 1f; t += Time.deltaTime) {
            if(player != null) {
                transform.LookAt2D(player.transform, Vector2.right);
                _rigid.velocity += (Vector2)transform.right * 5f;
                float dirVelocity = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
            }
            yield return null;
        }
    }
}
