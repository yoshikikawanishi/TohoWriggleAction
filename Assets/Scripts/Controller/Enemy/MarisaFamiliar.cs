using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaFamiliar : MonoBehaviour {
    

    //使い魔
    //内側
    private GameObject[] familiars = new GameObject[5];
    private SpiralBulletFunction[] familiar_Spiral_Bullet = new SpiralBulletFunction[5];    //赤、青、緑、黄、紫
    //外側
    private GameObject[] outside_Familiars = new GameObject[5];
    private SpiralBulletFunction[] outside_Spiral_Bullet = new SpiralBulletFunction[5]; //赤、青、緑、黄、紫
    //コンポーネント
    private SpriteRenderer[] familiars_Sprite = new SpriteRenderer[10];
    private CircleCollider2D[] familiars_Collider = new CircleCollider2D[10];


    // Use this for initialization
    void Awake () {
        //取得
	    for(int i = 0; i < 5; i++) {
            familiars[i] = transform.GetChild(i).gameObject;
            familiar_Spiral_Bullet[i] = familiars[i].GetComponent<SpiralBulletFunction>();

            outside_Familiars[i] = transform.GetChild(i + 5).gameObject;
            outside_Spiral_Bullet[i] = outside_Familiars[i].GetComponent<SpiralBulletFunction>();

            familiars_Sprite[i] = familiars[i].GetComponent<SpriteRenderer>();
            familiars_Sprite[i + 5] = outside_Familiars[i].GetComponent<SpriteRenderer>();

            familiars_Collider[i] = familiars[i].GetComponent<CircleCollider2D>();
            familiars_Collider[i + 5] = outside_Familiars[i].GetComponent<CircleCollider2D>();
        }        
	}


    //出現
    public IEnumerator Appear() {
        int i = 0;

        for (i = 0; i < 10; i++) {
            familiars_Sprite[i].color = familiars_Sprite[i].color - new Color(0, 0, 0, 1);
            familiars_Collider[i].enabled = false;
        }
        
        while (familiars_Sprite[0].color.a <= 1) {
            foreach (SpriteRenderer sr in familiars_Sprite) {
                sr.color += new Color(0, 0, 0, 0.002f);
            }
            yield return new WaitForSeconds(0.015f);
        }

        for(i = 0; i < 10; i++) {
            familiars_Collider[i].enabled = true;
        }
    }


    //弾のセット
    public void Set_Bullet_Pools(ObjectPool[] bullet_Pool) {
        for(int i = 0; i < 5; i++) {
            familiar_Spiral_Bullet[i].Set_Bullet_Pool(bullet_Pool[i]);
            outside_Spiral_Bullet[i].Set_Bullet_Pool(bullet_Pool[i]);
        }
    }

    //弾の生成開始
    public void Start_Spiral_Bullets() {
        //内側
        AngleTwoPoints angle = new AngleTwoPoints();
        float start_Angle = angle.Cal_Angle_Two_Points2(transform.position, familiars[0].transform.position);
        for(int i = 0; i < 5; i++) {
            familiar_Spiral_Bullet[i].Start_Spiral_Bullet(80f, start_Angle + i * 72f, 4f, 0.25f, 10.0f);
        }
        //外側;
        for(int i = 0; i < 5; i++) {
            outside_Spiral_Bullet[i].Start_Spiral_Bullet(-30f, 60f, 0f, 0.1f, 5.0f);
        }
    }

    
    //弾の生成終了
    public void Stop_Spiral_Bullets() {
        foreach (SpiralBulletFunction s in familiar_Spiral_Bullet) {
            s.Stop_Spiral_Bullet();
        }
        foreach(SpiralBulletFunction s in outside_Spiral_Bullet) {
            s.Stop_Spiral_Bullet();
        }
    }
}
