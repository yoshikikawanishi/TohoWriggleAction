using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuLastBombEffect : MonoBehaviour {

    private int CIRCLE_EFFECT_NUM = 6;
    private float CIRCLE_RADIAN = 32f;

	// Use this for initialization
	void Start () {
        StartCoroutine("Effect_Routine");
    }
	
    private IEnumerator Effect_Routine() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        //喰らいボム
        Time.timeScale = 0;
        for(float t = 0; t < 0.1f; t += 0.02f) { yield return null; }
        Time.timeScale = 1;
        GameObject.Find("RedCover").GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 0.1f);
        yield return new WaitForSeconds(1.5f);
        //霊夢の周りにエフェクト生成
        CircleDeposit inside_Circle = new CircleDeposit(transform.position, 0, CIRCLE_EFFECT_NUM, CIRCLE_RADIAN);
        List<GameObject> obj_List1 = new List<GameObject>();
        List<GameObject> obj_List2 = new List<GameObject>();
        for (int i = 0; i < CIRCLE_EFFECT_NUM; i++) {
            GameObject obj = Instantiate(Resources.Load("Effect/ReimuLastBomb")) as GameObject;
            obj.transform.position = inside_Circle.Get_Circle_Pos()[i];
            obj_List1.Add(obj);
        }
        CircleDeposit outside_Circle = new CircleDeposit(transform.position, 0, CIRCLE_EFFECT_NUM, CIRCLE_RADIAN * 2);
        for (int i = 0; i < CIRCLE_EFFECT_NUM; i++) {
            GameObject obj = Instantiate(Resources.Load("Effect/ReimuLastBomb")) as GameObject;
            obj.transform.position = outside_Circle.Get_Circle_Pos()[i];
            obj_List2.Add(obj);
        }
        //霊夢の周りを回す
        StartCoroutine(Rotate_Effect(obj_List1, obj_List2));
        //リグルに衝撃エフェクト
        for(int i = 0; i < 7; i++) {
            GameObject effect = Instantiate(Resources.Load("Effect/CalmBurstEffect") as GameObject);
            effect.transform.position = player.transform.position;
            GameObject color_Effect = Instantiate(Resources.Load("Effect/CalmBurstEffect") as GameObject);
            color_Effect.transform.position = new Vector3(Random.Range(-200f, 160f), Random.Range(-100f, 100f));
            ParticleSystem.MainModule pm = color_Effect.GetComponent<ParticleSystem>().main;
            pm.startColor = new Color(i / 7f, (7f - i) / 7f, i / 14f);
            UsualSoundManager.Shot_Sound();
            CameraShake _shake = gameObject.AddComponent<CameraShake>();
            _shake.Shake(0.25f, 3);
            yield return new WaitForSeconds(0.6f);
        }
    }


    //霊夢の周りを回す
    private IEnumerator Rotate_Effect(List<GameObject> inside_Circle_List, List<GameObject> outside_Circle_List) {
        float standard_Angle = 0;
        for (float t = 0; t < 5.0f; t += Time.deltaTime) {
            for(int i = 0; i < CIRCLE_EFFECT_NUM; i++) {
                float angle = standard_Angle + i * (2 * Mathf.PI / CIRCLE_EFFECT_NUM);
                inside_Circle_List[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * CIRCLE_RADIAN;
                outside_Circle_List[i].transform.position = transform.position + new Vector3(Mathf.Cos(-angle), Mathf.Sin(-angle), 0) * CIRCLE_RADIAN * 2;
            }
            standard_Angle += 0.05f;
            yield return null;
        }
        //消す
        for(int i = 0; i < CIRCLE_EFFECT_NUM; i++) {
            Destroy(inside_Circle_List[i]);
            Destroy(outside_Circle_List[i]);
        }
    }

}
