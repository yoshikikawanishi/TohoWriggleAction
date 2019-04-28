using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinderBulletFunction : BulletFunctions {

    //フィールド変数
    private float time = 0;


    //ワインダー
    public void Winder_Bullet(int num, float interAngle, float speed, float span, float lifeTime) {
        if (time < span) {
            span += Time.deltaTime;
        }
        else {
            base.Even_Num_Bullet(num, interAngle, speed, lifeTime);
        }
    }

}
