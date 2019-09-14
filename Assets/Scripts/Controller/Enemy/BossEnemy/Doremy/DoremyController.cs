using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoremyController : MonoBehaviour {

    //スクリプト
    private BossEnemyController boss_Controller;
    private DoremyAttack _attack;
    private MoveBetweenTwoPoints _move;
    //コンポーネント
    private Animator _anim;

    //戦闘開始
    public bool start_Battle = false;


    //Awake
    private void Awake() {
        //取得
        boss_Controller = GetComponent<BossEnemyController>();
        _attack = GetComponent<DoremyAttack>();
        _move = GetComponent<MoveBetweenTwoPoints>();
        _anim = GetComponent<Animator>();
    }


    // Use this for initialization
    void Start () {
        //テスト用
        Debug.Log("Boss Battle Test");
        boss_Controller.Set_Now_Phase(1);
	}
	

	// Update is called once per frame
	void Update () {
        //攻撃
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
                case 3: _attack.Phase3(); break;
                case 4: _attack.Phase4(); break;
                case 5: _attack.Phase5(); break;
                case 6: _attack.Phase6(); break;
            }
        }
	}


    //アニメーション変更
    public void Change_Parameter(string next_Bool) {

    }


    //移動
    public void Move(Vector2 next_Pos, float speed) {
        _move.Start_Move(next_Pos, 0, speed);
    }
    public bool End_Move() {
        return _move.End_Move();
    }

    public void Move_Randome() {
        _move.Start_Random_Move(32f, 0.01f);
    }


    //レイヤー変更
    public void Change_Layer(string layer_Name) {
        gameObject.layer = LayerMask.NameToLayer(layer_Name);
    }

    //瞬間移動
    public void Start_Warp(Vector2 next_Pos) {
        StartCoroutine("Warp", next_Pos);
    }

    public IEnumerator Warp(Vector2 next_Pos) {
        //エフェクト
        Debug.Log("Warp_Effect");
        yield return new WaitForSeconds(0.5f);
        transform.position = next_Pos;
    }
}
