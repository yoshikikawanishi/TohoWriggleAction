using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour {


    //OnEnable
    private void OnEnable() {
        StartCoroutine("Wait_One_Frame");
    }


    private IEnumerator Wait_One_Frame() {
        yield return null;
        //ボタン選択
        transform.GetChild(0).GetComponent<Button>().Select();
    }
}
