using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowRabit : MonoBehaviour {

    //コンポーネント
    private Animator _anim;
    private BulletFunctions _bullet;
    private Renderer _renderer;

    //弾
    [SerializeField] private GameObject bullet;

    //時間
    private float[] span;
    private float time = 0;

    private int count = 0;


    //Awake
    private void Awake() {
        _anim = GetComponent<Animator>();
        _bullet = transform.GetChild(0).GetComponent<BulletFunctions>();
        _renderer = GetComponent<Renderer>();
    }

    
    // Use this for initialization
    void Start () {
        span = GameObject.FindWithTag("ScriptsTag").GetComponent<Stage4_1Scene>().bgm_Match_Span;
        _bullet.Set_Bullet(bullet);
    }
	

	// Update is called once per frame
	void Update () {
	    if(time < span[count % span.Length]) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            _anim.SetTrigger("AttackTrigger");
            if (_renderer.isVisible) {
                _bullet.Some_Way_Bullet(5, 60f, 0, 30, 6.0f);
                UsualSoundManager.Shot_Sound();
            }
            count++;
        }
	}
}
