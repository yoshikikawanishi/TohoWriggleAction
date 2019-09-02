using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTwoPoints  {
    
    //2点間の角度計算
    public float Cal_Angle_Two_Points(Vector2 start, Vector2 end) {
        Vector2 d = end - start;
        float rad = Mathf.Atan2(d.y, d.x);
        return rad * Mathf.Rad2Deg;
    }


    //特別版
    public float Cal_Angle_Two_Points2(Vector2 start, Vector2 end) {
        Vector2 d = end - start;
        float rad = Mathf.Atan2(d.x, d.y);
        return rad * Mathf.Rad2Deg;
    }

}
