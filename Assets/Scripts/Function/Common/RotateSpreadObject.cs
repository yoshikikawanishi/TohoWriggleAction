using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpreadObject : MonoBehaviour {

    //フィールド
    [SerializeField] private Vector3 center_Pos;
    [SerializeField] private float rotate_Speed;
    [SerializeField] private float spread_Speed;

    [SerializeField] private float angle = 0;
    [SerializeField] private float radius = 0;


    //フィールド変数の代入用
    public void Set_Status(Vector2 center_Pos, float rotate_Speed, float spread_Speed) {
        this.center_Pos = center_Pos;
        this.rotate_Speed = rotate_Speed;
        this.spread_Speed = spread_Speed;

        AngleTwoPoints _angle = new AngleTwoPoints();
        angle = _angle.Cal_Angle_Two_Points(center_Pos, transform.position);
        radius = Vector2.Distance(center_Pos, transform.position);
    }


	// Update is called once per frame
	void Update () {
        transform.position = center_Pos + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        angle = angle + rotate_Speed;
        radius = radius + spread_Speed;
	}


}
