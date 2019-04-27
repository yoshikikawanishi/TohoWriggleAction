using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossFunction : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;

    //体力
    public int life = 1;
    //フェーズごとの体力
    public List<int> phase_Life_Border = new List<int>();
    //現在のフェーズ
    public int now_Phase = 0;

    //フェーズ切り替え時のボム
    [SerializeField] private GameObject phase_Change_Bomb;
    //ボス撃破時のエフェクト
    [SerializeField] private GameObject clear_Effect;



	// Use this for initialization
	void Start () {
        //コンポーネントの取得
        _sprite = GetComponent<SpriteRenderer>();
        
        //Damage()内のforループ用
        phase_Life_Border.Add(0);
        
	}


    //被弾の検知
    private void OnTriggerEnter2D(Collider2D collision) {
        //自機の弾に当たった時
        if (collision.tag == "PlayerBulletTag") {
            Damaged(1);
        }
        //キックに当たった時
        else if (collision.tag == "PlayerAttackTag") {
            Damaged(5);
        }
    }


    //被弾時の処理
    private void Damaged(int damage) {
        life -= damage;
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


    //被弾時の点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);
    }


    //フェーズ切り替え時の処理
    private void Phase_Change(int next_Phase) {
        //弾消し用のボム生成
        //var bomb = Instantiate(phase_Change_Bomb) as GameObject;
        now_Phase = next_Phase;
    }


    //クリア時の処理
    private void Clear() {
        Debug.Log("Clear");
        //エフェクト
        //var effect = Instantiate(vanished_Bomb) as GameObject;
        
    }

}
