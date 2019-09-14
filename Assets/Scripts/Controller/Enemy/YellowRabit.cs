using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRabit : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private BulletFunctions _bullet;

    //自機
    private GameObject player;

    //弾
    [SerializeField] private GameObject bullet;

    //時間
    private float span = 3.0f;
    private float time = 1.5f;

    private int count = 0;


    //Awake
    private void Awake() {
        _anim = GetComponent<Animator>();
        _bullet = transform.GetChild(0).GetComponent<BulletFunctions>();
    }

    
    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("PlayerTag");
        _bullet.Set_Bullet(bullet);
    }
	

	// Update is called once per frame
	void Update () {
	    if(time < span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            _anim.SetTrigger("AttackTrigger");
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < 500f) {
                _bullet.Some_Way_Bullet(5, 60f * transform.localScale.x, 0, 30, 6.0f);
                UsualSoundManager.Shot_Sound();
            }
            count++;
        }
	}
}
