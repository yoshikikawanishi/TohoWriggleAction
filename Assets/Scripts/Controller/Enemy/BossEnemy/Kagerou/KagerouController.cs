using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouController : MonoBehaviour {

    //コンポーネント
    private KagerouAttack _attack;
    private BossEnemyController boss_Controller;
    private Animator _anim;
    private CameraShake _shake;

    //スクリプト
    private Stage5_BossMovie stage_Movie;

    //戦闘開始
    private bool start_Battle = false;

    //バックデザイン
    [SerializeField] private GameObject back_Design;


	// Use this for initialization
	void Start () {
        //取得
        _attack = GetComponent<KagerouAttack>();
        boss_Controller = GetComponent<BossEnemyController>();
        _anim = GetComponent<Animator>();
        _shake = gameObject.AddComponent<CameraShake>();
        stage_Movie = GameObject.FindWithTag("ScriptsTag").GetComponent<Stage5_BossMovie>();

        //テスト
        Debug.Log("Boss Test");
        boss_Controller.Set_Now_Phase(1);
    }


    // Update is called once per frame
    void Update() {
        if (start_Battle) {
            switch (boss_Controller.Get_Now_Phase()) {
                case 1: _attack.Phase1(); break;
                case 2: _attack.Phase2(); break;
                case 3: _attack.Phase3(); break;
                case 4: _attack.Phase4(); break;
                case 5: _attack.Phase5(); break;
            }
        }

        //クリア時
        if (boss_Controller.Clear_Trigger()) {
            stage_Movie.Start_Clear_Movie();
            _attack.Stop_Phase5();
        }
    }


    //戦闘開始
    public void Start_Battle() {
        start_Battle = true;
    }


    //咆哮
    public void Roar() {
        GameObject roar_Effect = transform.Find("RoarEffect").gameObject;
        roar_Effect.GetComponent<ParticleSystem>().Play();
        roar_Effect.GetComponent<AudioSource>().Play();
    }

    //咆哮効果音
    public void Roar_Sound() {
        GameObject roar_Effect = transform.Find("RoarEffect").gameObject;
        roar_Effect.GetComponent<AudioSource>().Play();
    }

    //変身エフェクト
    public void Transform_Effect() {
        GameObject transform_Effect = transform.Find("TransformEffect").gameObject;
        transform_Effect.GetComponent<ParticleSystem>().Play();
    }

    //画面揺らす
    public void Shake_Camera(float duration, float magunitude) {
        _shake.Shake(duration, magunitude, true);
    }

    //パワーチャージエフェクト
    public void Play_Charge_Effect(float span) {
        var charge_Effect = Instantiate(Resources.Load("Effect/PowerChargeEffectsRed") as GameObject);
        charge_Effect.transform.position = transform.position;
        Destroy(charge_Effect, span);
    }

    //パワー放出エフェクト
    public void Play_Spread_Effect() {
        var spread_Effect = Instantiate(Resources.Load("Effect/PowerSpreadEffectRed") as GameObject);
        spread_Effect.transform.position = transform.position;
        Destroy(spread_Effect, 3.0f);
    }

    //バックデザイン出す
    public void Appear_Back_Design(Vector3 pos, Color color) {
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        back_Design.transform.position = pos;
        for(int i = 0; i < back_Design.transform.childCount; i++) {
            back_Design.transform.GetChild(i).GetComponent<SpriteRenderer>().color = color;
        }
        back_Design.SetActive(true);
    }

    //バックデザイン消す
    public void Delete_Back_Design() {
        back_Design.SetActive(false);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
    }

    //アニメーション変更
    public void Change_Parametar(string change_Bool, int scale_X) {
        _anim.SetBool("IdleBool", false);
        _anim.SetBool("RushBool", false);
        _anim.SetBool("DashBool", false);
        _anim.SetBool("RoarBool", false);
        _anim.SetBool("Idle2Bool", false);

        _anim.SetBool(change_Bool, true);
        transform.localScale = new Vector3(scale_X, transform.localScale.y, 1);
    }
}
