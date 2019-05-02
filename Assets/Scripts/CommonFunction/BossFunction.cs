using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossFunction : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;
    private AudioSource damage_Audio_Source;
    [SerializeField] AudioClip damage_Sound;
    //スクリプト
    private ObjectPool _pool;

    //体力
    public int life = 1;
    //フェーズごとの体力
    [SerializeField] private List<int> phase_Life_Border = new List<int>();
    //現在のフェーズ
    private int now_Phase = -1;

    //クリア検知用
    private bool clear_Trigger = false;

    //フェーズ切り替え時のボム
    [SerializeField] private GameObject phase_Change_Bomb;
    //ボス撃破時のエフェクト
    [SerializeField] private GameObject clear_Effect;
    //ダメージエフェクト
    private GameObject damage_Effect;



	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        //ダメージサウンドの設定
        damage_Audio_Source = gameObject.AddComponent<AudioSource>();
        damage_Audio_Source.clip = damage_Sound;
        damage_Audio_Source.volume = 0.008f;

        //ダメージエフェクトのオブジェクトプール
        _pool = gameObject.AddComponent<ObjectPool>();
        damage_Effect = Resources.Load("Effect/BossDamagedEffect") as GameObject;
        _pool.CreatePool(damage_Effect, 10);

        //Damage()内のforループ用
        phase_Life_Border.Add(0);
        
	}


    //被弾の検知
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾に当たった時
        if (collision.tag == "PlayerBulletTag") {
            Damaged(1);
            StartCoroutine("Damaged_Effect", collision.transform.position);
        }
        //キックに当たった時
        else if (collision.tag == "PlayerAttackTag") {
            Damaged(5);
        }
    }


    //被弾時の処理
    private void Damaged(int damage) {
        life -= damage;
        //効果音
        damage_Audio_Source.Play();
        //点滅
        StartCoroutine("Blink");
        //フェーズ切り替え
        int n = phase_Life_Border.Count();
        for(int i = 0; i < n-1; i++) {
            if(now_Phase != i+2 && phase_Life_Border[i+1] < life && life < phase_Life_Border[i]) {
                Phase_Change(i + 2);    /* phase_Life_Border[0]以上[1]以下になった時フェーズを2に変える */
            }
        }   
        //体力0でクリア
        if(life < 0) {
            Clear();
            //無敵化
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        }
    }


    //ダメージエフェクトの生成
    private IEnumerator Damaged_Effect(Vector3 pos) {
        var effect = _pool.GetObject();
        effect.transform.position = pos;
        yield return new WaitForSeconds(0.25f);
        effect.SetActive(false);
    }


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.02f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.02f);
    }


    //フェーズ切り替え時の処理
    private void Phase_Change(int next_Phase) {
        //弾消し用のボム生成
        var bomb = Instantiate(phase_Change_Bomb) as GameObject;
        bomb.transform.position = transform.position;
        now_Phase = next_Phase;
    }


    //クリア時の処理
    private void Clear() {
        now_Phase = -1;
        clear_Trigger = true;
        //エフェクト
        var effect = Instantiate(clear_Effect) as GameObject;
        effect.transform.position = transform.position;
        var bomb = Instantiate(phase_Change_Bomb) as GameObject;
        bomb.transform.position = transform.position;
    }

    //他からのクリア検知用
    public bool Clear_Trigger() {
        if (clear_Trigger) {
            clear_Trigger = false;
            return true;
        }
        return false;
    }


    //now_PhaseのGetter
    public int Get_Now_Phase() {
        return now_Phase;
    }
    //Setter
    public void Set_Now_Phase(int num) {
        now_Phase = num;
    }

}
