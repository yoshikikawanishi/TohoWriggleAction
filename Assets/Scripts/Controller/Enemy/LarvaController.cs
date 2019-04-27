using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    //スクリプト
    private BossFunction _bossFunction;
    private MoveBetweenTwoPoints _move;
    private BulletFunctions _bulletFunction;

    //固有弾
    [SerializeField] private GameObject scales_Bullet;

    /* フェーズ1用 */
    private bool start_Routine1 = false;

    /* フェーズ2用 */
    private bool start_Routine2 = false;


	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _rigid = GetComponent<Rigidbody2D>();
        //スクリプトの取得
        _bossFunction = GetComponent<BossFunction>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _bulletFunction = GetComponent<BulletFunctions>();

	}
	
	// Update is called once per frame
	void Update () {

        switch (_bossFunction.now_Phase) {
            case 1: Phase1(); break;
            case 2: Phase2(); break;
        }	
	}


    //フェーズ1
    private void Phase1() {
        if (!start_Routine1) {
            start_Routine1 = true;
            StartCoroutine("Phase1_Routine");
        }
    }
    private IEnumerator Phase1_Routine() {
        //移動
        _move.Set_Status(0, 0.03f);
        _move.StartCoroutine("Move_Two_Points", new Vector3(140f, 32f, 0));
        yield return new WaitUntil(_move.End_Move);

        while (_bossFunction.now_Phase == 1) {
            //弾の発射
            _bulletFunction.Set_Bullet(scales_Bullet);
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 15; j++) {
                    Scales_Bullet();
                }
                yield return new WaitForSeconds(1.0f);
            }
            //移動
            _move.Set_Status(-64f, 0.01f);
            _move.StartCoroutine("Move_Two_Points", new Vector3(-140f * transform.localScale.x, 32f, 0));
            yield return new WaitUntil(_move.End_Move);
            //向きの変更
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            yield return new WaitForSeconds(1.0f);
        }
    }

    //鱗粉弾の発射
    private void Scales_Bullet() {
        var bullet = Instantiate(scales_Bullet) as GameObject;
        bullet.transform.position = transform.position;
        Vector2 v = new Vector2(Random.Range(-200f, 200f), Random.Range(50f, 200f));
        bullet.GetComponent<Rigidbody2D>().velocity = v;
        Destroy(bullet, 5.0f);
    }


    //フェーズ2
    private void Phase2() {
        if (!start_Routine2) {
            start_Routine2 = true;
            StopCoroutine("Phase1_Routine");
            StartCoroutine("Phase2_Routine");
        }
    }
    private IEnumerator Phase2_Routine() {
        yield return new WaitForSeconds(1.0f);
        //移動
        _move.Set_Status(0, 0.02f);
        _move.StartCoroutine("Move_Two_Points", new Vector3(150f, 0));
        //向き
        transform.localScale = new Vector3(1, 1, 1);
    }

}
