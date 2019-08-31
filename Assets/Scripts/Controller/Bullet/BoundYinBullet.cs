using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundYinBullet : Enemy {

    //コンポネント
    private Rigidbody2D _rigid;


    // Use this for initialization
    new void Start() {
        base.Start();
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
        //初期値
        default_Color = new Color(1, 1, 1);
        //回転
        _rigid.angularVelocity = 500f;
    }


    //消滅時
    override protected void Vanish() {
        //エフェクトの生成
        GameObject effect = Instantiate(vanish_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, 1.0f);
        //点とPと回復アイテムの生成
        Put_Out_Item();
        //分裂、消滅
        Division();
    }


    //分裂
    private void Division() {
        GameObject small_Bullet = Resources.Load("Bullet/SmallYinBallBullet") as GameObject;
        BulletFunctions bf = gameObject.AddComponent<BulletFunctions>();
        bf.Set_Bullet(small_Bullet);
        bf.Even_Num_Bullet(6, 60f, 100f, 5.0f);
        UsualSoundManager.Shot_Sound();
        Destroy(gameObject);
    }

}
