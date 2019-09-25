using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffRollManager : MonoBehaviour {

    //フィールド
    [SerializeField] private GameObject roll_Text_Obj;

    private bool is_End_Staff_Roll = false;
    private float scroll_Speed = 1.0f;


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
            Change_Scroll_Speed();
            roll_Text_Obj.transform.position += new Vector3(0, scroll_Speed);
            yield return null;
        }
        is_End_Staff_Roll = true;
    }


    //速度の変更
    private void Change_Scroll_Speed() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            scroll_Speed = 10.0f;
        }
        if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Jump)) {
            scroll_Speed = 1.2f;
        }
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
