using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//霊夢の攻撃をするクラス
public class ReimuAttack : MonoBehaviour {

    private bool start_Phase1_Routine = true;
    private bool start_Phase2_Routine = true;

    
    //フェーズ1
    public void Phase1() {
        if (start_Phase1_Routine) {
            start_Phase1_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }


    //フェーズ1のルーチン
    private IEnumerator Phase1_Routine() {
        MoveBetweenTwoPoints _move = gameObject.AddComponent<MoveBetweenTwoPoints>();
        BulletFunctions _bullet = gameObject.AddComponent<BulletFunctions>();
        ReimuController _controller = GetComponent<ReimuController>();
        //移動
        _controller.Change_Parameter("DashBool");
        _move.Start_Move(new Vector3(180f, -64f), 64f, 0.015f);
        yield return new WaitUntil(_move.End_Move);
        _controller.Change_Parameter("AttackBool");
        transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(1.0f);
        //ホーミング弾
        GameObject homing_Bullet = Resources.Load("Bullet/ReimuHomingBullet") as GameObject;
        for(int i = 0; i < 4; i++) {
            _bullet.Set_Bullet(homing_Bullet);
            _bullet.Some_Way_Bullet(4, 150f, 0, 30f, 4.0f);
            Vector3 next_Pos = transform.position + new Vector3(Random.Range(-32f, 32f), Random.Range(-2f, 24f));
            _move.Start_Move(next_Pos, 0, 0.03f);
            yield return new WaitUntil(_move.End_Move);
        }
        //陰陽玉
        _move.Start_Move(new Vector3(150f, 0, 0), 0, 0.02f);
        _controller.Change_Parameter("AvoidBool");
        yield return new WaitForSeconds(3.0f);  //溜め
        _controller.Change_Parameter("AttackBool");
        GameObject yin_Bullet = Resources.Load("Bullet/BigYinBallBullet") as GameObject;
        _bullet.Set_Bullet(yin_Bullet);
        _bullet.Odd_Num_Bullet(1, 0, 200f, 0);
        yield return new WaitForSeconds(2.0f);
        //初めから
        start_Phase1_Routine = true;
    }

    //フェーズ2
    public void Phase2() {
        if (start_Phase2_Routine) {
            start_Phase2_Routine = false;
            StopAllCoroutines();
        }
    }


}
