using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRollManager : MonoBehaviour {

    //フィールド
    [SerializeField] private GameObject roll_Text_Obj;

    private bool is_End_Staff_Roll = false;
    private float scroll_Speed = 0.45f;


    /// <summary>
    /// スタッフロールの開始
    /// </summary>
    public void Start_Staff_Roll() {
        is_End_Staff_Roll = false;
        StartCoroutine("Scroll_Staff_Roll");
    }


    //スタッフロール
    private IEnumerator Scroll_Staff_Roll() {
        while (roll_Text_Obj.transform.position.y < 1000f) {
            roll_Text_Obj.transform.position += new Vector3(0, scroll_Speed);
            yield return null;
        }
        is_End_Staff_Roll = true;
    }


    /// <summary>
    /// スタッフロールの終了検知用
    /// </summary>
    /// <returns>終了したかどうか</returns>
    public bool Is_End_Staff_Roll() {
        if (is_End_Staff_Roll) {
            is_End_Staff_Roll = false;
            return true;
        }
        return false;
    }

}
