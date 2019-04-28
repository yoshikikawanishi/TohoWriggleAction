using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDeposit  {

    /* フィールド変数 */
    private Vector2 center_Pos;
    private float center_Angle;
    private int num;


    //コンストラクタ
    public CircleDeposit(Vector2 center_Pos, float center_Angle, int num) {
        this.center_Pos = center_Pos;
        this.center_Angle = center_Angle;
        this.num = num;
    }


    //center_Pos中心の半径1の円周をnum等分した時の座標リストを返す
	public List<Vector2> Circle_Deposit() {
        List<Vector2> list = new List<Vector2>();
        for(int i = 0; i < num; i++) {
            float angle = center_Angle + i * (2 * Mathf.PI) / num;
            Vector2 pos = center_Pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            list.Add(pos);
        }
        return list;
    }
}
